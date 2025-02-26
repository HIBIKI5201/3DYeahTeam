using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _mixer;

    /// <summary>
    /// 各オーディオグループの種類
    /// </summary>
    public enum AudioType
    {
        Master,
        BGM,
        SE,
    }

    private Dictionary<AudioType, (AudioMixerGroup group, AudioSource source, float originalVolume)> _audioDict = new();

    [SerializeField] private List<AudioData> _bgmList = new();
    private CancellationTokenSource _bgmChangeToken;

    [Serializable]
    private class AudioData
    {
        public float Volume = 1;
        public AudioClip Clip = default;
    }

    [SerializeField]
    private List<AudioData> _soundEffectList = new();

    private void Awake()
    {
        AudioSourceInit();

        if (_audioDict.TryGetValue(AudioType.BGM, out var data))
        {
            data.source.loop = true;
        }
    }

    /// <summary>
    /// AudioDictの初期化
    /// </summary>
    private void AudioSourceInit()
    {
        if (!_mixer)
        {
            Debug.LogWarning("オーディオミキサーがアサインされていません");
            return;
        }

        foreach (AudioType type in Enum.GetValues(typeof(AudioType)))
        {
            //Enumの名前を出す
            string name = type.ToString();
            if (string.IsNullOrEmpty(name))
            {
                continue;
            }

            //ミキサーグループを取得する
            AudioMixerGroup group = _mixer.FindMatchingGroups(name).FirstOrDefault();
            if (group)
            {
                AudioSource source = gameObject.AddComponent<AudioSource>();
                source.outputAudioMixerGroup = group;
                source.playOnAwake = false;

                //初期のボリュームを取得
                if (_mixer.GetFloat($"{name}_Volume", out float value))
                {
                    //各情報を追加
                    _audioDict.Add(type, (group, source, value));
                }
                else
                {
                    Debug.LogWarning($"{name}_Volume is not found");
                }
            }
            else
            {
                Debug.LogWarning($"{name} is not a valid AudioMixerGroup.");
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    public void VolumeSliderChanged(AudioType type, float value)
    {
        if (value < 0 || 1 < value)
        {
            Debug.LogWarning("入力は無効な値です");
            return;
        }

        //デシベルで音量を割合変更
        float db = value * (_audioDict[type].originalVolume + 80) - 80;

        _mixer.SetFloat(type.ToString(), db);
    }

    /// <summary>
    /// ミキサーグループを出す
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public AudioMixerGroup GetMixerGroup(AudioType type) => _audioDict[type].group;

    public async Task BGMFadeOut(float duration, CancellationToken token = default)
    {
        AudioSource source = _audioDict[AudioType.BGM].source;

        while (source.volume > 0)
        {
            source.volume -= 1 / (duration / 2) * Time.deltaTime;
            await Awaitable.NextFrameAsync(token);
        }

        source.Stop();
    }

    public async Task BGMFadeIn(float duration, float volume, CancellationToken token = default)
    {
        AudioSource source = _audioDict[AudioType.BGM].source;

        source.Play();

        while (source.volume < volume)
        {
            source.volume += 1 / (duration / 2) * Time.deltaTime * volume;
            await Awaitable.NextFrameAsync(token);
        }
    }


    /// <summary>
    /// BGMを変更する
    /// </summary>
    /// <param name="index"></param>
    /// <param name="duration"></param>
    public async void BGMChanged(int index, float duration)
    {
        if (_bgmList.Count <= index)
        {
            Debug.LogWarning("入力されたインデックスはBGMリストの範囲外です");
            return;
        }

        //前のBGM変更があれば止める
        if (_bgmChangeToken is { IsCancellationRequested: false })
        {
            _bgmChangeToken.Cancel();
        }

        //トークンを生成する
        _bgmChangeToken = new();
        var token = _bgmChangeToken.Token;

        AudioSource source = _audioDict[AudioType.BGM].source;
        var data = _bgmList[index];

        //BGMをフェードアウト
        try
        {
            await BGMFadeOut(duration, token);
        }
        finally
        {
            source.volume = 0;

            //新たなクリップに差し替え
            source.clip = data.Clip;
        }


        //BGMをフェードイン
        try
        {
            await BGMFadeIn(duration, data.Volume, token);
        }
        finally
        {
            source.volume = data.Volume;
        }
    }

    public void PlaySoundEffect(int index)
    {
        if (_soundEffectList.Count <= index)
        {
            Debug.LogWarning("入力されたインデックスはSEリストの範囲外です");
            return;
        }

        //データを取得して再生
        AudioSource source = _audioDict[AudioType.SE].source;
        var data = _soundEffectList[index];

        source.volume = data.Volume;
        source.PlayOneShot(data.Clip);
    }
}

using SymphonyFrameWork.System;
using System.Collections;
using UnityEngine;

public class ChargeManager : MonoBehaviour
{
    private float _pushCounter;
    public float PushCounter { get => _pushCounter; }
    private float _timer;
    public float Timer { get => _timer; }

    [SerializeField]
    private float _pushUp = 3;
    [SerializeField]
    private float _waitForSecondsDown = 0.1f;

    [Space(10)]
    [SerializeField]
    private float _timeLimit = 5;
    public float TimeLimit { get => _timeLimit; }

    private bool _chargeFinish = false;
    public bool ChargeFinish { get => _chargeFinish; }

    private AudioManager _audioManager;

    public void Phase3Init()
    {
        _timer = Time.time;

        _audioManager = ServiceLocator.GetInstance<AudioManager>();
        _audioManager.PlaySoundEffect(16);

        //アップデート用コルーチン起動
        StartCoroutine(ChargeManaUpdate());
    }

    /// <summary>
    /// ChargeManaのupdate関数として使用
    /// </summary>
    private IEnumerator ChargeManaUpdate()
    {
        while (!_chargeFinish)
        {
            if (Time.time > _timer + _timeLimit && !_chargeFinish)
            {
                Phase3End();
                Debug.Log("aaa");
            }

            if (Input.GetKeyDown(KeyCode.Space) && !_chargeFinish)
            {
                OnChangePushCounter();
            }

            //時間経過でカウントが減っていく処理
            if (_pushCounter > 0 && !_chargeFinish)
            {
                _pushCounter -= _waitForSecondsDown * Time.deltaTime;
            }
            yield return null;
        }
        yield break;
    }

    /// <summary>
    /// ChargeCountの加算加減処理
    /// </summary>
    public void OnChangePushCounter()
    {
        _audioManager.PlaySoundEffect(2);
        _pushCounter += _pushUp;
        Debug.Log($"現在値は　{_pushCounter}");
    }
    private void Phase3End()
    {
        _chargeFinish = true;

        IngameSystem system = ServiceLocator.GetInstance<IngameSystem>();
        system.CucumberData.Phase3Data = _pushCounter;//一旦そのままデータを代入しているが、後々スコアにするために計算すると思う
        system.NextPhaseEvent();
    }
}

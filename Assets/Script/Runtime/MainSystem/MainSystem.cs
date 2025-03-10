﻿using SymphonyFrameWork.Attribute;
using SymphonyFrameWork.System;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MainSystem : MonoBehaviour
{
    private MainUI _mainUI;

    private Task _loadTask = Task.CompletedTask;

    private SceneListEnum _nowScene;
    public SceneListEnum NowScene { get => _nowScene; }

    private Volume _volume;
    public Volume Volume { get => _volume; }

#if UNITY_EDITOR
    [DisplayText("以下はデバッグ機能")]
    [SerializeField] private SceneListEnum _targetScene;

    [ContextMenu("SceneChange")]
    private void DebugSceneChange() => SceneChange(_targetScene);
#endif
    private void Awake()
    {
        _mainUI = GetComponent<MainUI>();
        _volume = GetComponent<Volume>();

        Debug.Log($"ランキングデータを確認\n{string.Join("\n", SaveDataSystem<RankingData>.Data.Datas)}");

        //現在のシーンを保存する
        Scene scene = SceneManager.GetActiveScene();
        if (!Enum.TryParse(scene.name, out _nowScene))
        {
            Debug.LogWarning($"Scene '{scene.name}' is not a valid scene.");
        }

#if UNITY_EDITOR
        //インゲームのフェーズから始めた場合の処理
        if (_nowScene == SceneListEnum.IngamePhase_1 ||
            _nowScene == SceneListEnum.IngamePhase_2 ||
            _nowScene == SceneListEnum.IngamePhase_3 ||
            _nowScene == SceneListEnum.IngamePhase_Result)
        {
            LoadIngameScene();

            //インゲームを非同期でロード
            async void LoadIngameScene()
            {
                await SceneLoader.LoadScene(SceneListEnum.Ingame.ToString());
                SceneLoader.SetActiveScene(SceneListEnum.Ingame.ToString());
            }
        }
#endif
    }

    public async void SceneChange(SceneListEnum scene)
    {
        if (!_loadTask.IsCompleted)
        {
            Debug.LogWarning("既にロードが開始されています");
            return;
        }

        _loadTask = LoadScene();

        await _loadTask;

        async Task LoadScene()
        {
            await _mainUI.FadeOut(1f);

            // 今のシーンをアンロード
            await SceneLoader.UnloadScene(_nowScene.ToString());

            // 次のシーンをロードする
            _nowScene = scene;
            await SceneLoader.LoadScene(scene.ToString());
            SceneLoader.SetActiveScene(scene.ToString());

            await _mainUI.FadeIn(1f);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("ランキングをリセット")]
    private void RankingReset()
    {
        SaveDataSystem<RankingData>.Data.Datas.Clear();
        SaveDataSystem<RankingData>.Save();
        Debug.Log($"{SaveDataSystem<RankingData>.Data.Datas.ToString()}をリセットしました");
    }
#endif
}

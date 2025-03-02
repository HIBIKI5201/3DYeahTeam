using UnityEngine;
using SymphonyFrameWork.System;
using SymphonyFrameWork.Utility;

public class RotateCucumber : MonoBehaviour
{
    [SerializeField] private float _speed = 45f; // 回転速度

    private AudioManager _audioManager;
    private IngameSystem _ingameSystem;

    private GameObject _cucumber;
    private Renderer _renderer;

    private Vector3 _center; // モデルの中心座標

    private async void Awake()
    {
        await Awaitable.NextFrameAsync();

        _audioManager = ServiceLocator.GetInstance<AudioManager>();
        _ingameSystem = ServiceLocator.GetInstance<IngameSystem>();

        if (_ingameSystem != null)
        {
            await SymphonyTask.WaitUntil(() => _ingameSystem.Cucumber, destroyCancellationToken);
            _cucumber = _ingameSystem.Cucumber.CucumberModel;
            _renderer = _cucumber.GetComponent<Renderer>();
        }

        // きゅうりのモデル中心を取得（最初の値を固定）
        _center = _renderer.bounds.center;
    }

    private void Update()
    {
        // モデルの中心を基準にX軸回転
        _cucumber.transform.RotateAround(_center, Vector3.right, _speed * Time.deltaTime);
    }
}

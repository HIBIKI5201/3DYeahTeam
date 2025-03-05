using UnityEngine;
using SymphonyFrameWork.System;
using SymphonyFrameWork.Utility;
using ChargeShot.Runtime.Ingame;
using System;
using System.Collections.Generic;

public class RotateCucumber : MonoBehaviour
{
    [SerializeField] 
    private float _speed = 45f; // 回転速度
    [SerializeField]
    private Material _capMaterial;//

    private AudioManager _audioManager;
    private IngameSystem _ingameSystem;
    private AddDecal _addDecal;

    private GameObject _cucumber;
    private Vector3 _center; // モデルの中心座標


    private int _cutCount;
    private GameObject[] _piese;

    public event Action<List<GameObject>> OnCutEnd;

    private async void Start()
    {
        _audioManager = ServiceLocator.GetInstance<AudioManager>();
        _ingameSystem = ServiceLocator.GetInstance<IngameSystem>();
        _addDecal = GetComponent<AddDecal>();

        if (_ingameSystem != null)
        {
            await SymphonyTask.WaitUntil(() => _ingameSystem.Cucumber, destroyCancellationToken);
            _cucumber = _ingameSystem.Cucumber.gameObject;
        }

        // きゅうりのモデル中心を取得（最初の値を固定）
        _center = _cucumber.GetComponentInChildren<Renderer>().bounds.center;

        _piese = new GameObject[4];
        //0...一度目のleft
        //1...一度目のrihgt
        //2...二度目のleft
        //3...二度目のrihgt
    }

    private void Update()
    {
        // モデルの中心を基準にX軸回転
        if (_cucumber)
        {
            _cucumber?.transform?.RotateAround(_center, Vector3.right, _speed * Time.deltaTime);
        }
    }

    public void Phase2Cut()
    {
        if (_cutCount >= 2) return;

        GameObject[] pieces = MeshCutService.Cut(_ingameSystem.Cucumber.CucumberModel, transform.position, transform.up, _capMaterial);
        _audioManager.PlaySoundEffect(3);

        _cutCount++;
        _piese[0] = pieces[0];
        _piese[1] = pieces[1];

        MeshUtil.MeshColliderRefresh(_piese[0]);
        _piese[1].transform.parent = _cucumber.transform;
        _addDecal.OnDecal(_piese[1].transform, transform.position, transform.rotation);
    }
}

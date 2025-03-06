using UnityEngine;
using SymphonyFrameWork.System;
using SymphonyFrameWork.Utility;
using ChargeShot.Runtime.Ingame;
using System;

public class RotateCucumber : MonoBehaviour
{
    [SerializeField]
    private float _speed = 45f; // 回転速度
    [SerializeField]
    private Material _capMaterial;

    private AudioManager _audioManager;
    private IngameSystem _ingameSystem;
    private AddDecal _addDecal;

    private GameObject _cucumber;
    private Vector3 _center; // モデルの中心座標

    private int _cutCount;

    //一度目の切断後のオブジェクト
    private GameObject _leftPiese;
    private GameObject _rightPiese;

    //二度目の切断後のオブジェクト
    private GameObject[] _cutObject = new GameObject[4];
    //0...一度目のleft
    //1...一度目のrihgt
    //2...二度目のleft
    //3...二度目のrihgt

    public event Action<GameObject[]> OnCutEnd;

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


    }

    private void Update()
    {
        // モデルの中心を基準にX軸回転
        if (_cucumber && _cutCount < 2)
        {
            _cucumber?.transform?.RotateAround(_center, Vector3.right, _speed * Time.deltaTime);
        }
    }

    public void Phase2Cut()
    {
        if (_cutCount >= 2) return;

        GameObject[] pieces = new GameObject[2];

        if (_cutCount >= 1)
        {
            //二度目の切断、一度目に切断した両方のオブジェクトを切断する
            _cutCount++;
            pieces = MeshCutService.Cut(_leftPiese, transform.position, transform.up, _capMaterial);
            _audioManager.PlaySoundEffect(3);
            _cutObject[0] = pieces[0];
            _cutObject[1] = pieces[1];
            //メッシュの最適化
            MeshUtil.MeshColliderRefresh(_cutObject[0]);


            pieces = MeshCutService.Cut(_rightPiese, transform.position, transform.up, _capMaterial);
            _audioManager.PlaySoundEffect(3);
            _cutObject[2] = pieces[0];
            _cutObject[3] = pieces[1];

            MeshUtil.MeshColliderRefresh(_cutObject[2]);

            //デカールを貼り付け
            _addDecal.OnDecal(_rightPiese.transform, transform.position, transform.rotation);

            OnCutEnd.Invoke(_cutObject);

            return;
        }

        //一度目の切断
        _cutCount++;
        pieces = MeshCutService.Cut(_ingameSystem.Cucumber.CucumberModel, transform.position, transform.up, _capMaterial);
        _audioManager.PlaySoundEffect(3);

        _leftPiese = pieces[0];
        _rightPiese = pieces[1];

        //切ったオブジェクトのメッシュを最適化
        MeshUtil.MeshColliderRefresh(_leftPiese);
        _rightPiese.transform.parent = _cucumber.transform;

        //デカールを貼り付け
        _addDecal.OnDecal(_rightPiese.transform, transform.position, transform.rotation);
    }

    /// <summary>
    /// 回転のズレを計算して評価
    /// </summary>
    public void Distance(float currentYRotation)
    {
        var system = ServiceLocator.GetInstance<IngameSystem>();

        if (currentYRotation <= 50 && currentYRotation >= 35)
        {
            Debug.Log("perfectTiming");
            system.CucumberData.Phase2Data = 100;
        }
        else
        {
            if (currentYRotation >= 90) currentYRotation -= 45;
            else if (currentYRotation >= 180) currentYRotation -= 90;

            system.CucumberData.Phase2Data = Mathf.Abs(45 - currentYRotation);
            Debug.Log("badTiming point: " + system.CucumberData.Phase2Data);
        }
    }
}

using ChargeShot.Runtime.Ingame;
using SymphonyFrameWork.System;
using SymphonyFrameWork.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// カッターの管理クラス
/// </summary>
public class CutterMoveController : MonoBehaviour
{
    [Header("カッターの動き設定")]
    public float _speed = 2.0f; // 移動速度
    public float _maxPosition = 1.0f; // 右端
    public float _minPosition = -1.0f; // 左端

    [SerializeField, Header("きゅうり情報")] Material _capMaterial;
    private float _currentPositionX;
    private float _lastPositionX;
    private Vector3 _firstPos;

    [SerializeField, Header("ベストタイミング")] float[] _bestTimings = new float[2];
    private Vector3 _cuttingObjectPosition;
    List<float> _distanceList = new List<float>();

    GameObject[] _cuttingObject = new GameObject[3];
    private GameObject _targetObject;

    [SerializeField, Header("演出設定")]
    private Vector3 _knifeOffset = new Vector3(0, 100, 0);

    private bool _movingRight = true;
    private int _cutCount = 0;
    private float diffResult = 0;
    private float _result;

    public event Action<GameObject> OnCuttingFinish;

    private GameObject center;

    int initial = 0;
    private IngameSystem _ingameSystem;

    private AudioManager _audioManager;

    private void Awake()
    {
        _cuttingObject = new GameObject[3];
    }
    private async void Start()
    {
        _audioManager = ServiceLocator.GetInstance<AudioManager>();
        _cuttingObjectPosition = transform.position;

        await SymphonyTask.WaitUntil(() => SceneLoader.GetExistScene(SceneListEnum.SpaceShip.ToString(), out _));

        _ingameSystem = ServiceLocator.GetInstance<IngameSystem>();
        var cucumber = _ingameSystem.Cucumber.CucumberModel;
        MeshUtil.MeshColliderRefresh(cucumber);

        _targetObject = cucumber;
        var collider = _targetObject.GetComponent<MeshCollider>();
        collider.convex = true;
        collider.isTrigger = true;

        _firstPos = _ingameSystem.Cucumber.transform.position;

        var ship = ServiceLocator.GetInstance<SpaceShip>();
        if (ship)
        {
            var knife = ship.Knife;
            knife.transform.position = transform.position + _knifeOffset;
            knife.transform.parent = transform;
            knife.transform.rotation = Quaternion.Euler(3, -90, 5) * Quaternion.identity;
        }

        _ingameSystem.Cucumber.transform.position = new Vector3(20, 0, 20) + _cuttingObjectPosition;
    }
    private void Update()
    {
        MoveCutter();
        CuttingObject();
    }
    /// <summary>
    /// 
    /// </summary>
    private void CuttingObject()
    {
        if (2 <= _cutCount)
        {
            return;
        }

        if (!_targetObject)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            kiru();
        }
    }

    private void kiru()
    {
        _audioManager.PlaySoundEffect(3);
        var pieces = MeshCutService.Cut(_targetObject, this.transform.position, this.transform.right,
       _capMaterial);
        if (pieces == null) return;

        _cutCount++;

        //戻り値オブジェクトをリストや変数にいれて新規オブジェクトにBoxCollider付与
        var leftSide = pieces[0];
        var rightSide = pieces[1];

        if (_cutCount == 1)
        {
            _cuttingObject[0] = leftSide;
            _cuttingObject[2] = rightSide;

            _lastPositionX = _currentPositionX;
        }
        else
        {
            if (_lastPositionX < _currentPositionX)
            {
                _cuttingObject[1] = leftSide;
                _cuttingObject[2] = rightSide;
            }
            else
            {
                _cuttingObject[0] = leftSide;
                _cuttingObject[1] = rightSide;
            }
        }
        MeshUtil.MeshColliderRefresh(rightSide);
        _distanceList.Add(Mathf.Abs(_bestTimings[_cutCount - 1] - _currentPositionX));


        if (_cutCount == 1)
        {
            // 右側のオブジェクトを少し移動
            leftSide.transform.position += leftSide.transform.right * -300f;
        }
        else
        {
            // 左側のオブジェクトを少し移動
            rightSide.transform.position += rightSide.transform.right * 300f;
        }

        MakeDiff(rightSide, leftSide);

        if (_cutCount > 1)
        {
            StartCoroutine(nameof(SendIngameSystem));
        }
    }


    private void MakeDiff(GameObject right, GameObject left)
    {
        if (_cuttingObjectPosition.x > _firstPos.x)
        {
            diffResult += _cuttingObjectPosition.x - _bestTimings[0];
            _cuttingObjectPosition.x = 0;
            _maxPosition = 0;
            if (initial != 0) return;
        }
        else
        {
            diffResult += Mathf.Abs(_cuttingObjectPosition.x - _bestTimings[1]);
            _cuttingObjectPosition.x = 0;
            _minPosition = 0;
            if (initial != 0) return;
        }
        center = _cuttingObject[1];
        _result = Mathf.Abs((diffResult / 100) - 100);
    }

    private System.Collections.IEnumerator SendIngameSystem()
    {
        float distance = _distanceList.Sum();

        center.name = "center";
        Debug.Log($"{center.name}  : {center.gameObject.transform.position}");
        var inGameSystem = ServiceLocator.GetInstance<IngameSystem>();
        inGameSystem.CucumberData.Phase1Data = _result; //差分の合計値を一旦入れとく

        yield return new WaitForSeconds(3f);

        foreach (var data in _cuttingObject)
        {
            if (data.name == "center") continue;
            Destroy(data);
        }

        OnCuttingFinish?.Invoke(center);
        Destroy(gameObject);
    }

    /// <summary>
    /// カッターを左右に移動させる
    /// </summary>
    private void MoveCutter()
    {
        if (_cutCount >= 2) return;

        _cuttingObjectPosition.x = _currentPositionX;
        transform.position = _cuttingObjectPosition;
        if (_movingRight)
            _currentPositionX += _speed * Time.deltaTime;
        else
            _currentPositionX -= _speed * Time.deltaTime;

        if (_currentPositionX >= _maxPosition) _movingRight = false;
        if (_currentPositionX <= _minPosition) _movingRight = true;
    }

    private void OnDestroy()
    {
        ServiceLocator.DestroyInstance<CutterMoveController>();
    }
    /// <summary>
    /// 現在の位置を取得
    /// </summary>
    public float GetCurrentPosition() => _currentPositionX;
    private void OnTriggerEnter(Collider other)
    {
        var filter = other.GetComponentInChildren<MeshFilter>();
        if (filter)
        {
            _targetObject = filter.gameObject;

        }
    }
}

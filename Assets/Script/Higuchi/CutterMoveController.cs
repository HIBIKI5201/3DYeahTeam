using UnityEngine;
using System;
using ChargeShot.Runtime.Ingame;
using SymphonyFrameWork.System;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Linq;
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
    private float _currentPosition;
    private float _defaultXPosition = 0.0f;
    [SerializeField, Header("ベストタイミング")] float[] _bestTimings = new float[2];
    private Vector3 _cuttingObjectPosition;
    List<float> _distanceList = new List<float>();
    List<GameObject> _cuttingObject = new List<GameObject>();
    private GameObject _targetObject;
    private GameObject _centerObject;
    private bool _movingRight = true;
    private int _cutCount = 0;
    private float diffResult = 0;
    private float _result;
    public event Action<GameObject> OnCuttingFinish;

    private GameObject _leftSide;
    private GameObject _rightSide;
    private GameObject center;
    int initial = 0;
    private void Start()
    {
        ServiceLocator.SetInstance(this, ServiceLocator.LocateType.Singleton);
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var pieces = MeshCutService.Cut(_targetObject, this.transform.position, this.transform.up,
           _capMaterial);
            if (pieces == null) return;

            //戻り値オブジェクトをリストや変数にいれて新規オブジェクトにBoxCollider付与
            _leftSide = pieces[0];
            _rightSide = pieces[1];
            _cuttingObject.Add(_leftSide);
            _cuttingObject.Add(_rightSide);
            _rightSide.AddComponent<BoxCollider>();
            // 右側のオブジェクトを少し移動
            _rightSide.transform.position += _rightSide.transform.right * -300f;
            _distanceList.Add(Mathf.Abs(_bestTimings[_cutCount] - _currentPosition));
            _cutCount++;

            MakeDiff();

            if (_cutCount > 1)
            {
                StartCoroutine(nameof(SendIngameSystem));
            }
        }
    }

    private void MakeDiff()
    {
        if (_cuttingObjectPosition.z > 0)
        {

            diffResult += _cuttingObjectPosition.z - _bestTimings[0];
            _cuttingObjectPosition.z = 0;
            _maxPosition = 0;
            if (initial != 0) return;
            center = _leftSide;
        }
        else
        {
            diffResult += Mathf.Abs(_cuttingObjectPosition.z - _bestTimings[1]);
            _cuttingObjectPosition.z = 0;
            _minPosition = 0;
            if(initial != 0) return;
            center = _rightSide;
        }
        _result = Mathf.Abs((diffResult / 100) - 100);
    }

    private System.Collections.IEnumerator SendIngameSystem()
    {
        float _distance = 0;
        foreach (var data in _distanceList)
        {
            _distance += data;
        }

        Debug.Log($"{center.name}  : {center.gameObject.transform.position}");
        var inGameSystem = ServiceLocator.GetInstance<IngameSystem>();
        inGameSystem.CucumberData.Phase1Data = _result; //差分の合計値を一旦入れとく
        yield return new WaitForSeconds(3f);
        OnCuttingFinish?.Invoke(center);
        Destroy(gameObject);
    }

    /// <summary>
    /// カッターを左右に移動させる
    /// </summary>
    private void MoveCutter()
    {
        if (_cutCount >= 2) return;
        _cuttingObjectPosition.z = _currentPosition;
        transform.position = _cuttingObjectPosition;
        if (_movingRight)
            _currentPosition += _speed * Time.deltaTime;
        else
            _currentPosition -= _speed * Time.deltaTime;

        if (_currentPosition >= _maxPosition) _movingRight = false;
        if (_currentPosition <= _minPosition) _movingRight = true;
    }
    private void OnDestroy()
    {
        ServiceLocator.DestroyInstance<CutterMoveController>();
    }
    /// <summary>
    /// 現在の位置を取得
    /// </summary>
    public float GetCurrentPosition() => _currentPosition;
    private void OnTriggerEnter(Collider other)
    {
        _targetObject = other.gameObject;
    }
}

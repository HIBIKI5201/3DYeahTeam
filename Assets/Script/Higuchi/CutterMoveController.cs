using UnityEngine;
using System;
using ChargeShot.Runtime.Ingame;
using SymphonyFrameWork.System;
using Unity.VisualScripting;
using System.Collections.Generic;
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
    [SerializeField, Header("ベストタイミング")] float[] _bestTimings = new float[2];
    private Vector3 _cuttingObjectPosition;
    private bool _movingRight = true;
    private int _cutCount = 0;
    private float _distance1;
    private float _distance2;
    List<float> _distanceList = new List<float>();
    private GameObject _targetObject;
    private void Update()
    {
        MoveCutter();
        CuttingObject();
    }
    public void GetTargetObject(GameObject target)
    {
        _targetObject = target;
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

            // 切断されたオブジェクトが存在する場合のみ処理を行う
            if (pieces != null)
            {
                var leftSide = pieces[0];
                var rightSide = pieces[1];
                rightSide.AddComponent<BoxCollider>();
                // 右側のオブジェクトを少し移動
                rightSide.transform.position += rightSide.transform.up * 0.5f;
                _distanceList.Add(Mathf.Abs(_bestTimings[_cutCount] - _currentPosition));
                _cutCount++;
                if (_cutCount > 1)
                {
                    SendIngameSystem();
                }
            }
            else
            {
                Debug.LogError("オブジェクトの切断に失敗しました。");
            }
        }
    }

    private void SendIngameSystem()
    {
        float _distance = 0;
        foreach (var data in _distanceList)
        {
            _distance += data;
        }
        var inGameSystem = ServiceLocator.GetInstance<IngameSystem>();
        inGameSystem.CucumberData.Phase1Data = _distance; //差分の合計値を一旦入れとく
        Debug.Log($"差{_distance}");
    }

    /// <summary>
    /// カッターを左右に移動させる
    /// </summary>
    private void MoveCutter()
    {
        _cuttingObjectPosition.z = _currentPosition;
        transform.position = _cuttingObjectPosition;
        if (_movingRight)
            _currentPosition += _speed * Time.deltaTime;
        else
            _currentPosition -= _speed * Time.deltaTime;

        if (_currentPosition >= _maxPosition) _movingRight = false;
        if (_currentPosition <= _minPosition) _movingRight = true;
    }

    /// <summary>
    /// 現在の位置を取得
    /// </summary>
    public float GetCurrentPosition() => _currentPosition;
    private void OnTriggerEnter(Collider other) => _targetObject = other.gameObject;
}

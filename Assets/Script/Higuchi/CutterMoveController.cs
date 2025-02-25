﻿using UnityEngine;
using System;
using ChargeShot.Runtime.Ingame;
using SymphonyFrameWork.System;
/// <summary>
/// カッターの管理クラス
/// </summary>
public class CutterMoveController : MonoBehaviour
{
    [Header("カッターの動き設定")]
    public float _speed = 2.0f; // 移動速度
    public float _maxPosition = 1.0f; // 右端
    public float _minPosition = -1.0f; // 左端
    [SerializeField, Header("きゅうり情報")] private GameObject _targetObject;
    [SerializeField] Material _capMaterial;
    private float _currentPosition;
    [SerializeField, Header("ベストタイミング1")] float _bestTiming1;
    [SerializeField, Header("ベストタイミング2")] float _bestTiming2;

    private Vector3 _cuttingObjectPosition;
    private bool _movingRight = true;
    // 針の位置が更新されるたびに通知
    public event Action<float> _OnCuttingData;

    private void Update()
    {
        MoveGauge();
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

            // 切断されたオブジェクトが存在する場合のみ処理を行う
            if (pieces != null)
            {
                var leftSide = pieces[0];
                var rightSide = pieces[1];

                // 右側のオブジェクトを少し移動
                rightSide.transform.position += rightSide.transform.up * 0.5f;

                Debug.Log($"差{Mathf.Abs(_currentPosition - _bestTiming1)}");
               // var s = ServiceLocator.GetInstance<>();
            }
            else
            {
                Debug.LogError("オブジェクトの切断に失敗しました。");
            }
        }
    }

    /// <summary>
    /// 針を左右に移動させる
    /// </summary>
    private void MoveGauge()
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
}

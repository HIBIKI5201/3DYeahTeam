using UnityEngine;
using ChargeShot.Runtime.Ingame;
using System.Collections.Generic;
using System;
using SymphonyFrameWork.System;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeed = 50f;  // 回転速度
    public float perfectRotation = 90;
    private Quaternion saveFirstRotation; // 1回目の回転を保存
    private bool rotatingClockwise = true;  // 時計回りか反時計回りかを管理
    private float currentRotation = 0f;  // 現在の回転角度
    private GameObject savedRightSide;
    public GameObject _targetObject; // 切断したいオブジェクト
    public GameObject _cuttingPlane; // 切断平面
    public Material _capMaterial; // 切断面に適用するマテリアル
    public Transform CucumberPosition;
    private List<GameObject> _cutObject = new List<GameObject>();
    public event Action<List<GameObject>> _cutEnd;
    private int _cutCount = 0;
    private void Awake()
    {
        _targetObject = GameObject.Find("center");
        _targetObject.transform.position = CucumberPosition.transform.position;
    }
    private void Start()
    {
        ServiceLocator.SetInstance(this, ServiceLocator.LocateType.Singleton);
    }
    private void OnDestroy()
    {
        ServiceLocator.DestroyInstance(this);
    }
    void Update()
    {
        // 回転方向に応じて回転
        if (rotatingClockwise)
        {
            transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0);
            currentRotation += rotationSpeed * Time.deltaTime;
        }
        else
        {
            transform.Rotate(-rotationSpeed * Time.deltaTime, 0, 0);
            currentRotation -= rotationSpeed * Time.deltaTime;
        }

        // 180度回転したら回転方向を反転
        if (Mathf.Abs(currentRotation) >= 180f)
        {
            rotatingClockwise = !rotatingClockwise;  // 回転方向を反転
            currentRotation = 0f;  // 回転角度をリセット
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_cutCount <= 0)
            {
                saveFirstRotation = transform.rotation; // 1回目の回転を保存  // 1回目の回転
                // 最初のカット
                Cut(_targetObject);
            }
            else if (_cutCount == 1)
            {
                // 右側のオブジェクトをさらにカット
                Cut(_targetObject);
                Cut(savedRightSide);
                Quaternion secondRotation = transform.rotation;  // 2回目の回転
                float angleDifference = Quaternion.Angle(saveFirstRotation, secondRotation); // クォータニオンの角度差
                Debug.Log("回転の変化量 (Quaternion): " + angleDifference + "°");
                _cutEnd?.Invoke(_cutObject);
            }
        }
    }
    public void Cut(GameObject target)
    {

        // オブジェクトを切断
        var pieces = MeshCutService.Cut(target, _cuttingPlane.transform.position, _cuttingPlane.transform.up, _capMaterial);

        // 切断されたオブジェクトが存在する場合のみ処理を行う
        if (pieces != null)
        {
            var leftSide = pieces[0];
            var rightSide = pieces[1];
            _cutObject.Add(leftSide);
            _cutObject.Add(rightSide);
            leftSide.AddComponent<MeshCollider>();
            rightSide.AddComponent<MeshCollider>();
            rightSide.transform.position += rightSide.transform.up * -20f;
            if (savedRightSide == null)
            {
                savedRightSide = rightSide; // 右側のオブジェクトを保存
            }
        }
        else
        {
            Debug.LogError("オブジェクトの切断に失敗しました。");
        }
        _cutCount++;
    }
    public void Distance(float currentYRotation)
    {
        // Mathf.DeltaAngle を使って適切な回転差分を取得
        float distanceRotation = Mathf.Abs(Mathf.DeltaAngle(currentYRotation, perfectRotation));
        Debug.Log(distanceRotation);
        // 目標角度との差が10度未満なら「perfectTiming」
        if (Mathf.Abs(distanceRotation) < 10)
        {
            Debug.Log("perfectTiming");
        }
        else
        {
            Debug.Log("badTiming");
        }
    }
}

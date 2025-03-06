using ChargeShot.Runtime.Ingame;
using SymphonyFrameWork.System;
using SymphonyFrameWork.Utility;
using System;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [Header("回転設定")]
    [SerializeField]
    private float _rotationSpeed = 50f;//回転速度
    [SerializeField]
    private float _perfectRotation = 90;//理想の回転角度

    [Header("切断設定")]
    [SerializeField]
    private GameObject _targetObject;
    [SerializeField]
    private GameObject _cuttingPlane;//切断に使うオブジェクト
    [SerializeField]
    private Material _capMaterial;
    [SerializeField]
    private Transform _cucumberPosition;

    private AudioManager _audioManager;

    private GameObject[] _cutObject = new GameObject[4];
    private GameObject savedRightSide;
    private int _cutCount = 0;

    private float currentRotation = 0f;
    private bool rotatingClockwise = true;
    private Quaternion saveFirstRotation;//1回目の回転保存

    public event Action<GameObject[]> CutEnd;

    public float AngleDifference { get; private set; }


    private async void Start()
    {
        _audioManager = ServiceLocator.GetInstance<AudioManager>();
        var system = ServiceLocator.GetInstance<IngameSystem>();

        if (system)
        {
            await SymphonyTask.WaitUntil(() => system.Cucumber, destroyCancellationToken);

            system.Cucumber.transform.position = _cucumberPosition.position;
            _targetObject = system.Cucumber.CucumberModel;
        }
    }

    void Update()
    {
        RotateObjectBySpeed();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ProcessCutting();
        }
    }

    /// <summary>
    /// オブジェクトを回転させる
    /// </summary>
    private void RotateObjectBySpeed()
    {
        float rotationAmount = _rotationSpeed * Time.deltaTime;
        if (rotatingClockwise)
        {
            transform.Rotate(rotationAmount, 0, 0);
            currentRotation += rotationAmount;
        }
        else
        {
            transform.Rotate(-rotationAmount, 0, 0);
            currentRotation -= rotationAmount;
        }

        //180度回転で方向を反転
        if (Mathf.Abs(currentRotation) >= 180f)
        {
            rotatingClockwise = !rotatingClockwise;
            currentRotation = 0f;
        }
    }

    /// <summary>
    /// スペースキーが押されたときの処理
    /// </summary>
    private void ProcessCutting()
    {
        _audioManager.PlaySoundEffect(3);

        if (_cutCount == 0)
        {
            saveFirstRotation = transform.rotation;
            Cut(_targetObject);
        }
        else if (_cutCount == 1)
        {
            Cut(_targetObject);
            Cut(savedRightSide);

            //角度差を計算
            Quaternion secondRotation = transform.rotation;
            AngleDifference = Quaternion.Angle(saveFirstRotation, secondRotation);
            Debug.Log("回転の変化量 (Quaternion): " + AngleDifference + "°");

            CutEnd?.Invoke(_cutObject);
        }
    }

    /// <summary>
    /// 指定オブジェクトを切断
    /// </summary>
    public void Cut(GameObject target)
    {
        var pieces = MeshCutService.Cut(target, _cuttingPlane.transform.position, _cuttingPlane.transform.up, _capMaterial);

        if (pieces != null)
        {
            var leftSide = pieces[0];
            var rightSide = pieces[1];
            _cutObject[0] = leftSide;
            _cutObject[1] = rightSide;

            MeshUtil.MeshColliderRefresh(leftSide);
            MeshUtil.MeshColliderRefresh(rightSide);

            rightSide.transform.position += rightSide.transform.up * -20f;

            if (savedRightSide == null)
            {
                savedRightSide = rightSide;
            }
        }
        else
        {
            Debug.LogError("オブジェクトの切断に失敗しました。");
        }
        _cutCount++;
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

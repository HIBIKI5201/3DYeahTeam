using SymphonyFrameWork.System;
using UnityEngine;

public class ChargeManager : MonoBehaviour
{
    private float _pushCounter;
    public float PushCounter { get => _pushCounter; }
    private float _timer;
    public float Timer { get => _timer; }

    [SerializeField]
    private float _pushUp = 3;
    [SerializeField]
    private float _waitForSecondsDown = 0.1f;

    [Space(10)]
    [SerializeField]
    private float _timeLimit = 5;
    public float TimeLimit { get => _timeLimit; }

    private bool _chargeFinish = false;
    public bool ChargeFinish { get => _chargeFinish; }

    private void Start()
    {
        _timer = Time.time;
    }

    private void Update()
    {
        if (!_chargeFinish && Time.time > _timer + _timeLimit)
        {
            _chargeFinish = true;

            IngameSystem system = ServiceLocator.GetInstance<IngameSystem>();
            system.CucumberData.Phase3Data = _pushCounter;//一旦そのままデータを代入しているが、後々スコアにするために計算すると思う
            system.NextPhaseEvent();
        }

        if (Input.GetKeyDown(KeyCode.Space) && !_chargeFinish)
        {
            OnClickChargeButton();
        }

        //時間経過でカウントが減っていく処理
        if (_pushCounter > 0 && !_chargeFinish)
        {
            _pushCounter -= _waitForSecondsDown * Time.deltaTime;
        }
    }
    public void OnClickChargeButton()
    {
        if (Time.time < _timer + _timeLimit)
        {
            ChargeAction();
        }
    }

    /// <summary>
    /// カウントアップとカウントダウンを行うスクリプト
    /// </summary>
    private void ChargeAction()
    {
        if (!_chargeFinish)
        {

            _pushCounter += _pushUp;
            Debug.Log($"現在値は　{_pushCounter}");
        }
    }
}

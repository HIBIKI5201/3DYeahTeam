using SymphonyFrameWork.System;
using UnityEngine;

public class ChargeManager : MonoBehaviour
{
    private float _pushCounter;
    private float _timer;

    [SerializeField]
    private float _pushUp = 3;
    [SerializeField]
    private float _waitForSecondsDown = 0.1f;

    [Space(10)]
    [SerializeField]
    private float _timeLimit = 5;

    [Space(10)]
    [Tooltip("UI上での上限、PushCountは全然上限を超える")]
    [SerializeField]
    private float _countLimit = 50;

    private bool _chageFinish = false;

    private void Start()
    {
        _timer = Time.time;
    }

    private void Update()
    {
        if (Time.time < _timer + _timeLimit)
        {
            ChargeAction();
        }
        else if (!_chageFinish)
        {
            _chageFinish = true;

            IngameSystem system = ServiceLocator.GetInstance<IngameSystem>();
            system.CucumberData.Phase3Data = _pushCounter;//一旦そのままデータを代入しているが、後々スコアにするために計算すると思う
            system.NextPhaseEvent();
        }
    }

    /// <summary>
    /// カウントアップとカウントダウンを行うスクリプト
    /// </summary>
    private void ChargeAction()
    {
        if (!_chageFinish)
        {
            //時間経過でカウントが減っていく処理
            if (_pushCounter > 0)
            {
                _pushCounter -= _waitForSecondsDown;
            }

            //キー入力でカウントに加算する処理
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _pushCounter += _pushUp;
                Debug.Log($"現在値は　{_pushCounter}");
            }
        }
    }
}

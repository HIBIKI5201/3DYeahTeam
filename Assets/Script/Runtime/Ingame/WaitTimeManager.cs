using SymphonyFrameWork.System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class WaitTimeManager : MonoBehaviour
{
    //起動しなければならないスクリプトたち
    private ChargeManager _chargeManager;
    [Header("読み込むUIのスクリプト")]
    [SerializeField]
    private Phase3UI _uiManager;

    [Space(10)]

    //初期待機時間設定用変数
    [Header("初期待機時間用変数")]
    [SerializeField]
    private float _waitTime = 3;
    private float _timer;

    //表示用変数
    [Header("表示用変数")]
    [SerializeField]
    private UIDocument _document;
    private FirstWaitTimeWindow _firstWaitTimeWindow;

    private async void Awake()
    {
        var root = _document.rootVisualElement;
        _firstWaitTimeWindow = root.Q<FirstWaitTimeWindow>();

        var audioMana = ServiceLocator.GetInstance<AudioManager>();
        audioMana.PlaySoundEffect(1);
        await _firstWaitTimeWindow.InitializeTask;

        _chargeManager = GetComponent<ChargeManager>();

        _timer = Time.time;
        StartCoroutine(WaitTimer());
    }

    private IEnumerator WaitTimer()
    {
        while (Mathf.Ceil(_waitTime + _timer - Time.time) >= 0)
        {
            _firstWaitTimeWindow.WaitCount.text = Mathf.Ceil(_waitTime + _timer - Time.time).ToString();
            yield return null;
        }
        
        Phase3Init();
        yield break;
    }

    private void Phase3Init()
    {
        _firstWaitTimeWindow.WaitWindow.style.visibility = Visibility.Hidden;
        _uiManager.Phase3Init();
        _chargeManager.Phase3Init();
    }
}

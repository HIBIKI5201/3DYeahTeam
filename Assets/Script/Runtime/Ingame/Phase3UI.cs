using SymphonyFrameWork.Debugger;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Phase3UI : MonoBehaviour
{
    private UIDocument _document;

    private IngameButtonWindow _buttonWindow;
    private Phase3Window _phase3Window;

    [SerializeField]
    private ChargeManager _chargeManager;
    [Tooltip("UI上での上限、PushCountは全然上限を超える")]
    [SerializeField]
    private float _countLimit = 0;

    [Tooltip("丸いボタンの画像")]
    [SerializeField]
    private Texture2D _buttontexture;
    private float _percent;

    [SerializeField]
    private float _countDown = 1;

    private float _timer = 0;

    private async void Awake()
    {
        _document = GetComponent<UIDocument>();
        _document.CheckComponentNull();

        var root = _document.rootVisualElement;
        _buttonWindow = root.Q<IngameButtonWindow>();
        _phase3Window = root.Q<Phase3Window>();

        await _buttonWindow.InitializeTask;
        await _phase3Window.InitializeTask;
    }

    //初期化処理
    public void Phase3Init()
    {
        if (_document)
        {
            //ボタンの処理、イメージ画像を登録
            _buttonWindow.ChargeButton.clicked += _chargeManager.OnChangePushCounter;
            _buttonWindow.ChargeButton.style.backgroundImage = new StyleBackground(_buttontexture);
            _timer = Time.time;
            //アップデート用コルーチン起動
            StartCoroutine(Phase3Update());
        }
    }

    /// <summary>
    /// Phase3UIのUpdate関数として使用
    /// </summary>
    private IEnumerator Phase3Update()
    {
        while (!_chargeManager.ChargeFinish)
        {
            //ChargeCountの中身をゲージに表示
            _percent = _chargeManager.PushCounter / _countLimit * 100;
            _phase3Window.Gaugevalue.style.height = Length.Percent(_percent);

            if (_countDown >= 0)
            {
                //カウントダウン用のテキストを表示
                _countDown = (_chargeManager.TimeLimit + _timer) - Time.time;
                _phase3Window.TimerText.text = _countDown.ToString("0.00");
            }
            yield return null;
        }

        //ボタン処理の無効化、0以下にならないようにしている
        _buttonWindow.ChargeButton.clicked -= _chargeManager.OnChangePushCounter;
        _phase3Window.TimerText.text = "0.00";

        yield break;
    }
}

using SymphonyFrameWork.Debugger;
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

    private float _countDown = 1;

    private async void Awake()
    {
        _document = GetComponent<UIDocument>();
        _document.CheckComponentNull();

        if (_document)
        {
            var root = _document.rootVisualElement;
            _buttonWindow = root.Q<IngameButtonWindow>();
            _phase3Window = root.Q<Phase3Window>();

            await _buttonWindow.InitializeTask;
            await _phase3Window.InitializeTask;

            _buttonWindow.ChargeButton.clicked += _chargeManager.OnClickChargeButton;
            _buttonWindow.ChargeButton.style.backgroundImage = new StyleBackground(_buttontexture);
        }
    }

    private void Update()
    {
        _percent = _chargeManager.PushCounter / _countLimit * 100;
        _phase3Window.Gaugevalue.style.height = Length.Percent(_percent);

        if (_countDown >= 0)
        {
            _countDown = _chargeManager.TimeLimit - Time.time - _chargeManager.Timer;
            _phase3Window.TimerText.text = _countDown.ToString("0.00");
        }
        //以下ごり押しコード、直すべきところだが速さを重視して後回しにします
        else if (_chargeManager.ChargeFinish)
        {
            _buttonWindow.ChargeButton.clicked -= _chargeManager.OnClickChargeButton;
            _phase3Window.TimerText.text = "0.00";
        }
    }
}

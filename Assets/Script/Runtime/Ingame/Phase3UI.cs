using SymphonyFrameWork.Debugger;
using UnityEngine;
using UnityEngine.UIElements;

public class Phase3UI : MonoBehaviour
{
    private UIDocument _document;

    private IngameButtonWindow _buttonWindow;

    [SerializeField]
    private ChargeManager _chargeManager;
    [SerializeField]
    private Texture2D _buttontexture;

    private async void Awake()
    {
        _document = GetComponent<UIDocument>();
        _document.CheckComponentNull();

        if (_document)
        {
            var root = _document.rootVisualElement;
            _buttonWindow = root.Q<IngameButtonWindow>();

            await _buttonWindow.InitializeTask;

            _buttonWindow.ChargeButton.clicked += _chargeManager.OnClickChargeButton;
            _buttonWindow.ChargeButton.style.backgroundImage = new StyleBackground(_buttontexture);
        }
    }
}

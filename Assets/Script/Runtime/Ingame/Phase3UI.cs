using SymphonyFrameWork.Debugger;
using UnityEngine;
using UnityEngine.UIElements;

public class Phase3UI : MonoBehaviour
{
    private UIDocument _document;

    private Phase3Window _phase3Window;

    [SerializeField]
    private ChargeManager _chargeManager;

    private async void Awake()
    {
        _document = GetComponent<UIDocument>();
        _document.CheckComponentNull();

        if (_document)
        {
            var root = _document.rootVisualElement;
            _phase3Window = root.Q<Phase3Window>();

            await _phase3Window.InitializeTask;

            _phase3Window.ChargeButton.clicked += _chargeManager.OnClickChargeButton;
        }
    }
}

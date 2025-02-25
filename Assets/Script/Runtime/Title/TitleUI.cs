using SymphonyFrameWork.Debugger;
using UnityEngine;
using UnityEngine.UIElements;

public class TitleUI : MonoBehaviour
{
    private UIDocument _document;

    private TitleMainWindow _mainWindow;
    private async void Awake()
    {
        _document = GetComponent<UIDocument>();
        _document.CheckComponentNull();

        if (_document)
        {
            var root = _document.rootVisualElement;
            _mainWindow = root.Q<TitleMainWindow>();

            await _mainWindow.InitializeTask;

            _mainWindow.StartButton.clicked += () => Debug.Log("Start");
            _mainWindow.RankingButton.clicked += () => Debug.Log("Ranking");
            _mainWindow.CreditButton.clicked += () => Debug.Log("Credit");
        }
    }
}

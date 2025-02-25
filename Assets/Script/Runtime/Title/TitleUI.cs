using SymphonyFrameWork.Debugger;
using SymphonyFrameWork.System;
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

            _mainWindow.StartButton.clicked += OnStart;
            _mainWindow.RankingButton.clicked += OnRanking;
            _mainWindow.CreditButton.clicked += OnCredit;
        }
    }

    private void OnStart()
    {
        var system = ServiceLocator.GetInstance<MainSystem>();
        system.SceneChange(SceneListEnum.Ingame);
    }

    private void OnRanking()
    {
        Debug.Log("Ranking");
    }

    private void OnCredit()
    {
        Debug.Log("Credit");
    }

}

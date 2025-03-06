using SymphonyFrameWork.Debugger;
using SymphonyFrameWork.System;
using UnityEngine;
using UnityEngine.UIElements;

public class Phase1UI : MonoBehaviour
{
    private UIDocument _document;
    private IngameButtonWindow _buttonWindow;

    [Tooltip("丸いボタンの画像")]
    [SerializeField]
    private Texture2D _buttontexture;

    private Phase1CutTest _phase1;

    private async void Awake()
    {
        _document = GetComponent<UIDocument>();
        _document.CheckComponentNull();


        var root = _document.rootVisualElement;
        _buttonWindow = root.Q<IngameButtonWindow>();

        await _buttonWindow.InitializeTask;

    }
    private void Start()
    {
        _phase1 = ServiceLocator.GetInstance<Phase1CutTest>();
        //ボタンの処理、イメージ画像を登録
        _buttonWindow.ChargeButton.clicked += _phase1.Phase1Cut;
        _buttonWindow.ChargeButton.style.backgroundImage = new StyleBackground(_buttontexture);
        
    }
}

using SymphonyFrameWork.Debugger;
using SymphonyFrameWork.System;
using UnityEngine;
using UnityEngine.UIElements;

public class Phase2UI : MonoBehaviour
{
    private UIDocument _document;
    private IngameButtonWindow _buttonWindow;

    [Tooltip("丸いボタンの画像")]
    [SerializeField]
    private Texture2D _buttontexture;

    private RotateCucumber Phase2;

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
        Phase2 = ServiceLocator.GetInstance<RotateCucumber>();
        //ボタンの処理、イメージ画像を登録
        _buttonWindow.ChargeButton.clicked += Phase2.Phase2Cut;
        _buttonWindow.ChargeButton.style.backgroundImage = new StyleBackground(_buttontexture);
        
    }
}

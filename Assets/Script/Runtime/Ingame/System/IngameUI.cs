using System;
using UnityEngine;
using UnityEngine.UIElements;
using System.Threading.Tasks;

public class IngameUI : MonoBehaviour
{
    private UIDocument _uiDocument;
    
    private VisualElement _blackScreen;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        _blackScreen = _uiDocument.rootVisualElement.Q<VisualElement>("black-screen");
    }

    private void Start()
    {
        _blackScreen.style.opacity = 0;
    }

    public async Task FadeIn(float timer) => await UI_Utility.FadeIn(_blackScreen, timer);
    public async Task FadeOut(float timer) => await UI_Utility.FadeOut(_blackScreen, timer);
}

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
    }

    private void Start()
    {
        _blackScreen = _uiDocument.rootVisualElement.Q<VisualElement>("black-screen");
    }
    
    private async Task FadeIn(float timer)
    {
        float speed = 1 / timer; //透明度の秒間変化スピード
        
        do
        {
            //透明度を変化させる
            _blackScreen.style.opacity = _blackScreen.style.opacity.value
                                         - speed * Time.deltaTime;
            
            await Awaitable.NextFrameAsync();
            timer -= Time.deltaTime;
        } while (timer > 0);

        _blackScreen.style.opacity = 0;
    }
    
    private async Task FadeOut(float timer)
    {
        float speed = 1 / timer; //透明度の秒間変化スピード
        
        do
        {
            //透明度を変化させる
            _blackScreen.style.opacity = _blackScreen.style.opacity.value
                                         + speed * Time.deltaTime;
            
            await Awaitable.NextFrameAsync();
            timer -= Time.deltaTime;
        } while (timer > 0);
        
        _blackScreen.style.opacity = 1;
    }
}

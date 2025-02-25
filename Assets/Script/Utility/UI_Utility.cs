using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UIElements;

public static class UI_Utility
{
    public static async Task FadeIn(VisualElement screen, float timer)
    {
        float speed = 1 / timer; //透明度の秒間変化スピード
        
        do
        {
            //透明度を変化させる
            screen.style.opacity = screen.style.opacity.value
                                         - speed * Time.deltaTime;
            
            await Awaitable.NextFrameAsync();
            timer -= Time.deltaTime;
        } while (timer > 0);

        screen.style.opacity = 0;
    }
    
    public static async Task FadeOut(VisualElement screen,float timer)
    {
        float speed = 1 / timer; //透明度の秒間変化スピード
        
        do
        {
            //透明度を変化させる
            screen.style.opacity = screen.style.opacity.value
                                         + speed * Time.deltaTime;
            
            await Awaitable.NextFrameAsync();
            timer -= Time.deltaTime;
        } while (timer > 0);
        
        screen.style.opacity = 1;
    }
}

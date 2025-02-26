using SymphonyFrameWork.System;
using UnityEngine;

public class TitleSystem : MonoBehaviour
{
    void Start()
    {
        var audio = ServiceLocator.GetInstance<AudioManager>();
        audio.BGMChanged(0, 1);
    }

}

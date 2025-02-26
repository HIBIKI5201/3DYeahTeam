using SymphonyFrameWork.System;
using UnityEngine;

public class TitleSystem : MonoBehaviour
{
    private void Awake()
    {
        _ = SceneLoader.LoadScene(SceneListEnum.SpaceShip.ToString());
    }

    void Start()
    {
        var audio = ServiceLocator.GetInstance<AudioManager>();
        audio.BGMChanged(0, 1);
    }

}

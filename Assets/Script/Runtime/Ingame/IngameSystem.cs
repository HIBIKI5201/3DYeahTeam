using System;
using UnityEngine;

public class IngameSystem : MonoBehaviour
{
    private PhaseKind _nowPhase;
    
    private GameObject _cucumber;
    
    private void Awake()
    {
        _nowPhase = PhaseKind.Phase1;
    }

    public void SetCucumberInstance(GameObject instance)
    {
        if (!instance)
        {
            Debug.LogWarning("対象のインスタンスがありません");
        }
        
        //前のインスタンスを破壊
        if (instance != _cucumber)
        {
            Destroy(_cucumber);
        }
        
        _cucumber = instance;
    }




    /// <summary>
    /// 次のフェーズに遷移する
    /// </summary>
    /// <param name="phase"></param>
    public void NextPhaseEvent()
    {
        
    }

    public enum PhaseKind
    {
        None = 0,
        Phase1 = 1,
        Phase2 = 2,
        Phase3 = 3,
        Result = 4,
    }
}

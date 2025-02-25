using System;
using UnityEngine;

public class IngameSystem : MonoBehaviour
{
    private PhaseKind _nowPhase = 0;
    
    private GameObject _cucumber;
    
    private CucumberData _cucumberData;
    public CucumberData CucumberData { get => _cucumberData; }
    
    private void Awake()
    {
        _nowPhase = PhaseKind.Phase1;
    }

    /// <summary>
    /// キュウリをインゲームシーンに保存する
    /// </summary>
    /// <param name="instance"></param>
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

    public void SetCucumberData(CucumberData cucumberData)
    {
        _cucumberData = cucumberData;
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

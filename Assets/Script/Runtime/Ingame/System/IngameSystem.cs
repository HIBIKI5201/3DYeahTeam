using System;
using System.Threading.Tasks;
using SymphonyFrameWork.System;
using UnityEngine;

public class IngameSystem : MonoBehaviour
{
    private IngameUI _ingameUi;
    
    private PhaseKind _nowPhase = 0;
    private Task _loadTask = Task.CompletedTask;
    
    private GameObject _cucumber;
    
    private CucumberData _cucumberData;
    public CucumberData CucumberData { get => _cucumberData; }
    
    private void Awake()
    {
        _nowPhase = PhaseKind.Phase1;

        _ingameUi = GetComponent<IngameUI>();
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
    public async void NextPhaseEvent()
    {
        if (!_loadTask.IsCompleted)
        {
            Debug.LogWarning("既にロードが開始されています");
        }

        _loadTask = Task.Run(async () =>
        {
            // 今のシーンをアンロード
            SceneListEnum scene = GetSceneEnumByPhaseKind(_nowPhase);
            await SceneLoader.UnloadScene(scene.ToString());

            // 次のシーンをロードする
            _nowPhase++;
            scene = GetSceneEnumByPhaseKind(_nowPhase);
            await SceneLoader.LoadScene(scene.ToString());
        });
        
        await _loadTask;

    }

    private SceneListEnum GetSceneEnumByPhaseKind(PhaseKind kind)
    {
        return kind switch
        {
            PhaseKind.Phase1 => SceneListEnum.IngamePhase_1,
            PhaseKind.Phase2 => SceneListEnum.IngamePhase_2,
            PhaseKind.Phase3 => SceneListEnum.IngamePhase_3,
            PhaseKind.Result => SceneListEnum.IngamePhase_Result,
            _ => SceneListEnum.None,
        };
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

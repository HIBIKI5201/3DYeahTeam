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
    public GameObject Cucumber { get => _cucumber; }
    
    private CucumberData _cucumberData = new();
    public CucumberData CucumberData { get => _cucumberData; }
    
    private void Awake()
    {
        _nowPhase = PhaseKind.Phase1;
        
        _ingameUi = GetComponent<IngameUI>();
    }

    private void Start()
    {
#if UNITY_EDITOR
        //フェーズシーンから始めた時の特殊処理
        var system = ServiceLocator.GetInstance<MainSystem>();
        if (system.NowScene != SceneListEnum.Ingame)
        {
            //現在のシーンが何か取得
            _nowPhase = system.NowScene switch
            {
                SceneListEnum.IngamePhase_1 => PhaseKind.Phase1,
                SceneListEnum.IngamePhase_2 => PhaseKind.Phase2,
                SceneListEnum.IngamePhase_3 => PhaseKind.Phase3,
                SceneListEnum.IngamePhase_Result => PhaseKind.Result,
                _ => PhaseKind.Phase1,
            };
            
            //既に読み込まれているので終わる
            return;
        }
#endif
        
        //インゲームのフェーズをロード
        var scene = GetSceneEnumByPhaseKind(_nowPhase);
        _ = SceneLoader.LoadScene(scene.ToString());
    }

    private void OnDisable()
    {
        SceneListEnum scene = GetSceneEnumByPhaseKind(_nowPhase);
        _ = SceneLoader.UnloadScene(scene.ToString());
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
        _cucumber.transform.parent = transform;
    }
    
    /// <summary>
    /// 次のフェーズに遷移する
    /// </summary>
    [ContextMenu("NextPhase")]
    public async void NextPhaseEvent()
    {
        if (!_loadTask.IsCompleted)
        {
            Debug.LogWarning("既にロードが開始されています");
            return;
        }

        if (_nowPhase >= PhaseKind.Result)
        {
            Debug.LogWarning("リザルトの次のシーンはありません");
            return;
        }

        _loadTask = LoadScene();
        
        await _loadTask;

        async Task LoadScene()
        {
            await _ingameUi.FadeOut(0.5f);

            // 今のシーンをアンロード
            SceneListEnum scene = GetSceneEnumByPhaseKind(_nowPhase);
            await SceneLoader.UnloadScene(scene.ToString());

            // 次のシーンをロードする
            _nowPhase++;
            scene = GetSceneEnumByPhaseKind(_nowPhase);
            await SceneLoader.LoadScene(scene.ToString());

            await _ingameUi.FadeIn(0.5f);
        }
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

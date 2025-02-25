using System;
using UnityEngine;
using System.Threading.Tasks;
using SymphonyFrameWork.System;
using UnityEngine.SceneManagement;

public class MainSystem : MonoBehaviour
{
    private MainUI _mainUI;

    private Task _loadTask = Task.CompletedTask;

    private SceneListEnum _nowScene;
    
    #if UNITY_EDITOR
    [SerializeField] private SceneListEnum _targetScene;
    
    [ContextMenu("SceneChange")]
    private void DebugSceneChange() => SceneChange(_targetScene);
    #endif
    private void Awake()
    {
        _mainUI = GetComponent<MainUI>();
        
        //現在のシーンを保存する
        Scene scene = SceneManager.GetActiveScene();
        if (!Enum.TryParse(scene.name, out _nowScene))
        {
            Debug.LogWarning($"Scene '{scene.name}' is not a valid scene.");
        }
    }

    public async void SceneChange(SceneListEnum scene)
    {
        if (!_loadTask.IsCompleted)
        {
            Debug.LogWarning("既にロードが開始されています");
            return;
        }

        _loadTask = LoadScene();

        await _loadTask;

        async Task LoadScene()
        {
            await _mainUI.FadeOut(1f);

            // 今のシーンをアンロード
            await SceneLoader.UnloadScene(_nowScene.ToString());

            // 次のシーンをロードする
            await SceneLoader.LoadScene(scene.ToString());
            _nowScene = scene;
            SceneLoader.SetActiveScene(scene.ToString());

            await _mainUI.FadeIn(1f);
        }
    }
}

using SymphonyFrameWork.Debugger;
using UnityEngine;
using UnityEngine.UIElements;

public class TitleUI : MonoBehaviour
{
    private UIDocument _document;

    private TitleMainWindow _mainWindow;
    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        _document.CheckComponentNull();

        if (_document)
        {
            var root = _document.rootVisualElement;
            _mainWindow = root.Q<TitleMainWindow>();
        }
    }
}

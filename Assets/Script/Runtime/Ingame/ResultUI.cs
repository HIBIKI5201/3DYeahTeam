using SymphonyFrameWork.System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ResultUI : MonoBehaviour
{
    private UIDocument _document;

    private VisualElement _parent;
    private Label _score;

    public string Score { set { _score.text = value; } }

    private ListView _list;

    private List<string> _breakList = new();

    private Button _backButton;
    private void Awake()
    {
        _document = GetComponent<UIDocument>();
    }

    private void Start()
    {
        var trajection = ServiceLocator.GetInstance<TrajectionMovement>();
        trajection.AllFinished += OpenResult;

        if (!_document)
        {
            Debug.LogWarning("UI Documentがありません");
            return;
        }

        var root = _document.rootVisualElement;
        _parent = root.Q<VisualElement>("parent");
        _score = root.Q<Label>("score");
        _list = root.Q<ListView>("list");

        _breakList.Add("aaa");
        _breakList.Add("eee");

        if (_list != null)
        {
            //リストのバインドを設定
            _list.makeItem = () => new Label();
            _list.bindItem = (element, index) =>
            {
                var value = _breakList[index];
                var label = element as Label;
                label.enableRichText = true;
                label.text = $"<strike>value</strike>";
            };
            _list.itemsSource = _breakList;

            _list.selectionType = SelectionType.None;

            _backButton = root.Q<Button>("back-to-title-button");
            _backButton.clicked += BackToTitle;
        }
    }

    [ContextMenu("OpenResult")]
    public void OpenResult()
    {
        _parent.RemoveFromClassList("window-close");
        _backButton.RemoveFromClassList("button-close");
    }

    private void BackToTitle()
    {
        var system = ServiceLocator.GetInstance<MainSystem>();
        system.SceneChange(SceneListEnum.Title);
    }
}

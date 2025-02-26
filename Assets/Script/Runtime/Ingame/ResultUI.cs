using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ResultUI : MonoBehaviour
{
    private UIDocument _document;

    private VisualElement _parent;
    private Label _score;
    private ListView _list;

    private List<string> _breakList = new();
    private void Awake()
    {
        _document = GetComponent<UIDocument>();
    }

    private void Start()
    {
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
                (element as Label).text = $"<strike>value</strike>";
            };
            _list.itemsSource = _breakList;

            _list.selectionType = SelectionType.None;
            _list.fixedItemHeight = _list.style.fontSize.value.value;
        }
    }

    [ContextMenu("OpenResult")]
    public void OpenResult()
    {
        _parent.RemoveFromClassList("window-close");
    }
}

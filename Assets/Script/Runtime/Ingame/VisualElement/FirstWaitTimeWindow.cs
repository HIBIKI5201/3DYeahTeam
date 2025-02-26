using SymphonyFrameWork.Utility;
using System.Threading.Tasks;
using UnityEngine.UIElements;

[UxmlElement]
public partial class FirstWaitTimeWindow : SymphonyVisualElement
{
    private VisualElement _waitWindow;
    public VisualElement WaitWindow { get => _waitWindow; }

    private Label _waitCount;
    public Label WaitCount { get => _waitCount; }

    public FirstWaitTimeWindow() : base("UITK/Ingame/FirstWaitTimeWindow") { }

    protected override Task Initialize_S(TemplateContainer container)
    {
        _waitWindow = container.Q<VisualElement>("WaitWindow");
        _waitCount = container.Q<Label>("WaitCount");

        return Task.CompletedTask;
    }
}

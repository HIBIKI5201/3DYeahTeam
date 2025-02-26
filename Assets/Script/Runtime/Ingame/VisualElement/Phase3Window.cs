using SymphonyFrameWork.Utility;
using System.Threading.Tasks;
using UnityEngine.UIElements;

[UxmlElement]
public partial class Phase3Window : SymphonyVisualElement
{
    private VisualElement _gaugevalue;
    public VisualElement Gaugevalue { get => _gaugevalue; }

    private Label _timerText;
    public Label TimerText {  get => _timerText; }

    public Phase3Window() : base("UITK/Ingame/Phase3Window") { }

    protected override Task Initialize_S(TemplateContainer container)
    {
        _gaugevalue = container.Q<VisualElement>("GaugeValue");
        _timerText = container.Q<Label>("TimerText");

        return Task.CompletedTask;
    }

}

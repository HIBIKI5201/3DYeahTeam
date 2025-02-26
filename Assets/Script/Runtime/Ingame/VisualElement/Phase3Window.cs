using SymphonyFrameWork.Utility;
using System.Threading.Tasks;
using UnityEngine.UIElements;

[UxmlElement]
public partial class Phase3Window : SymphonyVisualElement
{
    private VisualElement _gaugevalue;
    public VisualElement Gaugevalue { get => _gaugevalue; }

    public Phase3Window() : base("UITK/Ingame/Phase3Window") { }

    protected override Task Initialize_S(TemplateContainer container)
    {
        _gaugevalue = container.Q<VisualElement>("GaugeValue");

        return Task.CompletedTask;
    }

}

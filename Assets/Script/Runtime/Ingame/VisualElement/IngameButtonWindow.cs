using SymphonyFrameWork.Utility;
using System.Threading.Tasks;
using UnityEngine.UIElements;

[UxmlElement]
public partial class IngameButtonWindow : SymphonyVisualElement
{
    private Button _chargeButton;
    public Button ChargeButton {  get => _chargeButton;  }

    public IngameButtonWindow() : base("UITK/Ingame/IngameButonnWindow") { }

    protected override Task Initialize_S(TemplateContainer container)
    {
        _chargeButton = container.Q<Button>("Button");

        return Task.CompletedTask;
    }
}

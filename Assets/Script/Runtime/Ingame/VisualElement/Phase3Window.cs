using SymphonyFrameWork.Utility;
using System.Threading.Tasks;
using UnityEngine.UIElements;

[UxmlElement]
public partial class Phase3Window : SymphonyVisualElement
{
    private Button _chargeButton;
    public Button ChargeButton {  get => _chargeButton;  }
    
    //ゲージのゲージの実装ができていない

    public Phase3Window() : base("UITK/Ingame/Phase3Window") { }

    protected override Task Initialize_S(TemplateContainer container)
    {
        _chargeButton = container.Q<Button>("ChargeButton");

        return Task.CompletedTask;
    }

}

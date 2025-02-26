using SymphonyFrameWork.Utility;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class TitleCreditWindow : SymphonyVisualElement
{
    public TitleCreditWindow() : base("") { }
    protected override Task Initialize_S(TemplateContainer container)
    {
        return Task.CompletedTask;
    }
}

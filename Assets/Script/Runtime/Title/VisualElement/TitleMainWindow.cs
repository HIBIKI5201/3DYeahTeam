using SymphonyFrameWork.Utility;
using System.Threading.Tasks;
using UnityEngine.UIElements;

[UxmlElement]
public partial class TitleMainWindow : SymphonyVisualElement
{
    private Button _startButton;
    public Button StartButton { get => _startButton; }

    private Button _rankingButton;
    public Button RankingButton { get => _rankingButton; }

    private Button _creditButton;
    public Button CreditButton { get => _creditButton; }

    public TitleMainWindow() : base("UITK/Title/TitleMainWindow") { }

    protected override Task Initialize_S(TemplateContainer container)
    {
        _startButton = container.Q<Button>("start-button");
        _rankingButton = container.Q<Button>("ranking-button");
        _creditButton = container.Q<Button>("credit-button");

        return Task.CompletedTask;
    }
}

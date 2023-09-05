using MudBlazor;

namespace LrsWebsite.Pages;

public partial class MainLayout
{
    private readonly MudTheme theme = new MudTheme()
    {
        Typography = new Typography()
        {
            Default = new Default()
            {
                FontFamily = new[] { "Noto Serif" }
            },
            H1 = new H1()
            {
                FontSize = "1.5rem",
                FontWeight = 400,
                LineHeight = 1.334,
                LetterSpacing = "0"
            }
        }
    };
}

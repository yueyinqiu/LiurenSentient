using MudBlazor;

namespace LrsWebsite.Shared;

public partial class MainLayout
{
    private bool isDrawerOpen = true;

    private void ToggleDrawer()
    {
        this.isDrawerOpen = !this.isDrawerOpen;
    }

    private readonly MudTheme theme = new MudTheme()
    {
        Typography = new Typography()
        {
            Default = new Default()
            {
                FontFamily = new[] { "Noto Serif" }
            }
        }
    };
}

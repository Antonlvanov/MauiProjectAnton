namespace MauiProjectAnton;

public partial class StartPage : ContentPage
{
    public StartPage()
    {
        InitializeComponent();
    }

    private async void NavigateToTextPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(TextPage));
    }

    private async void NavigateToFigurePage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(FigurePage));
    }

    private async void NavigateToTimerPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(TimerPage));
    }
}
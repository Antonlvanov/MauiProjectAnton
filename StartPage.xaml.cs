namespace MauiProjectAnton;

public partial class StartPage : ContentPage
{
    public StartPage()
    {
        InitializeComponent();
    }

    private async void NavigateToTextPage(object sender, EventArgs e)
    {
        var button = sender as Button;
        await button.ScaleTo(0.95, 50, Easing.Linear);
        await Shell.Current.GoToAsync(nameof(TextPage));
        await button.ScaleTo(1, 50, Easing.Linear);
    }

    private async void NavigateToFigurePage(object sender, EventArgs e)
    {
        var button = sender as Button;
        await button.ScaleTo(0.95, 50, Easing.Linear);
        await Shell.Current.GoToAsync(nameof(FigurePage));
        await button.ScaleTo(1, 50, Easing.Linear);
    }

    private async void NavigateToTimerPage(object sender, EventArgs e)
    {
        var button = sender as Button;
        await button.ScaleTo(0.95, 50, Easing.Linear);
        await Shell.Current.GoToAsync(nameof(TimerPage));
        await button.ScaleTo(1, 50, Easing.Linear);
    }
}
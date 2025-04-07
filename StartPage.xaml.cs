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

    private async void NavigateToValgusfoorPage(object sender, EventArgs e)
    {
        var button = sender as Button;
        await button.ScaleTo(0.95, 50, Easing.Linear);
        await Shell.Current.GoToAsync(nameof(Valgusfoor));
        await button.ScaleTo(1, 50, Easing.Linear);
    }

    private async void NavigateToLumememmPage(object sender, EventArgs e)
    {
        var button = sender as Button;
        await button.ScaleTo(0.95, 50, Easing.Linear);
        await Shell.Current.GoToAsync(nameof(Lumememm));
        await button.ScaleTo(1, 50, Easing.Linear);
    }

    private async void NavigateToColorPage(object sender, EventArgs e)
    {
        var button = sender as Button;
        await button.ScaleTo(0.95, 50, Easing.Linear);
        await Shell.Current.GoToAsync(nameof(ColorPage));
        await button.ScaleTo(1, 50, Easing.Linear);
    }

    private async void NavigateToDateTimePage(object sender, EventArgs e)
    {
        var button = sender as Button;
        await button.ScaleTo(0.95, 50, Easing.Linear);
        await Shell.Current.GoToAsync(nameof(DateTimePage));
        await button.ScaleTo(1, 50, Easing.Linear);
    }

    private async void NavigateToPopUpPage(object sender, EventArgs e)
    {
        var button = sender as Button;
        await button.ScaleTo(0.95, 50, Easing.Linear);
        await Shell.Current.GoToAsync(nameof(PopUpPage));
        await button.ScaleTo(1, 50, Easing.Linear);
    }
    private async void NavigateToPickerImagePage(object sender, EventArgs e)
    {
        var button = sender as Button;
        await button.ScaleTo(0.95, 50, Easing.Linear);
        await Shell.Current.GoToAsync(nameof(PickerImagePage));
        await button.ScaleTo(1, 50, Easing.Linear);
    }

    private async void NavigateToTripsTrapsPage(object sender, EventArgs e)
    {
        var button = sender as Button;
        await button.ScaleTo(0.95, 50, Easing.Linear);
        await Shell.Current.GoToAsync(nameof(TripsTraps));
        await button.ScaleTo(1, 50, Easing.Linear);
    }

    private async void NavigateToKontaktiAndmedPage(object sender, EventArgs e)
    {
        var button = sender as Button;
        await button.ScaleTo(0.95, 50, Easing.Linear);
        await Shell.Current.GoToAsync(nameof(KontaktiAndmed));
        await button.ScaleTo(1, 50, Easing.Linear);
    }

    private async void NavigateToRiikidPage(object sender, EventArgs e)
    {
        var button = sender as Button;
        await button.ScaleTo(0.95, 50, Easing.Linear);
        await Shell.Current.GoToAsync(nameof(Riikid));
        await button.ScaleTo(1, 50, Easing.Linear);
    }
}
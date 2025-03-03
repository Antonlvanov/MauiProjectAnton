namespace MauiProjectAnton;

public partial class TimerPage : ContentPage
{
    private bool _isTimerRunning;
    private readonly List<string> _buttons = new() { "Tagasi", "Avaleht", "Edasi" };

    public TimerPage()
    {
        InitializeComponent();
        InitializeNavigationButtons();
    }

    private void InitializeNavigationButtons()
    {
        var hsl = new HorizontalStackLayout
        {
            Spacing = 10,
            HorizontalOptions = LayoutOptions.Center
        };

        foreach (var buttonText in _buttons)
        {
            var button = new Button
            {
                Text = buttonText,
                WidthRequest = 120,
                Margin = new Thickness(5)
            };

            button.Clicked += Liikumine;
            hsl.Add(button);
        }
    }

    private async void Liikumine(object sender, EventArgs e)
    {
        try
        {
            if (sender is Button btn)
            {
                var route = btn.Text switch
                {
                    "Tagasi" => nameof(FigurePage),
                    "Avaleht" => "///StartPage",
                    "Edasi" => nameof(TimerPage),
                    _ => nameof(StartPage)
                };

                await Shell.Current.GoToAsync(route);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Viga", $"Navigeerimine ebaхnnestus: {ex.Message}", "OK");
        }
    }

    private async void ShowTime()
    {
        while (_isTimerRunning)
        {
            timer_btn.Text = DateTime.Now.ToString("T");
            await Task.Delay(1000);
        }
    }

    private void timer_btn_Clicked(object sender, EventArgs e)
    {
        _isTimerRunning = !_isTimerRunning;
        if (_isTimerRunning)
            ShowTime();
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        // Добавьте свою логику здесь
    }
}
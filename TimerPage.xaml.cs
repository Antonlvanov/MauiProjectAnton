namespace MauiProjectAnton;

public partial class TimerPage : ContentPage
{
    private bool _isTimerRunning;
    private Animation _gradientAnimation;

    public TimerPage()
    {
        InitializeComponent();
        timerLabel.Text = DateTime.Now.ToString("T");
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
            await DisplayAlert("Viga", $"Navigeerimine ebaõnnestus: {ex.Message}", "OK");
        }
    }

    private async void ShowTime()
    {
        while (_isTimerRunning)
        {
            timerLabel.Text = DateTime.Now.ToString("T");
            await Task.Delay(1000);
        }
    }

    private void timer_btn_Clicked(object sender, EventArgs e)
    {
        _isTimerRunning = !_isTimerRunning;
        if (_isTimerRunning)
        {
            ShowTime();
            StartGradientAnimation();
        }
        else
        {
            StopGradientAnimation();
        }
    }

    private void StartGradientAnimation()
    {
        _gradientAnimation = new Animation(v => gradientFrame.Rotation = v, 0, 360);
        _gradientAnimation.Commit(
            owner: gradientFrame,
            name: "gradientRotation",
            rate: 16,
            length: 2000,
            easing: Easing.Linear,
            repeat: () => _isTimerRunning);
    }

    private void StopGradientAnimation()
    {
        gradientFrame.AbortAnimation("gradientRotation");
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        timer_btn_Clicked(sender, e);
    }
}
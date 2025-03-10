namespace MauiProjectAnton;

public partial class TimerPage : ContentPage
{
    private bool _isRunning;
    private DateTime _startTime;
    private TimeSpan _elapsedTime;

    public TimerPage()
    {
        InitializeComponent();
        UpdateTimeDisplay(TimeSpan.Zero);
    }

    private async void TimerLoop()
    {
        _startTime = DateTime.Now - _elapsedTime;
        while (_isRunning)
        {
            _elapsedTime = DateTime.Now - _startTime;
            UpdateTimeDisplay(_elapsedTime);
            await Task.Delay(100);
        }
    }

    private void UpdateTimeDisplay(TimeSpan time)
    {
        timerLabel.Text = time.ToString(@"hh\:mm\:ss\.ff");
    }

    private void ControlButton_Clicked(object sender, EventArgs e)
    {
        _isRunning = !_isRunning;
        if (_isRunning)
        {
            TimerLoop();
            controlButton.ImageSource="stopicon.png";
        }
        else
        {
            controlButton.ImageSource = "playicon.png";
        }
    }

    private void ResetButton_Clicked(object sender, EventArgs e)
    {
        _isRunning = false;
        _elapsedTime = TimeSpan.Zero;
        UpdateTimeDisplay(TimeSpan.Zero);
    }

}
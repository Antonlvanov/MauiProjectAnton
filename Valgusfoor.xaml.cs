namespace MauiProjectAnton;

public partial class Valgusfoor : ContentPage
{
    private enum LightState { Red, RedYellow, Green, Yellow }
    private LightState _currentState;
    private bool _isRunning;
    private CancellationTokenSource _cts;

    public Valgusfoor()
    {
        InitializeComponent();
        InitializeLights();
    }

    private void InitializeLights()
    {
        RedLight.Opacity = 0.3;
        YellowLight.Opacity = 0.3;
        GreenLight.Opacity = 0.3;
    }

    private async Task RunTrafficLightCycle(CancellationToken token)
    {
        _isRunning = true;

        try
        {
            while (_isRunning)
            {
                await SwitchToRed(token);
                await SwitchToGreen(token);
                await SwitchToYellow(token);
            }
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            _isRunning = false;
        }
    }

    private async Task SwitchToRed(CancellationToken token)
    {
        _currentState = LightState.Red;
        await AnimateLight(RedLight, 5000, token);
    }

    private async Task SwitchToGreen(CancellationToken token)
    {
        _currentState = LightState.Green;
        await AnimateLight(GreenLight, 4000, token);
    }

    private async Task SwitchToYellow(CancellationToken token)
    {
        _currentState = LightState.Yellow;
        await AnimateLight(YellowLight, 2000, token);
    }

    private async Task AnimateLight(View targetLight, int duration, CancellationToken token, View additionalLight = null)
    {
        await Task.WhenAll(
            targetLight.FadeTo(1, 500, Easing.Linear),
            additionalLight?.FadeTo(1, 500, Easing.Linear) ?? Task.CompletedTask
        );

        try
        {
            await Task.Delay(duration - 1000, token);
        }
        catch (TaskCanceledException)
        {
            await Task.WhenAll(
                targetLight.FadeTo(0.3, 500, Easing.Linear),
                additionalLight?.FadeTo(0.3, 500, Easing.Linear) ?? Task.CompletedTask
            );
            throw;
        }

        await Task.WhenAll(
            targetLight.FadeTo(0.3, 500, Easing.Linear),
            additionalLight?.FadeTo(0.3, 500, Easing.Linear) ?? Task.CompletedTask
        );
    }

    private void ControlButton_Clicked(object sender, EventArgs e)
    {
        if (_isRunning)
        {
            _cts?.Cancel();
            ControlButton.Text = "▶";
            ControlButton.BackgroundColor = Colors.Green;
        }
        else
        {
            _cts = new CancellationTokenSource();
            ControlButton.Text = "⏸";
            ControlButton.BackgroundColor = Colors.Red;
            _ = RunTrafficLightCycle(_cts.Token);
        }
    }


    protected override void OnDisappearing()
    {
        _cts?.Cancel();
        base.OnDisappearing();
    }
}

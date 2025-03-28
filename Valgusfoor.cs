﻿namespace MauiProjectAnton;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;
using System;
using System.Threading;
using System.Threading.Tasks;

public partial class Valgusfoor : ContentPage
{
    AbsoluteLayout absoluteLayout = new AbsoluteLayout();

    private bool _isRunning;
    private CancellationTokenSource cancelToken;
    private Image background, valgusfoor;

    private Frame RedLight;
    private Frame YellowLight;
    private Frame GreenLight;
    private ImageButton ControlButton, NightButton;
    private double pageWidth, pageHeight;
    private Random rnd;

    public Valgusfoor()
    {
        BuildUI();
        SetOffColors();
    }

    private void BuildUI()
    {
        rnd = new Random();

        BackgroundColor = Color.FromArgb("#1a1a1a");
        pageHeight = (int)(DeviceDisplay.Current.MainDisplayInfo.Height / DeviceDisplay.Current.MainDisplayInfo.Density);
        pageWidth = (int)(DeviceDisplay.Current.MainDisplayInfo.Width / DeviceDisplay.Current.MainDisplayInfo.Density);

        background = new Image
        {
            Source = "valgusfoorday.png", 
            Aspect = Aspect.AspectFill, 
            Opacity = 1
        };

        valgusfoor = new Image
        {
            Source = "valgusfoorv4.png",
            Aspect = Aspect.AspectFit,
            Opacity = 1
        };

        RedLight = CreateLightFrame("#FF0000");
        YellowLight = CreateLightFrame("#FFFF00");
        GreenLight = CreateLightFrame("#32CD32");

        AbsoluteLayout.SetLayoutBounds(background, new Rect(0.5, 0.5, 1, 1));
        AbsoluteLayout.SetLayoutFlags(background, AbsoluteLayoutFlags.All);

        AbsoluteLayout.SetLayoutBounds(RedLight, new Rect(0.5, 0.06, 195, 200));
        AbsoluteLayout.SetLayoutFlags(RedLight, AbsoluteLayoutFlags.PositionProportional);

        AbsoluteLayout.SetLayoutBounds(YellowLight, new Rect(0.5, 0.5, 195, 200));
        AbsoluteLayout.SetLayoutFlags(YellowLight, AbsoluteLayoutFlags.PositionProportional);

        AbsoluteLayout.SetLayoutBounds(GreenLight, new Rect(0.5, 0.95, 195, 200));
        AbsoluteLayout.SetLayoutFlags(GreenLight, AbsoluteLayoutFlags.PositionProportional);

        AbsoluteLayout.SetLayoutBounds(valgusfoor, new Rect(0.5, 0.5, 400, 800));
        AbsoluteLayout.SetLayoutFlags(valgusfoor, AbsoluteLayoutFlags.PositionProportional);

        absoluteLayout.Children.Add(background);
        absoluteLayout.Children.Add(RedLight);
        absoluteLayout.Children.Add(YellowLight);
        absoluteLayout.Children.Add(GreenLight);
        absoluteLayout.Children.Add(valgusfoor);

        ControlButton = new ImageButton
        {
            WidthRequest = 70,
            HeightRequest = 70,
            CornerRadius = 35,
            Source = "playicon.png",
            BackgroundColor = Colors.Transparent,
            Aspect = Aspect.AspectFill,
            Padding = 0
        };
        ControlButton.Clicked += Button_Clicked;

        AbsoluteLayout.SetLayoutBounds(ControlButton, new Rect(1.05, 0.7, 100, 100));
        AbsoluteLayout.SetLayoutFlags(ControlButton, AbsoluteLayoutFlags.PositionProportional);
        ControlButton.BackgroundColor = Colors.Green;

        NightButton = new ImageButton
        {
            WidthRequest = 70,
            HeightRequest = 70,
            CornerRadius = 35,
            Source = "night.png",
            BackgroundColor = Colors.Transparent,
            Aspect = Aspect.AspectFill,
            Padding = 0
        };
        NightButton.Clicked += Button_Clicked;

        AbsoluteLayout.SetLayoutBounds(NightButton, new Rect(1, 1, 70, 70));
        AbsoluteLayout.SetLayoutFlags(NightButton, AbsoluteLayoutFlags.PositionProportional);
        NightButton.BackgroundColor = Colors.Green;

        absoluteLayout.Children.Add(NightButton);
        absoluteLayout.Children.Add(ControlButton);

        Content = absoluteLayout;
    }

    private Frame CreateLightFrame(string hexColor)
    {
        return new Frame
        {
            WidthRequest = 195,
            HeightRequest = 200,
            CornerRadius = 120,
            BackgroundColor = Color.FromArgb(hexColor),
            Opacity = 0.3,
            Shadow = new Shadow { Brush = new SolidColorBrush(Color.FromArgb(hexColor)), Offset = new Point(0, 0), Radius = 20 }
        };
    }

    private void SetOffColors()
    {
        SetLightColor(RedLight, "#808080");
        SetLightColor(YellowLight, "#808080");
        SetLightColor(GreenLight, "#808080");
    }

    private void SetLightColor(Frame light, string hexColor)
    {
        light.BackgroundColor = Color.FromArgb(hexColor);
        light.Shadow = new Shadow { Brush = new SolidColorBrush(Color.FromArgb(hexColor)), Offset = new Point(0, 0), Radius = 20 };
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        if (sender == NightButton)
        {
            if (_isRunning)
            {
                cancelToken?.Cancel();
                NightButton.BackgroundColor = Colors.Green;
                ControlButton.BackgroundColor = Colors.Green;
            }
            else
            {
                cancelToken?.Cancel();
                _isRunning = true;
                cancelToken = new CancellationTokenSource();
                RunRandomTrafficLightCycle(cancelToken.Token);
                NightButton.BackgroundColor = Colors.Red;
                ControlButton.BackgroundColor = Colors.Green;
            }
        }
        else if (sender == ControlButton)
        {
            if (_isRunning)
            {
                cancelToken?.Cancel();
                ControlButton.Source = "playicon.png";
                ControlButton.BackgroundColor = Colors.Green;
                NightButton.BackgroundColor = Colors.Green;
            }
            else
            {
                cancelToken?.Cancel();
                _isRunning = true;
                cancelToken = new CancellationTokenSource();
                ControlButton.Source = "stopicon.png";
                NightButton.BackgroundColor = Colors.Green;
                ControlButton.BackgroundColor = Colors.Red;
                RunTrafficLightCycle(cancelToken.Token);
            }
        }
    }

    private async void RunTrafficLightCycle(CancellationToken token)
    {
        _isRunning = true;
        try
        {
            while (!token.IsCancellationRequested)
            {
                await AnimateLight(RedLight, "#FF0000", 5000, token);
                await AnimateLight(GreenLight, "#32CD32", 4000, token);
                await AnimateLight(YellowLight, "#FFFF00", 2000, token);
            }
        }
        catch (OperationCanceledException) { }
        finally
        {
            _isRunning = false;
        }
    }

    private async void RunRandomTrafficLightCycle(CancellationToken token)
    {
        _isRunning = true;
        SetOffColors();
        try
        {
            while (!token.IsCancellationRequested)
            {
                await AnimateLight(YellowLight, "#FFFF00", 1000, token);
                await Task.Delay(1000, token);
            }
        }
        catch (OperationCanceledException) { }
        finally
        {
            _isRunning = false;
            SetOffColors();
        }
    }


    private async Task AnimateLight(Frame targetLight, string activeColor, int duration, CancellationToken token)
    {
        SetLightColor(targetLight, activeColor);
        await targetLight.FadeTo(1, 500, Easing.Linear);
        try
        {
            await Task.Delay(duration - 1000, token);
        }
        catch (TaskCanceledException)
        {
            await targetLight.FadeTo(0.3, 500, Easing.Linear);
            SetLightColor(targetLight, "#808080"); 
            throw;
        }
        await targetLight.FadeTo(0.3, 500, Easing.Linear);
        SetLightColor(targetLight, "#808080"); 
    }

    protected override void OnDisappearing()
    {
        cancelToken?.Cancel();
        base.OnDisappearing();
    }
}

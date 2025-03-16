namespace MauiProjectAnton;

using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;
using System.Threading;

public partial class Lumememm : ContentPage
{
    AbsoluteLayout absLayout;
    Image background, head, body, lowerBody, rightArm, leftArm, rightEar, leftEar;
    double midX, midY, pageWidth, pageHeight, headSize, bodySize, lowerbodySize, rightArmSize, leftArmSize, rightEarSize, leftEarSize;
    Button toggleButton;
    Slider speedSlider;

    bool isMelting = false;
    double meltFactor = 0.0;
    CancellationTokenSource meltTokenSource;
    Dictionary<Image, Rect> originalBounds = new Dictionary<Image, Rect>();
    List<Image> meltableParts = new List<Image>();
    List<Image> allParts = new List<Image>();

    public Lumememm()
    {
        Title = "Lumememm";
        Initializer();
        Content = absLayout;
    }

    void Initializer()
    {
        absLayout = new AbsoluteLayout();
        pageHeight = (int)(DeviceDisplay.Current.MainDisplayInfo.Height / DeviceDisplay.Current.MainDisplayInfo.Density);
        pageWidth = (int)(DeviceDisplay.Current.MainDisplayInfo.Width / DeviceDisplay.Current.MainDisplayInfo.Density);

        head = InsertImage("head.png");
        body = InsertImage("body.png");
        lowerBody = InsertImage("lowerbody.png");
        rightArm = InsertImage("rightarm.png");
        leftArm = InsertImage("leftarm.png");
        rightEar = InsertImage("rightear.png");
        leftEar = InsertImage("leftear.png");

        background = new Image
        {
            Source = "winter_scenery.gif",
            Aspect = Aspect.Fill,
            IsAnimationPlaying = true,
            InputTransparent = true
        };
        AbsoluteLayout.SetLayoutBounds(background, new Rect(0, 0, 1, 1));
        AbsoluteLayout.SetLayoutFlags(background, AbsoluteLayoutFlags.All);
        absLayout.Children.Add(background);

        foreach (var element in new[] { head, body, lowerBody, rightArm, leftArm, rightEar, leftEar })
        {
            absLayout.Children.Add(element);
        }

        toggleButton = new Button
        {
            ImageSource = "timericon.png",
            TextColor = Color.FromArgb("#000000"),
            CornerRadius = 35,
            WidthRequest = 70,
            HeightRequest = 70,
        };

        toggleButton.Clicked += OnToggleButtonClicked;

        speedSlider = new Slider
        {
            Minimum = 0,
            Maximum = 100,
            Value = 0,
            HorizontalOptions = LayoutOptions.Center,
            WidthRequest = 200
        };
        speedSlider.ValueChanged += OnSpeedSliderValueChanged;

        AbsoluteLayout.SetLayoutBounds(toggleButton, new Rect(pageWidth - 160, 20, 150, 50));
        AbsoluteLayout.SetLayoutBounds(speedSlider, new Rect(20, 20, 200, 40));

        absLayout.Children.Add(toggleButton);
        absLayout.Children.Add(speedSlider);

        SetLocations();

        meltableParts = new List<Image> { lowerBody, body, head };
        allParts = new List<Image> { lowerBody, body, head, leftEar, rightEar, rightArm, leftArm };
        StoreOriginalBounds();
    }

    void StoreOriginalBounds()
    {
        foreach (var part in allParts)
        {
            originalBounds[part] = AbsoluteLayout.GetLayoutBounds(part);
        }
    }

    Image InsertImage(string failinimi)
    {
        return new Image { Source = failinimi, Aspect = Aspect.Fill, Opacity = 1 };
    }

    void SetLocations()
    {
        midX = pageWidth / 2;
        midY = pageHeight / 2;
        headSize = pageWidth / 2.7;
        bodySize = pageWidth / 2;
        lowerbodySize = pageWidth / 1.5;

        AbsoluteLayout.SetLayoutBounds(head, new Rect(midX - headSize / 2, pageHeight / 5, headSize, headSize));
        Rect headBounds = AbsoluteLayout.GetLayoutBounds(head);
        AbsoluteLayout.SetLayoutBounds(body, new Rect(midX - bodySize / 2, headBounds.Y + headSize - 17, bodySize, bodySize));
        Rect bodyBounds = AbsoluteLayout.GetLayoutBounds(body);
        AbsoluteLayout.SetLayoutBounds(lowerBody, new Rect(midX - lowerbodySize / 2, bodyBounds.Y + bodySize - 44, lowerbodySize, lowerbodySize));

        AbsoluteLayout.SetLayoutBounds(rightArm, new Rect(midX + bodySize / 2 - 6, headBounds.Y + headSize / 2, 90, 142));
        AbsoluteLayout.SetLayoutBounds(leftArm, new Rect(midX - bodySize + 6, headBounds.Y + headSize / 2, 101, 135));

        AbsoluteLayout.SetLayoutBounds(rightEar, new Rect(midX + headSize / 2 - 20, headBounds.Y - 57, 80, 90));
        AbsoluteLayout.SetLayoutBounds(leftEar, new Rect(midX - headSize - 5, headBounds.Y - 75, 95, 110));
    }

    async Task StartMeltAnimation(CancellationToken token)
    {
        try
        {
            const int maxDelay = 70;
            const int minDelay = 10;

            while (!token.IsCancellationRequested && meltFactor < 1.0)
            {
                double speed = speedSlider.Value;
                int delay = (int)(maxDelay - (speed * (maxDelay - minDelay) / 100));
                delay = Math.Clamp(delay, minDelay, maxDelay);

                meltFactor += 0.02;
                UpdateMelt(meltFactor);
                await Task.Delay(delay, token);
            }

            if (meltFactor >= 1.0)
            {
                MainThread.BeginInvokeOnMainThread(() => speedSlider.Value = 0);
            }
        }
        catch (TaskCanceledException) { }
        finally
        {
            isMelting = false;
        }
    }

    void UpdateMelt(double meltFactor)
    {
        // Обновляем размеры и позиции тела, нижней части и головы
        var lowerBodyNewHeight = originalBounds[lowerBody].Height * (1 - meltFactor);
        var lowerBodyNewWidth = originalBounds[lowerBody].Width * (1 + meltFactor * 0.2);
        var bodyNewHeight = originalBounds[body].Height * (1 - meltFactor);
        var bodyNewWidth = originalBounds[body].Width * (1 + meltFactor * 0.2);
        var headNewHeight = originalBounds[head].Height * (1 - meltFactor);
        var headNewWidth = originalBounds[head].Width * (1 + meltFactor * 0.2);

        var lowerBodyNewY = originalBounds[lowerBody].Y + (originalBounds[lowerBody].Height - lowerBodyNewHeight);
        var lowerBodyNewX = midX - lowerBodyNewWidth / 2;

        var bodyNewY = lowerBodyNewY - bodyNewHeight + 44;
        var bodyNewX = midX - bodyNewWidth / 2;

        var headNewY = bodyNewY - headNewHeight + 17;
        var headNewX = midX - headNewWidth / 2;

        AbsoluteLayout.SetLayoutBounds(lowerBody, new Rect(
            lowerBodyNewX,
            lowerBodyNewY,
            lowerBodyNewWidth,
            lowerBodyNewHeight
        ));

        AbsoluteLayout.SetLayoutBounds(body, new Rect(
            bodyNewX,
            bodyNewY,
            bodyNewWidth,
            bodyNewHeight
        ));

        AbsoluteLayout.SetLayoutBounds(head, new Rect(
            headNewX,
            headNewY,
            headNewWidth,
            headNewHeight
        ));

        UpdateArmPositions(bodyNewY, bodyNewHeight);
        UpdateEarPositions(headNewY, headNewHeight);

        lowerBody.Opacity = 1 - meltFactor;
        body.Opacity = 1 - meltFactor;
        head.Opacity = 1 - meltFactor;
    }

    void UpdateArmPositions(double bodyY, double bodyNewHeight)
    {
        var lowerBodyBounds = AbsoluteLayout.GetLayoutBounds(lowerBody);
        double lowerBodyBottom = lowerBodyBounds.Y + lowerBodyBounds.Height;

        double rightArmNewY = bodyY + rightArm.Height - bodyNewHeight;
        rightArmNewY = Math.Min(rightArmNewY, lowerBodyBottom - rightArm.Height);

        AbsoluteLayout.SetLayoutBounds(rightArm, new Rect(
            rightArm.X,
            rightArmNewY,
            rightArm.Width,
            rightArm.Height
        ));

        double leftArmNewY = bodyY + leftArm.Height - bodyNewHeight;
        leftArmNewY = Math.Min(leftArmNewY, lowerBodyBottom - leftArm.Height);

        AbsoluteLayout.SetLayoutBounds(leftArm, new Rect(
            leftArm.X,
            leftArmNewY,
            leftArm.Width,
            leftArm.Height
        ));
    }

    void UpdateEarPositions(double headY, double headNewHeight)
    {
        var lowerBodyBounds = AbsoluteLayout.GetLayoutBounds(lowerBody);
        double lowerBodyBottom = lowerBodyBounds.Y + lowerBodyBounds.Height;
        double rightEarNewY = headY + rightEar.Height - headNewHeight;
        rightEarNewY = Math.Min(rightEarNewY, lowerBodyBottom - rightEar.Height);

        AbsoluteLayout.SetLayoutBounds(rightEar, new Rect(
            rightEar.X,
            rightEarNewY,
            rightEar.Width,
            rightEar.Height
        ));

        double leftEarNewY = headY + leftEar.Height - headNewHeight - 40;
        leftEarNewY = Math.Min(leftEarNewY, lowerBodyBottom - leftEar.Height);

        AbsoluteLayout.SetLayoutBounds(leftEar, new Rect(
            leftEar.X,
            leftEarNewY,
            leftEar.Width,
            leftEar.Height
        ));
    }

    async Task ResetMelt()
    {
        meltFactor = 0.0;
        foreach (var part in allParts)
        {
            AbsoluteLayout.SetLayoutBounds(part, originalBounds[part]);
            part.Opacity = 1;
        }

        foreach (var part in allParts)
        {
            await part.LayoutTo(originalBounds[part], 500, Easing.SpringOut);
        }
    }

    void OnSpeedSliderValueChanged(object sender, ValueChangedEventArgs e)
    {
        if (e.NewValue > 0)
        {
            if (meltFactor >= 1.0) return;

            if (!isMelting)
            {
                isMelting = true;
                meltTokenSource?.Cancel();
                meltTokenSource = new CancellationTokenSource();
                _ = StartMeltAnimation(meltTokenSource.Token);
            }
        }
        else
        {
            if (isMelting)
            {
                meltTokenSource?.Cancel();
                isMelting = false;
            }
        }
    }

    async void OnToggleButtonClicked(object sender, EventArgs e)
    {
        meltTokenSource?.Cancel();
        isMelting = false;
        speedSlider.Value = 0;
        await ResetMelt();
    }
}
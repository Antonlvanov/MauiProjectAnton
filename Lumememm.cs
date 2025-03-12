namespace MauiProjectAnton;
using Microsoft.Maui.Controls.Shapes;

public partial class Lumememm : ContentPage
{
    AbsoluteLayout absLayout;
    Image head, body, lowerBody, rightArm, leftArm, rightEar, leftEar;
    double pageWidth, pageHeight, headSize, bodySize, lowerbodySize, rightArmSize, leftArmSize, rightEarSize, leftEarSize;
    Button toggleButton;
    bool isBackgroundDark = false;

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

        foreach (var element in new[] { head, body, lowerBody, rightArm, leftArm, rightEar, leftEar })
        {
            absLayout.Children.Add(element);
        }

        toggleButton = new Button
        {
            ImageSource = "moon.png",
            TextColor = Color.FromArgb("#000000"),
            CornerRadius = 20,
            WidthRequest = 150,
            HeightRequest = 50
        };

        toggleButton.Clicked += OnToggleButtonClicked;

        AbsoluteLayout.SetLayoutBounds(toggleButton, new Rect(pageWidth - 160, 20, 150, 50));
        absLayout.Children.Add(toggleButton);

        SetLocations();
    }

    Image InsertImage(string failinimi)
    {
        return new Image { Source = failinimi, Aspect = Aspect.Fill, Opacity = 1 };
    }

    void SetLocations()
    {
        double midX = pageWidth / 2;
        double midY = pageHeight / 2;
        headSize = pageWidth / 2.7;
        bodySize = pageWidth / 2;
        lowerbodySize = pageWidth / 1.5;


        AbsoluteLayout.SetLayoutBounds(head, new Rect(midX - headSize/2, pageHeight / 5, headSize, headSize));
        Rect headBounds = AbsoluteLayout.GetLayoutBounds(head);
        AbsoluteLayout.SetLayoutBounds(body, new Rect(midX - bodySize/2, headBounds.Y + headSize-17, bodySize, bodySize));
        Rect bodyBounds = AbsoluteLayout.GetLayoutBounds(body);
        AbsoluteLayout.SetLayoutBounds(lowerBody, new Rect(midX - lowerbodySize/2, bodyBounds.Y + bodySize - 44, lowerbodySize, lowerbodySize));

        AbsoluteLayout.SetLayoutBounds(rightArm, new Rect(midX + bodySize / 2 - 6, headBounds.Y + headSize / 2, 90, 142));
        AbsoluteLayout.SetLayoutBounds(leftArm, new Rect(midX - bodySize + 6, headBounds.Y + headSize / 2, 101, 135));

        AbsoluteLayout.SetLayoutBounds(rightEar, new Rect(midX + headSize / 2 - 20, headBounds.Y - 57, 80, 90));
        AbsoluteLayout.SetLayoutBounds(leftEar, new Rect(midX - headSize - 5, headBounds.Y - 75, 95, 110));
    }

    void OnToggleButtonClicked(object sender, EventArgs e)
    {
        isBackgroundDark = !isBackgroundDark;
        absLayout.BackgroundColor = isBackgroundDark ? Color.FromArgb("#333333") : Color.FromArgb("#FFFFFF");
        toggleButton.TextColor = isBackgroundDark ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#000000");
    }
}

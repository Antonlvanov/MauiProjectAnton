using Microsoft.Maui.Graphics;
using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

public class ColorPage : ContentPage
{
    private Slider redSlider, greenSlider, blueSlider, alphaSlider;
    private Label hexcodeLabel, redLabel, greenLabel, blueLabel, alphaLabel;
    private Label redValueLabel, greenValueLabel, blueValueLabel, alphaValueLabel;
    private Button randomColorButton;
    private AbsoluteLayout layout;
    private Frame colorFrame;
    int pageWidth = (int)(DeviceDisplay.Current.MainDisplayInfo.Width / DeviceDisplay.Current.MainDisplayInfo.Density);

    public ColorPage()
    {
        redSlider = CreateSlider(0, 255, 255);
        greenSlider = CreateSlider(0, 255, 255);
        blueSlider = CreateSlider(0, 255, 255);
        alphaSlider = CreateSlider(0, 255, 255); 

        hexcodeLabel = CreateLabel("HEX Code: #FFFFFFFF", 20, Colors.White, FontAttributes.Bold);
        hexcodeLabel.TextColor = Color.FromRgba("#000000");

        redLabel = CreateLabel("R");
        greenLabel = CreateLabel("G");
        blueLabel = CreateLabel("B");
        alphaLabel = CreateLabel("A");

        redValueLabel = CreateLabel("255");
        greenValueLabel = CreateLabel("255");
        blueValueLabel = CreateLabel("255");
        alphaValueLabel = CreateLabel("255");

        colorFrame = new Frame
        {
            BackgroundColor = Color.FromRgba(255, 255, 255, 255) 
        };

        randomColorButton = new Button
        {
            Text = "Random Color",
            BackgroundColor = Colors.Black,
            TextColor = Colors.White
        };
        randomColorButton.Clicked += async (s, e) => await AnimateRandomColor();

        layout = new AbsoluteLayout();
        ArrangeElements();

        UpdateColor();
    }

    private Slider CreateSlider(double min = 0, double max = 255, double value = 0)
    {
        var slider = new Slider(min, max, value);
        slider.ValueChanged += (s, e) => UpdateColor();
        return slider;
    }

    private Label CreateLabel(string text, int fontSize = 18, Color? textColor = null, FontAttributes attributes = FontAttributes.None)
    {
        return new Label { Text = text, FontSize = fontSize, TextColor = textColor ?? Colors.Black, FontAttributes = attributes };
    }

    private void ArrangeElements()
    {
        layout.Children.Add(hexcodeLabel);
        layout.Children.Add(randomColorButton);
        layout.Children.Add(colorFrame);

        layout.Children.Add(redLabel); layout.Children.Add(redSlider); layout.Children.Add(redValueLabel);
        layout.Children.Add(greenLabel); layout.Children.Add(greenSlider); layout.Children.Add(greenValueLabel);
        layout.Children.Add(blueLabel); layout.Children.Add(blueSlider); layout.Children.Add(blueValueLabel);
        layout.Children.Add(alphaLabel); layout.Children.Add(alphaSlider); layout.Children.Add(alphaValueLabel);

        Content = layout;

        AbsoluteLayout.SetLayoutBounds(hexcodeLabel, new Rect(20, 25, pageWidth - 150, 50));
        AbsoluteLayout.SetLayoutBounds(randomColorButton, new Rect(pageWidth - 120, 15, 85, 45));

        AbsoluteLayout.SetLayoutBounds(colorFrame, new Rect(0, 80, pageWidth, pageWidth));

        int sliderY = pageWidth + 90;
        int step = 40;
        int sliderWidth = pageWidth - 130;

        AbsoluteLayout.SetLayoutBounds(redLabel, new Rect(25, sliderY, 50, 30));
        AbsoluteLayout.SetLayoutBounds(redSlider, new Rect(60, sliderY, sliderWidth, 30));
        AbsoluteLayout.SetLayoutBounds(redValueLabel, new Rect(pageWidth - 50, sliderY, 40, 30));

        sliderY += step;
        AbsoluteLayout.SetLayoutBounds(greenLabel, new Rect(25, sliderY, 50, 30));
        AbsoluteLayout.SetLayoutBounds(greenSlider, new Rect(60, sliderY, sliderWidth, 30));
        AbsoluteLayout.SetLayoutBounds(greenValueLabel, new Rect(pageWidth - 50, sliderY, 40, 30));

        sliderY += step;
        AbsoluteLayout.SetLayoutBounds(blueLabel, new Rect(25, sliderY, 50, 30));
        AbsoluteLayout.SetLayoutBounds(blueSlider, new Rect(60, sliderY, sliderWidth, 30));
        AbsoluteLayout.SetLayoutBounds(blueValueLabel, new Rect(pageWidth - 50, sliderY, 40, 30));

        sliderY += step;
        AbsoluteLayout.SetLayoutBounds(alphaLabel, new Rect(25, sliderY, 50, 30));
        AbsoluteLayout.SetLayoutBounds(alphaSlider, new Rect(60, sliderY, sliderWidth, 30));
        AbsoluteLayout.SetLayoutBounds(alphaValueLabel, new Rect(pageWidth - 50, sliderY, 40, 30));
    }

    private void UpdateColor()
    {
        int r = (int)redSlider.Value;
        int g = (int)greenSlider.Value;
        int b = (int)blueSlider.Value;
        int a = (int)alphaSlider.Value;

        colorFrame.BackgroundColor = Color.FromRgba(r, g, b, a);
        hexcodeLabel.Text = $"HEX Code: #{r:X2}{g:X2}{b:X2}{a:X2}";

        redValueLabel.Text = r.ToString();
        greenValueLabel.Text = g.ToString();
        blueValueLabel.Text = b.ToString();
        alphaValueLabel.Text = a.ToString();
    }

    private async Task AnimateRandomColor()
    {
        Random rnd = new Random();
        int R = rnd.Next(256);
        int G = rnd.Next(256);
        int B = rnd.Next(256);
        int A = rnd.Next(256);

        while ((int)redSlider.Value != R || (int)greenSlider.Value != G || (int)blueSlider.Value != B || (int)alphaSlider.Value != A)
        {
            redSlider.Value += Math.Sign(R - redSlider.Value);
            greenSlider.Value += Math.Sign(G - greenSlider.Value);
            blueSlider.Value += Math.Sign(B - blueSlider.Value);
            alphaSlider.Value += Math.Sign(A - alphaSlider.Value);

            UpdateColor();
            await Task.Delay(5);
        }
    }
}

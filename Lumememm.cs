namespace MauiProjectAnton;
using Microsoft.Maui.Controls.Shapes;

public partial class Lumememm : ContentPage
{
    AbsoluteLayout absoluutnePaigutus;
    Image head, body, lowerBody, rightArm, leftArm, rightEar, leftEar;
    double pageWidth, pageHeight, headSize, bodySize, lowerbodySize, rightArmSize, leftArmSize, rightEarSize, leftEarSize;
    

    public Lumememm()
    {
        InitsialiseeriKujundus();
        Content = absoluutnePaigutus;
    }

    void InitsialiseeriKujundus()
    {
        absoluutnePaigutus = new AbsoluteLayout();
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
            absoluutnePaigutus.Children.Add(element);
        }

        SetBorders();
    }

    Image InsertImage(string failinimi)
    {
        return new Image { Source = failinimi, Aspect = Aspect.Fill, Opacity = 1 };
    }

    void SetBorders()
    {
        double midX = pageWidth / 2;
        double midY = pageHeight / 2;
        headSize = pageWidth / 2.5;
        bodySize = pageWidth / 2;
        lowerbodySize = pageWidth / 1.5;


        AbsoluteLayout.SetLayoutBounds(head, new Rect(midX - headSize/2, pageHeight / 6, headSize, headSize));
        Rect headBounds = AbsoluteLayout.GetLayoutBounds(head);
        AbsoluteLayout.SetLayoutBounds(body, new Rect(midX - bodySize/2, headBounds.Y + headSize-17, bodySize, bodySize));
        Rect bodyBounds = AbsoluteLayout.GetLayoutBounds(body);
        AbsoluteLayout.SetLayoutBounds(lowerBody, new Rect(midX - lowerbodySize/2, bodyBounds.Y + bodySize - 30, lowerbodySize, lowerbodySize));

        AbsoluteLayout.SetLayoutBounds(rightArm, new Rect(midX + 50, 160, 80, 80));
        AbsoluteLayout.SetLayoutBounds(leftArm, new Rect(midX - 130, 160, 80, 80));

        AbsoluteLayout.SetLayoutBounds(rightEar, new Rect(midX + 40, 40, 40, 40));
        AbsoluteLayout.SetLayoutBounds(leftEar, new Rect(midX - 80, 40, 40, 40));
    }
}

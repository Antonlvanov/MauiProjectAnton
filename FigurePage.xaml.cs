namespace MauiProjectAnton;

public partial class FigurePage : ContentPage
{
    BoxView bw;
    Label lbl;
    Random rnd = new Random();
    HorizontalStackLayout hsl;
    List<string> buttons = new List<string> { "Tagasi", "Avaleht", "Edasi" };
    int click = 0;

    public FigurePage()
    {
        InitializeComponent();

        int r = rnd.Next(0, 255);
        int g = rnd.Next(0, 255);
        int b = rnd.Next(0, 255);

        lbl = new Label
        {
            Text = "Klikid: 0",
            FontSize = 24,
            TextColor = Color.FromRgb(50, 50, 50),
            HorizontalOptions = LayoutOptions.Center
        };

        bw = new BoxView
        {
            Color = Color.FromRgb(r, g, b),
            CornerRadius = 20,
            WidthRequest = 200,
            HeightRequest = 200,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            BackgroundColor = Color.FromRgba(0, 0, 0, 0)
        };

        TapGestureRecognizer tap = new TapGestureRecognizer();
        tap.Tapped += Klik_boksi_peal;
        bw.GestureRecognizers.Add(tap);
        hsl = new HorizontalStackLayout { };
        for (int i = 0; i < 3; i++)
        {
            Button nupp = new Button
            {
                Text = buttons[i],
                WidthRequest = DeviceDisplay.Current.MainDisplayInfo.Width / 8.3,
            };

            hsl.Add(nupp);
            nupp.Clicked += Liikumine;
        }

        VerticalStackLayout vsl = new VerticalStackLayout
        {
            Children = { lbl, bw, hsl },
            VerticalOptions = LayoutOptions.End
        };

        Content = vsl;
    }

    private void Klik_boksi_peal(object? sender, TappedEventArgs e)
    {
        click++;
        lbl.Text = $"Klikid: {click}";

        bw.Color = Color.FromRgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
        // br.RotateXTo(10,10);
        bw.WidthRequest += 20;
        bw.HeightRequest += 20;

        if (bw.WidthRequest > (int)DeviceDisplay.MainDisplayInfo.Width / 3)
        {
            bw.WidthRequest = 200;
            bw.HeightRequest = 200;
            click = 0;
            lbl.Text = "Klikid: 0";
        }
    }

    private async void Liikumine(object? sender, EventArgs e)
    {
        var btn = (Button)sender;
        var route = btn.Text switch
        {
            "Tagasi" => nameof(TextPage),
            "Avaleht" => "..", // Возврат к корневой странице
            "Edasi" => nameof(TimerPage),
            _ => nameof(StartPage)
        };

        await Shell.Current.GoToAsync(route);
    }

}
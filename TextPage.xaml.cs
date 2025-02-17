namespace MauiProjectAnton;

public partial class TextPage : ContentPage
{
    Label lbl;
    Editor editor;
    HorizontalStackLayout hsl;
    List<string> buttons = new List<string> { "Tagasi", "Avaleht", "Edasi" };
    Random rnd = new Random();
    public TextPage()
    {
        InitializeComponent();
        lbl = new Label
        {
            Text = "Pealkiri",
            TextColor = Color.FromRgb(100, 10, 10),
            FontFamily = "Luckymoon 400",
            FontAttributes = FontAttributes.Bold,
            TextDecorations = TextDecorations.Underline,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
            FontSize = 28,
        };
        editor = new Editor
        {
            Placeholder = "Vihje: Sisesta siia tekst",
            PlaceholderColor = Color.FromRgb(250, 200, 100),
            BackgroundColor = Color.FromRgb(200, 200, 100),
            TextColor = Color.FromRgb(100, 50, 200),
            FontSize = 28,
            FontAttributes = FontAttributes.Italic,
        };

        editor.TextChanged += Teksti_sisestamine;
        hsl = new HorizontalStackLayout { };

        for (int i = 0; i < 3; i++)
        {
            Button b = new Button
            {
                Text = buttons[i],
                WidthRequest = DeviceDisplay.Current.MainDisplayInfo.Width / 8.3,
            };
            hsl.Add(b);
            b.Clicked += Liikumine;
        }

        VerticalStackLayout vst = new VerticalStackLayout
        {
            Children = { lbl, editor, hsl },
            VerticalOptions = LayoutOptions.End
        };
        Content = vst;
    }
    private void Teksti_sisestamine(object? sender, TextChangedEventArgs e)
    {
        lbl.Text = editor.Text;
    }

    private async void Liikumine(object? sender, EventArgs e)
    {
        try
        {
            var btn = (Button)sender;
            var route = btn.Text switch
            {
                "Tagasi" => nameof(TextPage),
                "Avaleht" => "///StartPage",
                "Edasi" => nameof(FigurePage),
                _ => nameof(StartPage)
            };

            await Shell.Current.GoToAsync(route);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Viga", $"Navigeerimine ebaõnnestus: {ex.Message}", "OK");
        }
    }

    int i = 0;
    private void Ed_TextChanged(object sender, TextChangedEventArgs e)
    {
        editor.TextChanged -= Ed_TextChanged;
        char key = e.NewTextValue?.LastOrDefault() ?? ' ';
        if (key == 'A')
        {
            i++;
            editor.Text = i.ToString() + ": " + i;
        }
        editor.TextChanged += Ed_TextChanged;
    }

}
namespace MauiProjectAnton;

public partial class StartPage : ContentPage
{
    public List<ContentPage> lehed = new List<ContentPage>() { new TextPage(0), new FigurePage(1) };
    public List<string> tekstid = new List<string> { "Tee lahti TekstPage", "Tee lahti FigurePage" };

    ScrollView sv;
    VerticalStackLayout vsl;
    public StartPage()
    {
        // InitializeComponent();
        Title = "Avaleht";
        vsl = new VerticalStackLayout { BackgroundColor = Color.FromRgb(180, 100, 20) };
        for (int i = 0; i < tekstid.Count; i++)
        {
            Button nupp = new Button
            {
                Text = tekstid[i],
                BackgroundColor = Color.FromRgb(20, 100, 200),
                TextColor = Color.FromRgb(10, 20, 15),
                BorderWidth = 10,
                ZIndex = i,
                FontFamily = "Helvetica 400"
            };
            vsl.Add(nupp);
            nupp.Clicked += OpenPage;
        }
        sv = new ScrollView { Content = vsl };
        Content = sv;
    }
    private async void OpenPage(object? sender, EventArgs e)
    {
        Button btn = (Button)sender;
        await Navigation.PushAsync(lehed[btn.ZIndex]);
    }
}
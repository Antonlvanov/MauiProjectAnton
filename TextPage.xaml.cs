namespace MauiProjectAnton
{
    public partial class TextPage : ContentPage
    {
        private List<(string Text, Color Color)> enteredLines = new List<(string, Color)>();

        private readonly Random rnd = new Random();

        public TextPage()
        {
            InitializeComponent();
            Editor.TextChanged += OnEditorTextChanged;
        }

        private void OnEditorTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue) && e.NewTextValue.EndsWith("\n"))
            {
                var inputLine = e.NewTextValue.TrimEnd('\n', '\r');
                if (!string.IsNullOrWhiteSpace(inputLine))
                {
                    enteredLines.Add((inputLine, Color.FromRgb(rnd.Next(256), rnd.Next(256), rnd.Next(256))));
                }
                Editor.Text = string.Empty;
            }

            var formatted = new FormattedString();
            foreach (var line in enteredLines)
            {
                formatted.Spans.Add(new Span { Text = line.Text + "\n", TextColor = line.Color });
            }

            if (!string.IsNullOrEmpty(Editor.Text))
            {
                formatted.Spans.Add(new Span { Text = Editor.Text, TextColor = Colors.Black });
            }

            TitleLabel.FormattedText = formatted;
        }

        private async void Liikumine(object sender, EventArgs e)
        {
            try
            {
                if (sender is Button btn)
                {
                    var route = btn.Text switch
                    {
                        "Tagasi" => nameof(TextPage),
                        "Avaleht" => "///StartPage",
                        "Edasi" => nameof(FigurePage),
                        _ => nameof(StartPage)
                    };

                    await Shell.Current.GoToAsync(route);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Viga", $"Navigeerimine ebaõnnestus: {ex.Message}", "OK");
            }
        }
    }
}

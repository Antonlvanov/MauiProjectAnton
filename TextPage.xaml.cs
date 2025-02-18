namespace MauiProjectAnton
{
    public partial class TextPage : ContentPage
    {
        // Список для хранения строк с назначенными случайными цветами.
        private List<(string Text, Color Color)> enteredLines = new List<(string, Color)>();

        // Экземпляр генератора случайных чисел.
        private readonly Random rnd = new Random();

        public TextPage()
        {
            InitializeComponent();
            Editor.TextChanged += OnEditorTextChanged;
        }

        /// <summary>
        /// Генерация случайного цвета.
        /// </summary>
        private Color GetRandomColor()
        {
            return Color.FromRgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
        }

        /// <summary>
        /// Обработка изменения текста в Editor.
        /// Если обнаружен символ перевода строки, строка сохраняется с назначенным цветом,
        /// поле ввода очищается, а Label обновляется.
        /// </summary>
        private void OnEditorTextChanged(object sender, TextChangedEventArgs e)
        {
            // Если введенный текст заканчивается символом новой строки,
            // считаем, что пользователь завершил ввод строки.
            if (!string.IsNullOrEmpty(e.NewTextValue) && e.NewTextValue.EndsWith("\n"))
            {
                // Убираем символ новой строки (и возможные пробелы в конце)
                var inputLine = e.NewTextValue.TrimEnd('\n', '\r');
                if (!string.IsNullOrWhiteSpace(inputLine))
                {
                    // Сохраняем строку с случайным цветом.
                    enteredLines.Add((inputLine, GetRandomColor()));
                }
                // Очищаем поле ввода.
                Editor.Text = string.Empty;
            }

            // Обновляем отображение в TitleLabel.
            // Формируем FormattedString из сохранённых строк и текущего ввода.
            var formatted = new FormattedString();

            foreach (var line in enteredLines)
            {
                formatted.Spans.Add(new Span { Text = line.Text + "\n", TextColor = line.Color });
            }

            // Отображаем текущий ввод (если он есть) стандартным цветом.
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

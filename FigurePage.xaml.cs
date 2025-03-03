namespace MauiProjectAnton;

public partial class FigurePage : ContentPage
{
    private int click = 0;
    private Random rnd = new Random();

    public FigurePage()
    {
        InitializeComponent();
    }

    private void Klik_boksi_peal(object sender, TappedEventArgs e)
    {
        click++;
        ClickLabel.Text = $"Klikid: {click}";

        // Изменение цвета
        ColorBox.Color = Color.FromRgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));

        // Увеличение размера
        ColorBox.WidthRequest += 20;
        ColorBox.HeightRequest += 20;

        // Проверка на максимальный размер
        if (ColorBox.WidthRequest > (int)DeviceDisplay.MainDisplayInfo.Width / 3)
        {
            // Сброс размера
            ColorBox.WidthRequest = 200;
            ColorBox.HeightRequest = 200;
            click = 0;
            ClickLabel.Text = "Klikid: 0";

            // Изменение формы на случайную
            ChangeShape();
        }
    }

    private void ChangeShape()
    {
        // Случайный выбор формы
        int shape = rnd.Next(0, 3); // 0 - прямоугольник, 1 - круг, 2 - скругленный прямоугольник

        switch (shape)
        {
            case 0:
                // Прямоугольник
                ColorBox.CornerRadius = 0;
                break;
            case 1:
                // Круг
                ColorBox.CornerRadius = (float)ColorBox.WidthRequest / 2;
                break;
            case 2:
                // Скругленный прямоугольник
                ColorBox.CornerRadius = 20;
                break;
        }
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
                    "Edasi" => nameof(TimerPage),
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
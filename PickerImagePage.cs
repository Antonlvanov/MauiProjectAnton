namespace MauiProjectAnton;

public partial class PickerImagePage : ContentPage
{
    Grid gr4x1, gr3x3;
    Picker picker; //piltide valik
    Image img; //pilt
    Switch s_pilt, s_grid; //piltide kuvamine/peidmine ja grid 3x3 kuvamine/peidmine
    Random rnd = new Random();
    public PickerImagePage()
    {
        gr4x1 = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(3, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(3, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
            },
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
            }
        };
        picker = new Picker
        {
            Title = "Pildid",
            HorizontalOptions = LayoutOptions.Center
        };
        picker.Items.Add("Pilt 1");
        picker.Items.Add("Pilt 2");
        picker.Items.Add("Pilt 3");
        picker.Items.Add("Enda valitud foto");
        picker.SelectedIndexChanged += Piltide_Valik;

        img = new Image
        {
            Source = "dotnet_bot.png",
            HorizontalOptions = LayoutOptions.Center
        };
        s_pilt = new Switch
        {
            IsToggled = true,
            IsEnabled = true,
            HorizontalOptions = LayoutOptions.Center
        };
        s_pilt.Toggled += Kuva_Peida_pilt;
        s_grid = new Switch
        {
            IsToggled = true,
            IsEnabled = true,
            HorizontalOptions = LayoutOptions.Center
        };
        s_grid.Toggled += Kuva_Peida_grid; ;
        gr4x1.Add(picker, 0, 0);
        gr4x1.SetColumnSpan(picker, 2);
        gr4x1.Add(img, 0, 1);
        gr4x1.SetColumnSpan(img, 2);
        gr4x1.Add(s_pilt, 0, 3);
        gr4x1.Add(s_grid, 1, 3);
        Content = gr4x1;
    }

    private void Kuva_Peida_grid(object? sender, ToggledEventArgs e)
    {
        gr3x3 = new Grid();
        for (int i = 0; i < 3; i++)
        {
            gr3x3.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            gr3x3.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        }
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Frame f = new Frame
                {
                    BackgroundColor = Color.FromRgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)),
                };
                gr3x3.Add(f, i, j);
            }
        }
        if (e.Value)
        {
            gr4x1.Add(gr3x3, 0, 2);
            gr4x1.SetColumnSpan(gr3x3, 2);
        }
        else
        {
            gr4x1.RemoveAt(4); //Index in gr4x1
        };
    }
    private void Kuva_Peida_pilt(object? sender, ToggledEventArgs e)
    {
        if (e.Value)
        {
            img.IsVisible = true;
        }
        else
        {
            img.IsVisible = false;
        }
    }

    private async void Piltide_Valik(object? sender, EventArgs e)
    {
        if (picker.SelectedIndex == 3)
        {
            if (picker.SelectedIndex == 3)
            {
                var images = await FilePicker.Default.PickAsync(new PickOptions
                {
                    FileTypes = FilePickerFileType.Images
                });
                var imageSource = images.FullPath.ToString();
                img.Source = imageSource;
            }
        }
        else if (picker.SelectedIndex == 2)
        {
            img.Source = "pilt.png";
        }
        else if (picker.SelectedIndex == 1)
        {
            img.Source = "start.png";
        }
        else if (picker.SelectedIndex == 0)
        {
            img.Source = "dotnet_bot.png";
        }
    }
}
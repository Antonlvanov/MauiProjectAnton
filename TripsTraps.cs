namespace MauiProjectAnton;

public class TripsTraps : ContentPage
{
    Grid grid;
    Picker picker;
    Frame cell_1, cell_2, cell_3, cell_4, cell_5, cell_6, cell_7, cell_8, cell_9;

    public TripsTraps()
    {
        grid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
            },
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
            }
        };

        picker = new Picker
        {
            Title = "Pildid",
            HorizontalOptions = LayoutOptions.Center
        };
    }
}
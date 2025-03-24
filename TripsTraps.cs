using System.Drawing;

namespace MauiProjectAnton;

public class TripsTraps : ContentPage
{
    Grid grid;
    Picker playersPicker;
    Picker gridSizePicker;
    Image statusImage;
    Label statusLabel;
    Button gameControlButton;
    StackLayout mainLayout;
    public enum Player { X, O }
    private Player currentPlayer = Player.X;

    public TripsTraps()
    {
        Title = "Trips traps trull";
        BackgroundColor = Colors.LightGray;
        MakeUI();
        gridSizePicker.SelectedIndexChanged += OnGridSizeChanged;
        MakeCells();
    }

    private void MakeUI()
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
            },
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Center,
            BackgroundColor = Colors.White
        };

        grid.SizeChanged += (sender, e) =>
        {
            if (grid.Width > 0)
                grid.HeightRequest = grid.Width;
        };

        // pickers

        playersPicker = new Picker
        {
            Title = "Mängijad",
            Items = { "1 Mängija", "2 Mängijad" },
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Center,
            BackgroundColor = Colors.DarkGray,
            TextColor = Colors.Green,
            FontSize = 20,
            Margin = new Thickness(10),
            TitleColor = Colors.Gray,
            ItemDisplayBinding = new Binding(".", stringFormat: "{0}")
        };

        gridSizePicker = new Picker
        {
            Title = "Võre suurus",
            Items = { "3x3", "4x4", "5x5" },
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Center,
            BackgroundColor = Colors.DarkGray,
            TextColor = Colors.Green,
            FontSize = 20,
            Margin = new Thickness(10),
            TitleColor = Colors.Gray,
            ItemDisplayBinding = new Binding(".", stringFormat: "{0}")
        };

        playersPicker.SelectedIndex = 0;
        gridSizePicker.SelectedIndex = 0;

        var pickersGrid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
            },
            ColumnSpacing = 10,
            Padding = new Thickness(15, 5),
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Center
        };

        pickersGrid.Children.Add(playersPicker);
        pickersGrid.Children.Add(gridSizePicker);

        Grid.SetColumn(playersPicker, 0);
        Grid.SetColumn(gridSizePicker, 1);

        // status
        statusImage = new Image
        {
            Source = "status_icon.png",
            HeightRequest = 30,
            WidthRequest = 30,
            VerticalOptions = LayoutOptions.Center
        };

        statusLabel = new Label
        {
            Text = "Someshit",
            FontSize = 16,
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalOptions = LayoutOptions.StartAndExpand
        };

        var statusLayout = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            Children = { statusLabel, statusImage },
            Spacing = 10,
            HorizontalOptions = LayoutOptions.Fill,
            Padding = new Thickness(10, 5)
        };

        // control
        gameControlButton = new Button
        {
            Text = "Alusta",
            BackgroundColor = Colors.Gray,
            TextColor = Colors.White,
            CornerRadius = 10,
            HorizontalOptions = LayoutOptions.Fill,
            Margin = new Thickness(20, 10),
            HeightRequest = 50
        };
        gameControlButton.Clicked += OnGameControlClicked;

        // main layout
        mainLayout = new StackLayout
        {
            Children =
            {
                pickersGrid,
                statusLayout,
                grid,
                gameControlButton
            },
            Spacing = 10,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill
        };

        Content = mainLayout;
    }

    private void OnGameControlClicked(object sender, EventArgs e)
    {
        if (gameControlButton.Text == "Algus")
        {
            gameControlButton.Text = "Tühista";
            gameControlButton.BackgroundColor = Colors.Red;
            statusLabel.Text = "Ход игрока X";
            MakeCells();
        }
        else
        {
            gameControlButton.Text = "Algus";
            gameControlButton.BackgroundColor = Colors.Green;
        }
    }

    private void MakeCells()
    {
        grid.Children.Clear();
        grid.RowDefinitions.Clear();
        grid.ColumnDefinitions.Clear();

        int size = 3;
        if (gridSizePicker.SelectedIndex >= 0)
            size = int.Parse(gridSizePicker.SelectedItem.ToString().Split('x')[0]);

        for (int i = 0; i < size; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        }

        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                var cell = new Frame
                {
                    BorderColor = Colors.Black,
                    CornerRadius = 0,
                    Padding = 2,
                    Content = new BoxView { Color = Colors.Transparent },
                    BackgroundColor = Colors.Transparent,
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Fill
                };

                cell.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(() => OnCellTapped(cell))
                });

                grid.Children.Add(cell);
                Grid.SetRow(cell, row);
                Grid.SetColumn(cell, col);
            }
        }
    }

    private async void OnCellTapped(Frame cell)
    {
        if (cell.Content is BoxView)
        {
            SwitchPlayer();
            Image image;

            if (currentPlayer == Player.X)
            {
                image = new Image
                {
                    Source = "crossv2.gif",
                    Aspect = Aspect.AspectFit,
                    IsAnimationPlaying = true,
                };

                cell.Content = image;
                await Task.Delay(1000);
                image.Source = "crossplaceholder.png";
            }
            else
            {
                image = new Image
                {
                    Source = "circlev4.gif",
                    Aspect = Aspect.AspectFit,
                    IsAnimationPlaying = true,
                    Margin = new Thickness(15)
                };

                cell.Content = image;
                await Task.Delay(600);
                image.Source = "circleplaceholder.png";
            }
        }
    }

    private void SwitchPlayer()
    {
        currentPlayer = (currentPlayer == Player.X) ? Player.O : Player.X;
    }

    private void OnGridSizeChanged(object sender, EventArgs e)
    {
        if (gameControlButton.Text == "Tühista")
        {
            MakeCells();
        }
    }
}
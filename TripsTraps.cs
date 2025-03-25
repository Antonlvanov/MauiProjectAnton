using Microsoft.Maui.Layouts;
using System.ComponentModel;
using System.Drawing;

namespace MauiProjectAnton;

public class TripsTraps : ContentPage
{
    Grid grid;
    Picker playersPicker;
    Picker gridSizePicker;
    Label statusLabel;
    Button gameControlButton;
    StackLayout mainLayout;
    public enum Player { X, O, Bot }
    private Player currentPlayer;
    private bool isBotTurn;
    private bool gameStarted = false;
    private Random random = new Random();
    private bool isProcessingMove = false;

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
            VerticalTextAlignment = TextAlignment.Center,
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
        statusLabel = new Label
        {
            Text = "Valike mängurežiim",
            FontSize = 28,
            FontAttributes = FontAttributes.Bold,
            TextColor = Colors.White,
            BackgroundColor = Colors.Grey,
            Padding = new Thickness(10),
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Start,
            Margin = new Thickness(0, 20, 0, 10)
        };

        // control
        gameControlButton = new Button
        {
            Text = "Start",
            BackgroundColor = Colors.DarkGreen,
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
                statusLabel,
                pickersGrid,
                grid,
                gameControlButton
            },
            Spacing = 10,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill
        };

        Content = mainLayout;
    }

    private void OnGridSizeChanged(object sender, EventArgs e)
    {
        MakeCells();
    }

    // starter
    private async void OnGameControlClicked(object sender, EventArgs e)
    {
        if (gameStarted==false)
        {
            gameStarted = true;
            gameControlButton.IsVisible = false;

            if (playersPicker.SelectedIndex == 0)
            {
                currentPlayer = random.Next(2) == 0 ? Player.X : Player.O;
                statusLabel.Text = $"Käik: {currentPlayer}";
                MakeCells();
                if (currentPlayer == Player.O)
                {
                    isBotTurn = true;
                    await MakeBotMove();
                }
            }
            else
            {
                currentPlayer = random.Next(2) == 0 ? Player.X : Player.O;
                statusLabel.Text = $"Käik: {currentPlayer}";
                MakeCells();
            }
        }
    }

    // grid maker
    private void MakeCells()
    {
        grid.Children.Clear();
        grid.RowDefinitions.Clear();
        grid.ColumnDefinitions.Clear();

        int size = 3;
        if (gridSizePicker.SelectedIndex >= 0) // if grid size is selected
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

                if (gameStarted) // if game is started
                {
                    cell.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = new Command(() => OnCellTapped(cell))
                    });
                }
                grid.Children.Add(cell);
                Grid.SetRow(cell, row);
                Grid.SetColumn(cell, col);
            }
        }
    }
    // turn handler
    private async Task OnCellTapped(Frame cell)
    {
        if (!gameStarted || isProcessingMove || cell.Content is Image)
            return;
        isProcessingMove = true;
        Image image;
        try
        {
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
            if (CheckForWin(currentPlayer) || CheckForDraw())
            {
                EndGame();
                return;
            }
            if (playersPicker.SelectedIndex == 0 || isBotTurn)
            {
                isBotTurn = !isBotTurn;
            }
        }
        finally
        {
            isProcessingMove = false;
            SwitchPlayer();
        }
    }
    // switcher
    private async void SwitchPlayer()
    {
        currentPlayer = currentPlayer == Player.X ? Player.O : Player.X;
        statusLabel.Text = $"Käik: {currentPlayer}";
        statusLabel.BackgroundColor = (currentPlayer == Player.X) ? Colors.DarkRed : Colors.Black;

        if (playersPicker.SelectedIndex == 0 && isBotTurn)
        {
            await MakeBotMove();
        }
    }
    // bot
    private async Task MakeBotMove()
    {
        if (!gameStarted)
            return;
        if (CheckForWin(Player.O) || CheckForDraw())
            return;

        var emptyCells = grid.Children
            .OfType<Frame>()
            .Where(f => f.Content is not Image)
            .ToList();

        if (emptyCells.Count > 0)
        {
            Frame botMove = FindWinningMove(Player.X) ??
                            FindWinningMove(Player.O) ??
                            emptyCells[random.Next(emptyCells.Count)];

            await OnCellTapped(botMove);
        }
    }

    private Frame FindWinningMove(Player player)
    {
        int size = grid.ColumnDefinitions.Count;
        string targetImage = player == Player.X ? "crossplaceholder.png" : "circleplaceholder.png";

        foreach (var child in grid.Children)
        {
            if (child is Frame frame && frame.Content is not Image)
            {
                frame.Content = new Image { Source = targetImage };
                bool isWinning = CheckForWin(player);
                frame.Content = null;

                if (isWinning) return frame;
            }
        }
        return null;
    }

    // game status

    private void EndGame()
    {
        if (CheckForDraw())
        {
            statusLabel.Text = "Viik!";
            statusLabel.BackgroundColor = Colors.DarkOrange;
            DisplayAlert("Mäng läbi", "Mäng jäi viiki!", "OK");
            gameStarted = false;
            gameControlButton.IsVisible = true;
        }
        else
        {
            statusLabel.Text = $"{currentPlayer} võitis!";
            statusLabel.BackgroundColor = (currentPlayer == Player.X) ? Colors.DarkRed : Colors.Black;
            DisplayAlert("Mäng läbi", $"{currentPlayer} võitis!", "OK");
            gameStarted = false;
            gameControlButton.IsVisible = true;
        }
    }

    public bool CheckForWin(Player player)
    {
        int size = grid.ColumnDefinitions.Count;

        string targetImage = player == Player.X
            ? "crossplaceholder.png"
            : "circleplaceholder.png";

        for (int i = 0; i < size; i++)
        {
            if (CheckLine(i, 0, 0, 1, targetImage) ||
                CheckLine(0, i, 1, 0, targetImage))
                return true;
        }

        return CheckLine(0, 0, 1, 1, targetImage) ||
               CheckLine(0, size - 1, 1, -1, targetImage);
    }

    private bool CheckLine(int startRow, int startCol, int rowStep, int colStep, string targetImage)
    {
        int size = grid.ColumnDefinitions.Count;

        for (int i = 0; i < size; i++)
        {
            int row = startRow + i * rowStep;
            int col = startCol + i * colStep;

            var cell = grid.Children.OfType<Frame>().FirstOrDefault(c =>
                Grid.GetRow((BindableObject)c) == row && Grid.GetColumn((BindableObject)c) == col);

            if (cell?.Content is not Image image || image.Source is not FileImageSource fileImage || fileImage.File != targetImage)
                return false;
        }
        return true;
    }

    private bool CheckForDraw()
    {
        int size = grid.ColumnDefinitions.Count;
        foreach (var child in grid.Children)
        {
            if (child is Frame frame && frame.Content is not Image)
            {
                return false;
            }
        }
        return !CheckForWin(Player.X) && !CheckForWin(Player.O);
    }
}
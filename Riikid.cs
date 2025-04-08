using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MauiProjectAnton
{
    public class Riikid : ContentPage
    {
        private readonly ObservableCollection<Country> countries = new();
        private readonly CountryService _countryService;
        private readonly AppDbContext _db;
        private ActivityIndicator loadingIndicator;
        private CollectionView countriesCollection;
        private Button loadButton;

        public Riikid(AppDbContext db)
        {
            _db = db;
            _countryService = new CountryService(_db);
            
            CreateUI();
            LoadCountriesFromDb();
        }

        private void CreateUI()
        {
            loadingIndicator = new ActivityIndicator
            {
                IsVisible = false,
                IsRunning = false,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            loadButton = new Button
            {
                Text = "Laadi riigid",
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 20)
            };
            loadButton.Clicked += OnLoadCountriesClicked;

            countriesCollection = new CollectionView
            {
                SelectionMode = SelectionMode.Single,
                VerticalOptions = LayoutOptions.FillAndExpand,
                ItemTemplate = new DataTemplate(() =>
                {
                    var flagImage = new Image
                    {
                        WidthRequest = 50,
                        HeightRequest = 30,
                        Aspect = Aspect.AspectFill,
                    };
                    flagImage.SetBinding(Image.SourceProperty, new Binding("Flag",
                        converter: new UriToImageSourceConverter()));

                    var nameLabel = new Label
                    {
                        FontSize = 16,
                        FontAttributes = FontAttributes.Bold,
                        VerticalOptions = LayoutOptions.Center
                    };
                    nameLabel.SetBinding(Label.TextProperty, "Name");

                    var regionLabel = new Label
                    {
                        FontSize = 12,
                        VerticalOptions = LayoutOptions.Center,
                        TextColor = Colors.Gray
                    };
                    regionLabel.SetBinding(Label.TextProperty, "Region");

                    // Добавляем подпись для столицы
                    var capitalLayout = new HorizontalStackLayout
                    {
                        Spacing = 5,
                        VerticalOptions = LayoutOptions.Center
                    };
                    capitalLayout.Children.Add(new Label
                    {
                        Text = "Pealinn:",
                        FontAttributes = FontAttributes.Bold,
                        FontSize = 12
                    });
                    var capitalValueLabel = new Label
                    {
                        FontSize = 12,
                        VerticalOptions = LayoutOptions.Center
                    };
                    capitalValueLabel.SetBinding(Label.TextProperty, "Capital");
                    capitalLayout.Children.Add(capitalValueLabel);

                    var populationLayout = new HorizontalStackLayout
                    {
                        Spacing = 5,
                        VerticalOptions = LayoutOptions.Center
                    };
                    populationLayout.Children.Add(new Label
                    {
                        Text = "Rahvaarv:",
                        FontAttributes = FontAttributes.Bold,
                        FontSize = 12
                    });
                    var populationValueLabel = new Label
                    {
                        FontSize = 12,
                        VerticalOptions = LayoutOptions.Center
                    };
                    populationValueLabel.SetBinding(Label.TextProperty, new Binding("Population", stringFormat: "{0:N0}"));
                    populationLayout.Children.Add(populationValueLabel);

                    var textLayout = new VerticalStackLayout
                    {
                        Padding = new Thickness(10, 0),
                        VerticalOptions = LayoutOptions.Center,
                        Spacing = 2
                    };
                    textLayout.Children.Add(nameLabel);
                    textLayout.Children.Add(regionLabel);
                    textLayout.Children.Add(capitalLayout);
                    textLayout.Children.Add(populationLayout);

                    var grid = new Grid
                    {
                        Padding = 10,
                        VerticalOptions = LayoutOptions.Center
                    };
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

                    grid.Children.Add(flagImage);
                    grid.Children.Add(textLayout);
                    Grid.SetColumn(textLayout, 1);

                    return grid;
                })
            };

            countriesCollection.SelectionChanged += OnCountrySelected;

            var mainGrid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = GridLength.Auto }
                }
            };

            var scrollView = new ScrollView
                {
                    Content = countriesCollection
                };
            Grid.SetRow(scrollView, 0);
            mainGrid.Children.Add(scrollView);

            var bottomPanel = new VerticalStackLayout
            {
                Children =
                {
                    loadingIndicator,
                    loadButton
                },
                Spacing = 10,
                Padding = 10
            };
            Grid.SetRow(bottomPanel, 1);
            mainGrid.Children.Add(bottomPanel);

            Content = mainGrid;
        }

        private void OnCountrySelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Country selectedCountry)
            {
                DisplayAlert("Info",
                    $"{selectedCountry.Name}\n" +
                    $"Pealinn: {selectedCountry.Capital}\n" +
                    $"Piirkond: {selectedCountry.Region}\n" +
                    $"Rahvaarv: {selectedCountry.Population:N0}",
                    "OK");
            }
            ((CollectionView)sender).SelectedItem = null;
        }

        private async void OnLoadCountriesClicked(object sender, EventArgs e)
        {
            loadingIndicator.IsVisible = true;
            loadingIndicator.IsRunning = true;
            loadButton.IsEnabled = false;

            try
            {
                if (!await IsInternetAvailable())
                {
                    await DisplayAlert("Ошибка", "Отсутствует подключение к интернету", "OK");
                    return;
                }

                await _countryService.LoadCountriesAsync();
                await LoadCountriesFromDb();
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;

                if (ex.InnerException != null)
                {
                    errorMessage += $"\nInner: {ex.InnerException.Message}";
                    if (ex.InnerException is Microsoft.Data.Sqlite.SqliteException sqlEx)
                    {
                        errorMessage += $"\nSQL Error: {sqlEx.SqliteErrorCode}";
                    }
                }

                await DisplayAlert("Ошибка", $"Не удалось загрузить страны:\n{errorMessage}", "OK");
                Debug.WriteLine($"Full error: {ex.ToString()}");
            }
            finally
            {
                loadingIndicator.IsVisible = false;
                loadingIndicator.IsRunning = false;
                loadButton.IsEnabled = true;
            }
        }

        private async Task LoadCountriesFromDb()
        {
            try
            {
                var countriesList = await _countryService.GetCountriesAsync();

                if (countriesList.Count == 0)
                {
                    Debug.WriteLine("No countries found in database");
                    return;
                }

                foreach (var country in countriesList.Take(5))
                {
                    Debug.WriteLine($"Country: {country.Name}, " +
                                   $"Capital: {country.Capital}, " +
                                   $"Population: {country.Population}, " +
                                   $"Flag: {country.Flag}, " +
                                   $"Region: {country.Region}, "
                                   );
                }

                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    countriesCollection.ItemsSource = countriesList;
                    Debug.WriteLine($"Displaying {countriesList.Count} countries");
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading from DB: {ex}");
                await DisplayAlert("Error", $"Failed to load from DB: {ex.Message}", "OK");
            }
        }

        private async Task<bool> IsInternetAvailable()
        {
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current != NetworkAccess.Internet)
                {
                    return false;
                }
                using (var ping = new System.Net.NetworkInformation.Ping())
                {
                    var reply = await ping.SendPingAsync("8.8.8.8", 3000);
                    return reply.Status == System.Net.NetworkInformation.IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }
    }

    public class CountryService
    {
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _dbContext;

        public CountryService(AppDbContext dbContext)
        {
            _httpClient = new HttpClient();
            _dbContext = dbContext;
        }

        public async Task LoadCountriesAsync()
        {
            try
            {
                using HttpClient client = new HttpClient();
                string url = "https://restcountries.com/v3.1/all?fields=name,capital,flags,population,region,cca2";
                var response = await client.GetStringAsync(url);
                //Debug.WriteLine($"API Response: {response}");

                var apiCountries = JsonSerializer.Deserialize<List<ApiCountry>>(response);

                var countries = apiCountries
                    .Where(c => c.name != null && c.name.common != null)
                    .Select(c => new Country
                    {
                        Name = c.name.common,
                        Capital = c.capital != null && c.capital.Any() ? c.capital[0] : "—",
                        Flag = c.flags?.png,
                        Population = c.population,
                        Region = c.region
                    })
                    .ToList();

                foreach (var country in countries.Take(5))
                {
                    Debug.WriteLine($"Preparing to save: {country.Name}, Pop: {country.Population}, Flag: {country.Flag}, ");
                }

                await _dbContext.Database.ExecuteSqlRawAsync("DELETE FROM Countries");
                await _dbContext.Countries.AddRangeAsync(countries);
                await _dbContext.SaveChangesAsync();

                Debug.WriteLine($"Successfully saved {countries.Count} countries");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex}");
                throw;
            }
        }

        private class ApiCountry
        {
            public Name name { get; set; }
            public List<string> capital { get; set; }
            public Flags flags { get; set; }
            public int population { get; set; }
            public string region { get; set; }
            public string cca2 { get; set; }
        }

        private class Name
        {
            public string common { get; set; }
        }

        private class Flags
        {
            public string png { get; set; }
            public string svg { get; set; }
        }

        public async Task<List<Country>> GetCountriesAsync()
        {
            var countries = await _dbContext.Countries.ToListAsync();
            Debug.WriteLine($"Loaded {countries.Count} countries from DB");
            return countries;
        }
    }

    public class UriToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string uriString && Uri.TryCreate(uriString, UriKind.Absolute, out var uri))
            {
                try
                {
                    return ImageSource.FromUri(uri);
                }
                catch
                {
                    return ImageSource.FromFile("placeholder_flag.png");
                }
            }
            return ImageSource.FromFile("placeholder_flag.png");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

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
        private async Task<bool> IsInternetAvailable()
        {
            try
            {
                // Проверяем доступность сети
                var current = Connectivity.NetworkAccess;
                if (current != NetworkAccess.Internet)
                {
                    return false;
                }

                // Дополнительная проверка через ping (опционально)
                using (var ping = new System.Net.NetworkInformation.Ping())
                {
                    var reply = await ping.SendPingAsync("8.8.8.8", 3000); // Google DNS
                    return reply.Status == System.Net.NetworkInformation.IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
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
                Text = "Загрузить страны",
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 20)
            };
            loadButton.Clicked += OnLoadCountriesClicked;

            countriesCollection = new CollectionView
            {
                SelectionMode = SelectionMode.Single,
                VerticalScrollBarVisibility = ScrollBarVisibility.Always,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 500, // Фиксированная высота для скролла
                ItemTemplate = new DataTemplate(() =>
                {
                    var flagImage = new Image
                    {
                        WidthRequest = 50,
                        HeightRequest = 30,
                        Aspect = Aspect.AspectFill,
                        Source = "placeholder_flag.png" // Локальный файл-заглушка
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

                    var capitalLabel = new Label
                    {
                        FontSize = 14,
                        VerticalOptions = LayoutOptions.Center
                    };
                    capitalLabel.SetBinding(Label.TextProperty, "Capital");

                    var populationLabel = new Label
                    {
                        FontSize = 12,
                        VerticalOptions = LayoutOptions.Center
                    };
                    populationLabel.SetBinding(Label.TextProperty, new Binding("Population", stringFormat: "{0:N0}"));

                    var textLayout = new VerticalStackLayout
                    {
                        Padding = new Thickness(10, 0),
                        VerticalOptions = LayoutOptions.Center
                    };
                    textLayout.Children.Add(nameLabel);
                    textLayout.Children.Add(capitalLabel);
                    textLayout.Children.Add(populationLabel);

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

            // Обработчик нажатия на элемент
            countriesCollection.SelectionChanged += OnCountrySelected;

            Content = new ScrollView // Основной ScrollView
            {
                Content = new StackLayout
                {
                    Children =
            {
                loadButton,
                loadingIndicator,
                new ScrollView // ScrollView для списка стран
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Content = countriesCollection
                }
            }
                }
            };
        }

        private void OnCountrySelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Country selectedCountry)
            {
                DisplayAlert("Информация",
                    $"{selectedCountry.Name}\nСтолица: {selectedCountry.Capital}\nНаселение: {selectedCountry.Population:N0}",
                    "OK");
            }
            ((CollectionView)sender).SelectedItem = null; // Сбрасываем выбор
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

                // Добавляем внутреннее исключение если есть
                if (ex.InnerException != null)
                {
                    errorMessage += $"\nInner: {ex.InnerException.Message}";

                    // Для SQLite ошибок
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

                // Логируем первые 5 стран для проверки данных
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
    }

    public class CountryService
    {
        private const string ApiUrl = "http://api.countrylayer.com/v2/all";
        private const string ApiKey = "96f822c6c22511926026e1e1333f97da";

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
                Debug.WriteLine($"API Response: {response}"); // Полный вывод для диагностики

                var apiCountries = JsonSerializer.Deserialize<List<ApiCountry>>(response);

                var countries = apiCountries
                    .Where(c => c.name != null && c.name.common != null)
                    .Select(c => new Country
                    {
                        Name = c.name.common,
                        Capital = c.capital != null && c.capital.Any() ? c.capital[0] : "—",
                        Flag = c.flags?.png ?? $"https://flagcdn.com/w160/{c.cca2?.ToLower()}.png",
                        Population = c.population,
                        Region = c.region
                    });

                // Проверка данных перед сохранением
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

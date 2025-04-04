using CommunityToolkit.Maui.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;
using System.Collections.ObjectModel;

namespace MauiProjectAnton;

public class KontaktiAndmed : ContentPage
{
    private readonly AppDbContext _db;
    private readonly ObservableCollection<Contact> _contacts = new();
    private Expander _currentExpanded;

    public KontaktiAndmed(AppDbContext db)
    {
        Title = "Kontaktid";
        _db = db;

        var collectionView = new CollectionView
        {
            ItemsSource = _contacts,
            ItemTemplate = new DataTemplate(() =>
            {
                var contact = new Contact();
                var expander = new Expander
                {
                    Header = CreateExpanderHeader(contact),
                    Content = CreateExpanderContent(contact),
                    BindingContext = contact
                };
                expander.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "IsExpanded")
                    {
                        var current = (Expander)sender;
                        if (current.IsExpanded)
                        {
                            if (_currentExpanded != null && _currentExpanded != current)
                            {
                                _currentExpanded.IsExpanded = false;
                            }
                            _currentExpanded = current;
                            UpdateExpanderAppearance(current, true);
                        }
                        else
                        {
                            if (_currentExpanded == current)
                            {
                                _currentExpanded = null;
                            }
                            UpdateExpanderAppearance(current, false);
                        }
                    }
                };

                return expander;
            }),
            SelectionMode = SelectionMode.Single,
            BackgroundColor = Colors.White,
        };

        var addButton = new Button
        {
            Text = "Lisa kontakt",
            BackgroundColor = Colors.Black,
            WidthRequest = 200,
            Margin = new Thickness(20, 10, 20, 10),
            HorizontalOptions = LayoutOptions.Center
        };
        addButton.Clicked += OnAddContact;

        var grid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Star },
                new RowDefinition { Height = GridLength.Auto } 
            },
            BackgroundColor = Colors.LightGray
        };

        var framedCollectionView = new Frame
        {
            Content = collectionView,
            BorderColor = Colors.Gray, 
            CornerRadius = 10, 
            HasShadow = true, 
            Padding = 0,
            Margin = new Thickness(40, 10),
            BackgroundColor = Colors.White
        };

        grid.Add(framedCollectionView, 0, 0);
        grid.Add(addButton, 0, 1);

        Content = grid;
        LoadContacts();
    }

    private async void LoadContacts()
    {
        _contacts.Clear();
        var contacts = await _db.Contacts.ToListAsync();
        foreach (var contact in contacts)
        {
            _contacts.Add(contact);
        }
    }

    private async void OnAddContact(object sender, EventArgs e)
    {
        var popup = new AddContactPopup(_db, _contacts);
        await Application.Current.MainPage.ShowPopupAsync(popup);
    }

    private View CreateExpanderHeader(Contact contact)
    {
        var mainLayout = new StackLayout {
            Padding = new Thickness(0,2), //
            BackgroundColor = Colors.White,
            Margin = new Thickness(20, 0),
        };

        var contactInfo = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Auto },
                new ColumnDefinition { Width = GridLength.Star }
            },
            BackgroundColor = Colors.White,
        };

        var contactImage = new Image { HeightRequest = 50, WidthRequest = 50, Margin = new Thickness(0, 0, 10, 0) };
        contactImage.SetBinding(Image.SourceProperty, "PhotoPath");

        var contactDetails = new StackLayout();
        var nameLabel = new Label { FontSize = 16, FontAttributes = FontAttributes.Bold };
        var phoneLabel = new Label { FontSize = 14 };
        var emailLabel = new Label { FontSize = 14 };

        nameLabel.SetBinding(Label.TextProperty, "Name");
        phoneLabel.SetBinding(Label.TextProperty, "Phone");
        emailLabel.SetBinding(Label.TextProperty, "Email");

        contactDetails.Children.Add(nameLabel);
        contactDetails.Children.Add(phoneLabel);
        contactDetails.Children.Add(emailLabel);

        contactInfo.Children.Add(contactImage);
        contactInfo.Children.Add(contactDetails);
        Grid.SetColumn(contactDetails, 1);

        mainLayout.Children.Add(contactInfo);
        return mainLayout;
    }

    private View CreateExpanderContent(Contact contact)
    {
        var buttonsContainer = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            HorizontalOptions = LayoutOptions.Center,
            Padding = new Thickness(10, 0),
            Children =
            {
                CreateCircleButton("call.png", async c =>
                {
                    if (!string.IsNullOrEmpty(c.Phone))
                        await Launcher.OpenAsync(new Uri($"tel:{c.Phone}"));
                }),

                CreateCircleButton("sms.png", async c =>
                {
                    if (!string.IsNullOrEmpty(c.Phone))
                        await Launcher.OpenAsync(new Uri($"sms:{c.Phone}"));
                }),

                CreateCircleButton("email2.png", async c =>
                {
                    if (!string.IsNullOrEmpty(c.Email))
                        await Launcher.OpenAsync(new Uri($"mailto:{c.Email}"));
                })
            }
        };

        return new Frame
        {
            Content = buttonsContainer,
            Margin = new Thickness(20, 0, 20, 0),
            Padding = new Thickness(0, 0),
            BorderColor = Colors.LightGray,
            HasShadow = false,
            BackgroundColor = Colors.LightGray,
            HeightRequest = 50
        };
    }

    private ImageButton CreateCircleButton(string imageSource, Func<Contact, Task> action)
    {
        var button = new ImageButton
        {
            Source = imageSource,
            Aspect = Aspect.AspectFit,
            BackgroundColor = Colors.LightGray,
            WidthRequest = 70,
            HeightRequest = 70,
            CornerRadius = 35,
            Padding = 8,
            Margin = new Thickness(5)
        };

        button.Pressed += (s, e) => button.BackgroundColor = Color.FromRgba(0, 0, 0, 0.1);
        button.Released += (s, e) => button.BackgroundColor = Colors.Transparent;

        button.Clicked += async (s, e) =>
        {
            if (s is ImageButton btn && btn.BindingContext is Contact c)
            {
                await btn.ScaleTo(0.95, 100);
                await btn.ScaleTo(1, 100);
                await action(c);
            }
        };

        return button;
    }


    private void UpdateExpanderAppearance(Expander expander, bool isSelected)
    {
        if (expander.Header is StackLayout headerLayout)
        {
            if (headerLayout.Children.FirstOrDefault() is Grid headerGrid)
            {
                headerGrid.BackgroundColor = isSelected ? Colors.LightGray : Colors.White;
            }
        }
    }

    private class AddContactPopup : Popup
    {
        private readonly Entry _nameEntry = new Entry { Placeholder = "Nimi" };
        private readonly Entry _phoneEntry = new Entry { Placeholder = "Telefon", Keyboard = Keyboard.Telephone };
        private readonly Entry _emailEntry = new Entry { Placeholder = "Email", Keyboard = Keyboard.Email };
        private readonly Editor _descriptionEditor = new Editor { HeightRequest = 100 };
        private readonly Image _photoImage = new Image { HeightRequest = 100, WidthRequest = 100, Margin = new Thickness(20, 10, 20, 10) };
        private string _photoPath;
        private readonly Button _takePhotoButton = new Button { Text = "Lisa foto", BackgroundColor = Colors.Black, WidthRequest = 100 };
        private readonly AppDbContext _db;
        private readonly ObservableCollection<Contact> contacts;

        public AddContactPopup(AppDbContext db, ObservableCollection<Contact> _contacts)
        {
            _db = db;
            contacts = _contacts;
            this.Size = new Size(300, 600);
            this.CanBeDismissedByTappingOutsideOfPopup = true;

            _takePhotoButton.Clicked += OnTakePhoto;

            var saveButton = new Button { Text = "Salvesta", BackgroundColor = Colors.Black };
            saveButton.Clicked += OnSaveContact;

            var cancelButton = new Button { Text = "Tagasi", BackgroundColor = Colors.Black };
            cancelButton.Clicked += (s, e) => this.Close();

            var inputGrid = new Grid
            {
                ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Auto },
                new ColumnDefinition { Width = GridLength.Star }
            },
                RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto }
            },
                RowSpacing = 15,
                ColumnSpacing = 10
            };

            var labelStyle = new Style(typeof(Label))
            {
                Setters =
            {
                new Setter { Property = Label.MarginProperty, Value = new Thickness(0, 10, 0, 0) }
            }
            };

            inputGrid.Add(new Label { Text = "Nimi:", Style = labelStyle }, 0, 0);
            inputGrid.Add(_nameEntry, 1, 0);

            inputGrid.Add(new Label { Text = "Telefon:", Style = labelStyle }, 0, 1);
            inputGrid.Add(_phoneEntry, 1, 1);

            inputGrid.Add(new Label { Text = "Email:", Style = labelStyle }, 0, 2);
            inputGrid.Add(_emailEntry, 1, 2);

            inputGrid.Add(new Label { Text = "Lisainfo:", Style = labelStyle }, 0, 3);
            inputGrid.Add(_descriptionEditor, 1, 3);
            Grid.SetRowSpan(_descriptionEditor, 1);

            Content = new ScrollView
            {
                Content = new StackLayout
                {
                    Padding = 20,
                    Children =
                {
                    new Label {
                        Text = "Lisa uus kontakt",
                        FontAttributes = FontAttributes.Bold,
                        FontSize = 18,
                        HorizontalOptions = LayoutOptions.Center
                    },
                    _photoImage,
                    _takePhotoButton,
                    inputGrid,
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.Center,
                        Spacing = 10,
                        Children = { cancelButton, saveButton }
                    }
                }
                }
            };
        }

        private async void OnSaveContact(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_nameEntry.Text))
            {
                await Shell.Current.DisplayAlert("Viga", "Palun sisesta nimi", "OK");
                return;
            }

            var newContact = new Contact
            {
                Name = _nameEntry.Text,
                Phone = _phoneEntry.Text,
                Email = _emailEntry.Text,
                Description = _descriptionEditor.Text,
                PhotoPath = _photoPath
            };

            await _db.Contacts.AddAsync(newContact);
            await _db.SaveChangesAsync();
            contacts.Add(newContact);
            Close();
        }
        private async void OnTakePhoto(object sender, EventArgs e)
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                if (photo != null)
                {
                    await SavePhoto(photo);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Viga", ex.Message, "OK");
            }
        }

        private async Task SavePhoto(FileResult foto)
        {
            if (foto != null)
            {
                _photoPath = Path.Combine(FileSystem.CacheDirectory, foto.FileName);

                using Stream sourceStream = await foto.OpenReadAsync();
                using FileStream localFileStream = File.OpenWrite(_photoPath);
                await sourceStream.CopyToAsync(localFileStream);

                _photoImage.Source = ImageSource.FromFile(_photoPath);

                await Shell.Current.DisplayAlert("Edu", "Foto on edukalt salvestatud", "Ok");
            }
        }
    }
}

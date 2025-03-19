namespace MauiProjectAnton;

public partial class PopUpPage : ContentPage
{
    public PopUpPage()
    {
        Button alertButton = new Button
        {
            Text = "Teade",
            VerticalOptions = LayoutOptions.Start,
            HorizontalOptions = LayoutOptions.Center
        };
        alertButton.Clicked += AlertButton_Clicked;
        Button alertYesNOButton = new Button
        {
            Text = "Jah v�i ei",
            VerticalOptions = LayoutOptions.Start,
            HorizontalOptions = LayoutOptions.Center
        };
        alertYesNOButton.Clicked += AlertYesNOButton_Clicked;
        Button alertListButton = new Button
        {
            Text = "Valik",
            VerticalOptions = LayoutOptions.Start,
            HorizontalOptions = LayoutOptions.Center
        };
        alertListButton.Clicked += AlertListButton_Clicked;
        Button alertQuestButton = new Button
        {
            Text = "K�simus",
            VerticalOptions = LayoutOptions.Start,
            HorizontalOptions = LayoutOptions.Center
        };
        alertQuestButton.Clicked += AlertQuestButton_Clicked;
        Content = new StackLayout { Children = { alertButton, alertYesNOButton, alertListButton, alertQuestButton } };
    }

    //Valiksvastusega
    private async void AlertQuestButton_Clicked(object? sender, EventArgs e)
    {
        string result1 = await DisplayPromptAsync("K�simus", "Kuidas l�heb?", placeholder: "Tore!");
        string result2 = await DisplayPromptAsync("Vasta", "Millega v�rdub 5+5?", initialValue: "10", keyboard: Keyboard.Numeric);
    }

    private async void AlertListButton_Clicked(object? sender, EventArgs e)
    {
        var action = await DisplayActionSheet("Mida teha?", "Loobu", "Kustutada", "Tantsida", "Laulda", "Joonestada");
    }

    //Valikuga
    private async void AlertYesNOButton_Clicked(object? sender, EventArgs e)
    {
        bool result = await DisplayAlert("Kinnitus", "Kas oled kindel?", "Olen kindel", "Ei ole kindel");
        await DisplayAlert("Teade", "Teie valik on: " + (result ? "Jah" : "Ei"), "OK");
    }

    //Lihne teade
    private void AlertButton_Clicked(object? sender, EventArgs e)
    {
        DisplayAlert("Teade", "Teil on uus teade", "OK");
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MauiProjectAnton
{
    public partial class AppShell : Shell
    {
        private readonly IServiceProvider _serviceProvider;
        public AppShell(IServiceProvider serviceProvider)
        {
            InitializeComponent(); 
            _serviceProvider = serviceProvider;
            RegisterRoutes();
        }

        void RegisterRoutes()
        {
            Routing.RegisterRoute(nameof(StartPage), typeof(StartPage));
            Routing.RegisterRoute(nameof(TextPage), typeof(TextPage));
            Routing.RegisterRoute(nameof(FigurePage), typeof(FigurePage));
            Routing.RegisterRoute(nameof(TimerPage), typeof(TimerPage));
            Routing.RegisterRoute(nameof(Valgusfoor), typeof(Valgusfoor));
            Routing.RegisterRoute(nameof(DateTimePage), typeof(DateTimePage));
            Routing.RegisterRoute(nameof(ColorPage), typeof(ColorPage));
            Routing.RegisterRoute(nameof(Lumememm), typeof(Lumememm));
            Routing.RegisterRoute(nameof(TripsTraps), typeof(TripsTraps));
            Routing.RegisterRoute(nameof(PickerImagePage), typeof(PickerImagePage));
            Routing.RegisterRoute(nameof(PopUpPage), typeof(PopUpPage));
            Routing.RegisterRoute(nameof(KontaktiAndmed), typeof(KontaktiAndmed));
            // Routing.RegisterRoute(nameof(Test), typeof(Test));
        }
        private async void OpenKontaktiAndmed(object sender, EventArgs e)
        {
            var page = _serviceProvider.GetRequiredService<KontaktiAndmed>();
            await Navigation.PushAsync(page);
        }
    }
}

namespace MauiProjectAnton
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
        }

        void RegisterRoutes()
        {
            Routing.RegisterRoute(nameof(StartPage), typeof(StartPage));
            Routing.RegisterRoute(nameof(TextPage), typeof(TextPage));
            Routing.RegisterRoute(nameof(FigurePage), typeof(FigurePage));
            Routing.RegisterRoute(nameof(TimerPage), typeof(TimerPage));
        }
    }
}

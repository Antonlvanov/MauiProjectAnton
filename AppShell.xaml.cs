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
            Routing.RegisterRoute(nameof(Valgusfoor), typeof(Valgusfoor));
            Routing.RegisterRoute(nameof(Lumememm), typeof(Lumememm));
            Routing.RegisterRoute(nameof(ColorPage), typeof(ColorPage));
            Routing.RegisterRoute(nameof(DateTimePage), typeof(DateTimePage));
            Routing.RegisterRoute(nameof(Test), typeof(Test));
        }
    }
}

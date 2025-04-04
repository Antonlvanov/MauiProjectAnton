namespace MauiProjectAnton
{
    public partial class App : Application
    {
        public App(AppShell shell)
        {
            Current.UserAppTheme = AppTheme.Light;
            InitializeComponent();
            MainPage = shell;
        }
    }
}

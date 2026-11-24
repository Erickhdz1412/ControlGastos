using Microsoft.Maui.LifecycleEvents;

namespace ControlGastos
{
    public partial class App : Application
    {
        public static DatabaseService Database { get; private set; } = null!;
        public App()
        {
            InitializeComponent();

            // Ruta de la base de datos
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "gastos.db3");
            Database = new DatabaseService(dbPath);
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}

using Microsoft.Maui.Controls;

namespace CadastroEvento
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell(); // Inicializa a navegação com a AppShell
        }
    }
}
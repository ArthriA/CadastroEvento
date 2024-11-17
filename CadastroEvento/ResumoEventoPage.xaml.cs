using CadastroEvento;

namespace CadastroEvento
{
    public partial class ResumoEventoPage : ContentPage
    {
        public Evento Evento { get; set; }

        // Construtor que recebe o objeto Evento
        public ResumoEventoPage(Evento evento)
        {
            InitializeComponent();
            Evento = evento; // Atribui o objeto Evento à propriedade
            BindingContext = Evento; // Define o BindingContext para os dados do Evento
        }

        // Método para voltar à página anterior (Cadastro de Evento)
        private async void OnVoltarClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(); // Navega de volta para a página anterior
        }
    }
}
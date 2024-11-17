using CadastroEvento;
using System.Globalization;

namespace CadastroEvento
{
    public partial class CadastroEventoPage : ContentPage
    {
        public Evento Evento { get; set; }

        public CadastroEventoPage()
        {
            InitializeComponent();
            Evento = new Evento(); // Cria��o do objeto Evento
            BindingContext = this; // Altera aqui para vincular o BindingContext corretamente

            // Define as datas do calend�rio a partir deste ano e m�s
            var dataAtual = DateTime.Now;
            DataEventoPicker.Date = new DateTime(dataAtual.Year, dataAtual.Month, 1);  // Define o m�s atual
            DataFimEventoPicker.Date = new DateTime(dataAtual.Year, dataAtual.Month, 1); // Define o m�s atual
            DataEventoPicker.MinimumDate = DateTime.Today;
            DataFimEventoPicker.MinimumDate = DateTime.Today;
        }

        // M�todo chamado quando o bot�o de salvar � clicado
        private async void OnSalvarEventoClicked(object sender, EventArgs e)
        {
            try
            {
                // Verifica se os campos obrigat�rios est�o preenchidos
                if (string.IsNullOrEmpty(NomeEventoEntry.Text) || string.IsNullOrEmpty(LocalEventoEntry.Text))
                {
                    await DisplayAlert("Erro", "Por favor, preencha todos os campos obrigat�rios.", "OK");
                    return;
                }

                // Popula o objeto Evento com os dados inseridos
                Evento evento = new Evento
                {
                    NomeEvento = NomeEventoEntry.Text,
                    DataInicio = DataEventoPicker.Date,
                    DataFim = DataFimEventoPicker.Date,
                    HorarioInicio = HorarioInicioPicker.Time,
                    HorarioTermino = HorarioTerminoPicker.Time,
                    Local = LocalEventoEntry.Text,
                    Descricao = DescricaoEventoEditor.Text,
                    NomeOrganizador = NomeOrganizadorEntry.Text
                };

                // Valida o n�mero de participantes
                if (!int.TryParse(NumeroParticipantesEntry.Text, out int numeroParticipantes))
                {
                    await DisplayAlert("Erro", "O n�mero de participantes deve ser um valor num�rico v�lido.", "OK");
                    return;
                }
                evento.NumeroParticipantes = numeroParticipantes;

                // Valida o or�amento
                if (!decimal.TryParse(OrcamentoEventoEntry.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal orcamento))
                {
                    await DisplayAlert("Erro", "O or�amento deve ser um valor num�rico v�lido.", "OK");
                    return;
                }
                evento.OrcamentoEvento = orcamento;

                // Valida o contato de emerg�ncia
                if (!string.IsNullOrEmpty(ContatoEmergenciaEntry.Text) && !long.TryParse(ContatoEmergenciaEntry.Text, out _))
                {
                    await DisplayAlert("Erro", "O contato de emerg�ncia deve ser um n�mero v�lido.", "OK");
                    return;
                }
                evento.ContatoEmergencia = ContatoEmergenciaEntry.Text;

                // Outros campos
                evento.ResponsavelOrcamento = ResponsavelOrcamentoEntry.Text;
                evento.ResponsavelLogistica = ResponsavelLogisticaEntry.Text;
                evento.Observacoes = ObservacoesEditor.Text;

                // Navega para a p�gina de resumo, passando o objeto Evento
                await Navigation.PushAsync(new ResumoEventoPage(evento));
            }
            catch (Exception ex)
            {
                // Em caso de erro, exibe uma mensagem
                await DisplayAlert("Erro", $"Ocorreu um erro: {ex.Message}", "OK");
            }
        }

    }
}

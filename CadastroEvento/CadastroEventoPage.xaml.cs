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
            Evento = new Evento(); // Criação do objeto Evento
            BindingContext = this; // Altera aqui para vincular o BindingContext corretamente

            // Define as datas do calendário a partir deste ano e mês
            var dataAtual = DateTime.Now;
            DataEventoPicker.Date = new DateTime(dataAtual.Year, dataAtual.Month, 1);  // Define o mês atual
            DataFimEventoPicker.Date = new DateTime(dataAtual.Year, dataAtual.Month, 1); // Define o mês atual
            DataEventoPicker.MinimumDate = DateTime.Today;
            DataFimEventoPicker.MinimumDate = DateTime.Today;
        }

        // Método chamado quando o botão de salvar é clicado
        private async void OnSalvarEventoClicked(object sender, EventArgs e)
        {
            try
            {
                // Verifica se os campos obrigatórios estão preenchidos
                if (string.IsNullOrEmpty(NomeEventoEntry.Text) || string.IsNullOrEmpty(LocalEventoEntry.Text))
                {
                    await DisplayAlert("Erro", "Por favor, preencha todos os campos obrigatórios.", "OK");
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

                // Valida o número de participantes
                if (!int.TryParse(NumeroParticipantesEntry.Text, out int numeroParticipantes))
                {
                    await DisplayAlert("Erro", "O número de participantes deve ser um valor numérico válido.", "OK");
                    return;
                }
                evento.NumeroParticipantes = numeroParticipantes;

                // Valida o orçamento
                if (!decimal.TryParse(OrcamentoEventoEntry.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal orcamento))
                {
                    await DisplayAlert("Erro", "O orçamento deve ser um valor numérico válido.", "OK");
                    return;
                }
                evento.OrcamentoEvento = orcamento;

                // Valida o contato de emergência
                if (!string.IsNullOrEmpty(ContatoEmergenciaEntry.Text) && !long.TryParse(ContatoEmergenciaEntry.Text, out _))
                {
                    await DisplayAlert("Erro", "O contato de emergência deve ser um número válido.", "OK");
                    return;
                }
                evento.ContatoEmergencia = ContatoEmergenciaEntry.Text;

                // Outros campos
                evento.ResponsavelOrcamento = ResponsavelOrcamentoEntry.Text;
                evento.ResponsavelLogistica = ResponsavelLogisticaEntry.Text;
                evento.Observacoes = ObservacoesEditor.Text;

                // Navega para a página de resumo, passando o objeto Evento
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

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

                // Valida o orçamento do evento
                if (!decimal.TryParse(OrcamentoEventoEntry.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal orcamentoEvento))
                {
                    await DisplayAlert("Erro", "O orçamento do evento deve ser um valor numérico válido.", "OK");
                    return;
                }
                evento.OrcamentoEvento = orcamentoEvento;

                // Valida outras despesas
                if (!decimal.TryParse(OutrasDespesasEntry.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal outrasDespesas))
                {
                    await DisplayAlert("Erro", "O valor de outras despesas deve ser um número válido.", "OK");
                    return;
                }
                evento.OutrasDespesas = outrasDespesas;

                // Calcula o total do evento (soma)
                decimal totalEvento = orcamentoEvento + outrasDespesas;

                // Atribui o total ao evento e exibe no formato contábil
                evento.TotalEvento = totalEvento;

                // Exibe o valor total do evento no formato contábil
                TotalEventoEntry.Text = totalEvento.ToString("C2", CultureInfo.CurrentCulture); // Formato contábil

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




        private void CalculoTotal_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Valores iniciais
                decimal orcamento = 0, outrasDespesas = 0;

                // Verifica se os campos estão preenchidos e tenta converter para decimal
                if (!string.IsNullOrWhiteSpace(OrcamentoEventoEntry.Text))
                {
                    if (!decimal.TryParse(OrcamentoEventoEntry.Text, out orcamento))
                    {
                        DisplayAlert("Erro", "O valor do orçamento deve ser numérico.", "OK");
                        return;
                    }
                }

                if (!string.IsNullOrWhiteSpace(OutrasDespesasEntry.Text))
                {
                    if (!decimal.TryParse(OutrasDespesasEntry.Text, out outrasDespesas))
                    {
                        DisplayAlert("Erro", "O valor das outras despesas deve ser numérico.", "OK");
                        return;
                    }
                }

                // Soma os valores
                decimal valorTotal = orcamento + outrasDespesas;

                // Exibe o total no formato contábil (R$)
                TotalEventoEntry.Text = valorTotal.ToString("C", new System.Globalization.CultureInfo("pt-BR"));
            }
            catch (Exception ex)
            {
                DisplayAlert("Erro", $"Algo deu errado: {ex.Message}", "OK");
            }
        }


    }
}

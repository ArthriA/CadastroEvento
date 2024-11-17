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

                // Valida o or�amento do evento
                if (!decimal.TryParse(OrcamentoEventoEntry.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal orcamentoEvento))
                {
                    await DisplayAlert("Erro", "O or�amento do evento deve ser um valor num�rico v�lido.", "OK");
                    return;
                }
                evento.OrcamentoEvento = orcamentoEvento;

                // Valida outras despesas
                if (!decimal.TryParse(OutrasDespesasEntry.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal outrasDespesas))
                {
                    await DisplayAlert("Erro", "O valor de outras despesas deve ser um n�mero v�lido.", "OK");
                    return;
                }
                evento.OutrasDespesas = outrasDespesas;

                // Calcula o total do evento (soma)
                decimal totalEvento = orcamentoEvento + outrasDespesas;

                // Atribui o total ao evento e exibe no formato cont�bil
                evento.TotalEvento = totalEvento;

                // Exibe o valor total do evento no formato cont�bil
                TotalEventoEntry.Text = totalEvento.ToString("C2", CultureInfo.CurrentCulture); // Formato cont�bil

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




        private void CalculoTotal_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Valores iniciais
                decimal orcamento = 0, outrasDespesas = 0;

                // Verifica se os campos est�o preenchidos e tenta converter para decimal
                if (!string.IsNullOrWhiteSpace(OrcamentoEventoEntry.Text))
                {
                    if (!decimal.TryParse(OrcamentoEventoEntry.Text, out orcamento))
                    {
                        DisplayAlert("Erro", "O valor do or�amento deve ser num�rico.", "OK");
                        return;
                    }
                }

                if (!string.IsNullOrWhiteSpace(OutrasDespesasEntry.Text))
                {
                    if (!decimal.TryParse(OutrasDespesasEntry.Text, out outrasDespesas))
                    {
                        DisplayAlert("Erro", "O valor das outras despesas deve ser num�rico.", "OK");
                        return;
                    }
                }

                // Soma os valores
                decimal valorTotal = orcamento + outrasDespesas;

                // Exibe o total no formato cont�bil (R$)
                TotalEventoEntry.Text = valorTotal.ToString("C", new System.Globalization.CultureInfo("pt-BR"));
            }
            catch (Exception ex)
            {
                DisplayAlert("Erro", $"Algo deu errado: {ex.Message}", "OK");
            }
        }


    }
}

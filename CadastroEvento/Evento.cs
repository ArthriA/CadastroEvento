using System;

namespace CadastroEvento
{
    public class Evento
    {
        public string? NomeEvento { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public TimeSpan HorarioInicio { get; set; }
        public TimeSpan HorarioTermino { get; set; }
        public string? Local { get; set; }
        public string? Descricao { get; set; }
        public string? NomeOrganizador { get; set; }
        public int NumeroParticipantes { get; set; }
        public decimal OrcamentoEvento { get; set; }
        public string? ResponsavelOrcamento { get; set; }
        public string? ResponsavelLogistica { get; set; }
        public string? ContatoEmergencia { get; set; }
        public string? Observacoes { get; set; }

        // Calcula a duração do evento em dias
        public int DuracaoEvento => (DataFim - DataInicio).Days;

        // Verifica se a data de início é anterior à data de término
        public bool ValidarDatas()
        {
            return DataInicio <= DataFim;
        }
    }
}
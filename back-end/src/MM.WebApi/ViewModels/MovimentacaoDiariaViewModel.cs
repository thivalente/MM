using System;

namespace MM.WebApi.ViewModels
{
    public class MovimentacaoDiariaViewModel
    {
        // Formato: { data: diautil, entrada: true, periodo: periodo, rendimento: true, valor: rendimento, valor_di: rendimento_di, valor_poupanca: rendimento_poupanca }
        #region [ Construtores ]

        public MovimentacaoDiariaViewModel(Guid id, bool entrada, bool rendimento, decimal valor, decimal valor_di, decimal valor_poupanca, DateTime data)
        {
            this.id = id;
            this.entrada = entrada;
            this.rendimento = rendimento;
            this.valor = valor;
            this.valor_di = valor_di;
            this.valor_poupanca = valor_poupanca;
            this._data = data;
        }

        #endregion [ FIM - Construtores ]

        #region [ Propriedades ]

        public Guid id                  { get; set; }
        public string data              { get { return this._data.ToString("dd/MM/yyyy"); } }
        public bool entrada             { get; set; }
        public int periodo              { get { return Convert.ToInt32(this._data.ToString("yyyyMM")); } }
        public bool rendimento          { get; set; }
        public decimal valor            { get; set; }
        public decimal valor_di         { get; set; }
        public decimal valor_poupanca   { get; set; }

        private DateTime _data;

        #endregion [ FIM - Propriedades ]
    }
}

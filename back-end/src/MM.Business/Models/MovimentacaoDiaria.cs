using System;

namespace MM.Business.Models
{
    public class MovimentacaoDiaria
    {
        #region [ Construtores ]

        public MovimentacaoDiaria() { }

        public MovimentacaoDiaria(Guid id, Guid user_id, DateTime data, bool entrada, bool rendimento, decimal valor, decimal valor_di, decimal valor_poupanca)
        {
            this.id = id;
            this.usuario_id = user_id;
            this.data_criacao = data;
            this.entrada = entrada;
            this.rendimento = rendimento;
            this.valor = valor;
            this.valor_di = valor_di;
            this.valor_poupanca = valor_poupanca;
        }

        #endregion [ FIM - Construtores ]

        #region [ Propriedades ]

        public Guid id                  { get; private set; }
        public Guid usuario_id          { get; private set; }
        public DateTime data_criacao    { get; private set; }
        public bool entrada             { get; private set; }
        public bool rendimento          { get; private set; }
        public decimal valor            { get; private set; }
        public decimal valor_di         { get; private set; }
        public decimal valor_poupanca   { get; private set; }

        #endregion [ FIM - Propriedades ]
    }
}

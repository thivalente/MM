using System;
using System.Collections.Generic;
using System.Text;

namespace MM.Business.Models
{
    public class Movimentacao
    {
        #region [ Propriedades ]

        public Guid id                  { get; private set; }
        public Guid usuario_id          { get; private set; }
        public decimal valor            { get; private set; }
        public DateTime data_criacao    { get; private set; }
        public bool entrada             { get; private set; }
        public bool ativo               { get; private set; }

        #endregion [ FIM - Propriedades ]

        #region [ Construtores ]

        public Movimentacao() { }

        public Movimentacao(Guid id, Guid usuario_id, decimal valor, DateTime data_criacao, bool entrada, bool ativo)
        {
            this.id = id;
            this.usuario_id = usuario_id;
            this.valor = valor;
            this.data_criacao = data_criacao;
            this.entrada = entrada;
            this.ativo = ativo;
        }

        #endregion [ FIM - Construtores ]
    }
}

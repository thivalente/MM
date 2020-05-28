using System;

namespace MM.WebApi.ViewModels
{
    public class MovimentacaoViewModel
    {
        #region [ Propriedades ]

        public Guid id                  { get; set; }
        public Guid usuario_id          { get; set; }
        public decimal valor            { get; set; }
        public DateTime data_criacao    { get; set; }
        public bool entrada             { get; set; }
        public bool ativo               { get; set; }
        
        public string tipo              { get { return this.entrada ? "Aporte" : "Retirada"; } }

        #endregion [ FIM - Propriedades ]

        #region [ Construtores ]

        public MovimentacaoViewModel() { }

        public MovimentacaoViewModel(Guid id, Guid usuario_id, decimal valor, DateTime data_criacao, bool entrada, bool ativo)
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

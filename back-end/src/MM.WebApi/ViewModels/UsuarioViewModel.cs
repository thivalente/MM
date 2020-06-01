using MM.WebApi.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MM.WebApi.ViewModels
{
    public class UsuarioViewModel
    {
        #region [ Propriedades ]

        public Guid id                          { get; set; }
        [Required(ErrorMessage = "O nome é obrigatório")]
        public string nome                      { get; set; }
        [Required(ErrorMessage = "O cpf é obrigatório")]
        public string cpf                       { get; set; }
        [Required(ErrorMessage = "O e-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "Formato do e-mail inválido")]
        public string email                     { get; set; }
        public string senha                     { get; set; }
        public bool aceitou_termos              { get; set; }
        public DateTime? data_aceitou_termos    { get; set; }
        public DateTime data_criacao            { get; set; }
        [Required(ErrorMessage = "A taxa acima do cdi é obrigatória")]
        public decimal taxa_acima_cdi           { get; set; }
        public bool is_admin                    { get; set; }
        public bool trocar_senha                { get; set; }
        public bool ativo                       { get; set; }

        public string primeiro_nome             { get { return this.nome.GetFirstName(); } }

        private List<MovimentacaoViewModel> _movimentacoes;
        public IReadOnlyCollection<MovimentacaoViewModel> Movimentacoes => _movimentacoes;

        private List<ClaimViewModel> _claims;
        public IReadOnlyCollection<ClaimViewModel> Claims => _claims;

        #endregion [ FIM - Propriedades ]

        #region [ Construtores ]

        public UsuarioViewModel()
        {
            this._movimentacoes = new List<MovimentacaoViewModel>();
        }

        public UsuarioViewModel(Guid id, string nome, string cpf, string email, string senha, bool aceitou_termos, DateTime? data_aceitou_termos, DateTime data_criacao, 
            decimal taxa_acima_cdi, bool is_admin, bool trocar_senha, bool ativo) : this()
        {
            this.id = id;
            this.nome = nome;
            this.cpf = cpf;
            this.email = email;
            this.senha = senha;
            this.aceitou_termos = aceitou_termos;
            this.data_aceitou_termos = data_aceitou_termos;
            this.data_criacao = data_criacao;
            this.taxa_acima_cdi = taxa_acima_cdi;
            this.is_admin = is_admin;
            this.trocar_senha = trocar_senha;
            this.ativo = ativo;
        }

        #endregion [ FIM - Construtores ]

        public void SetarListaClaims(List<ClaimViewModel> claims)
        {
            this._claims = claims;
        }

        public void SetarListaMovimentacoes(List<MovimentacaoViewModel> movimentacoes)
        {
            this._movimentacoes = movimentacoes;
        }
    }
}

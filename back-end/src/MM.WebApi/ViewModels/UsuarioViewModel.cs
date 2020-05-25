using MM.WebApi.Helpers;
using System;
using System.Collections.Generic;

namespace MM.WebApi.ViewModels
{
    public class UsuarioViewModel
    {
        #region [ Propriedades ]

        public Guid id                          { get; private set; }
        public string nome                      { get; private set; }
        public string cpf                       { get; private set; }
        public string email                     { get; private set; }
        public string senha                     { get; private set; }
        public bool aceitou_termos              { get; private set; }
        public DateTime? data_aceitou_termos    { get; private set; }
        public DateTime data_criacao            { get; private set; }
        public decimal taxa_acima_cdi           { get; private set; }
        public bool is_admin                    { get; private set; }
        public bool ativo                       { get; private set; }

        public string primeiro_nome             { get { return this.nome.GetFirstName(); } }

        private List<ClaimViewModel> _claims;
        public IReadOnlyCollection<ClaimViewModel> Claims => _claims;

        #endregion [ FIM - Propriedades ]

        #region [ Construtores ]

        public UsuarioViewModel(Guid id, string nome, string cpf, string email, string senha, bool aceitou_termos, DateTime? data_aceitou_termos, DateTime data_criacao, 
            decimal taxa_acima_cdi, bool is_admin, bool ativo)
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
            this.ativo = ativo;
        }

        #endregion [ FIM - Construtores ]

        public void SetarListaClaims(List<ClaimViewModel> claims)
        {
            this._claims = claims;
        }
    }
}

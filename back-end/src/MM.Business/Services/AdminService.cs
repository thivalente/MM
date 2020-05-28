using MM.Business.Interfaces;
using MM.Business.Models;
using MM.Business.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TGV.Framework.Core.Helper;

namespace MM.Business.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IMovimentacaoRepository _movimentacaoRepository;

        public AdminService(IAdminRepository adminRepository, IMovimentacaoRepository movimentacaoRepository)
        {
            this._adminRepository = adminRepository;
            this._movimentacaoRepository = movimentacaoRepository;
        }

        public async Task<Usuario> EfetuarLogin(string email, string senha)
        {
            return await this._adminRepository.EfetuarLogin(email, senha);
        }

        public async Task<List<Usuario>> Listar()
        {
            var usuarios = await this._adminRepository.Listar();
            var movimentacoes = await this._adminRepository.ListarMovimentacoes();

            foreach (var usuario in usuarios)
            {
                var movimentacoesUsuario = movimentacoes.Where(m => m.usuario_id == usuario.id).ToList();

                if (movimentacoesUsuario.Count > 0 && movimentacoesUsuario.All(m => m != null))
                    usuario.SetarListaMovimentacoes(movimentacoesUsuario);
            }

            return usuarios;
        }

        public async Task<Usuario> Obter(Guid usuario_id)
        {
            var usuario = await this._adminRepository.Obter(usuario_id);
            var movimentacoes = await this._adminRepository.ListarMovimentacoes(usuario_id);

            if (movimentacoes.Count > 0 && movimentacoes.All(m => m != null))
                usuario.SetarListaMovimentacoes(movimentacoes);

            return usuario;
        }

        public async Task<Tuple<decimal, decimal>> ObterTaxasAtualizadas()
        {
            var diModel = await this._movimentacaoRepository.ObterTaxaDIDoDia();

            var taxa_mensal_di = diModel.cdi / 12;
            var taxa_mensal_selic = diModel.selic / 12;
            var taxa_mensal_poupanca = diModel.selic_daily > 8.5m ? taxa_mensal_selic * 0.5m : taxa_mensal_selic * 0.7m;

            return new Tuple<decimal, decimal>(taxa_mensal_di, taxa_mensal_poupanca);
        }

        public async Task<bool> Salvar (Usuario usuario)
        {
            if (usuario == null)
                return false;

            if (usuario.id != Guid.Empty)
                return await this._adminRepository.Salvar_Editar(usuario);

            return await this._adminRepository.Salvar_Cadastrar(usuario);
        }
    }
}

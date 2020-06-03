using MM.Business.Interfaces;
using MM.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MM.Business.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IMovimentacaoRepository _movimentacaoRepository;
        private readonly IMovimentacaoService _movimentacaoService;
        private readonly ISendGridEmail _sendGridEmail;

        public AdminService(IMovimentacaoService movimentacaoService, IAdminRepository adminRepository, IMovimentacaoRepository movimentacaoRepository, ISendGridEmail sendGridEmail)
        {
            this._adminRepository = adminRepository;
            this._movimentacaoRepository = movimentacaoRepository;
            this._movimentacaoService = movimentacaoService;
            this._sendGridEmail = sendGridEmail;
        }

        public async Task<Usuario> EfetuarLogin(string email, string senha)
        {
            return await this._adminRepository.EfetuarLogin(email, senha);
        }

        public async Task EnviarEmailContato(string nome, string email, string assunto, string mensagem)
        {
            await this._sendGridEmail.EnviarEmailContato(nome, email, assunto, mensagem);
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

        public async Task RecuperarSenha(string email)
        {
            // Verifica se existe o usuário e se ele está ativo
            var usuario = await this._adminRepository.ObterPorEmail(email);

            // Se não existir, retorna
            if (usuario == null || !usuario.ativo)
                await Task.CompletedTask;

            // Se existir, gera uma nova senha randomica
            var novaSenha = BaseBusiness.RandomString(6);

            // Atualiza o usuário com essa nova senha e marca que ele precisa trocar a senha no primeiro login
            usuario.TrocarSenha(novaSenha);
            await this._adminRepository.Salvar_TrocarSenha(usuario);

            // Envia a senha por e-mail ao usuário
            await this._sendGridEmail.EnviarEmailRecuperarSenha(email, BaseBusiness.GetFirstName(usuario.nome), novaSenha);
        }

        public async Task<bool> Salvar (Usuario usuario)
        {
            if (usuario == null)
                return false;

            if (usuario.id != Guid.Empty)
                return await this._adminRepository.Salvar_Editar(usuario);

            return await this._adminRepository.Salvar_Cadastrar(usuario);
        }

        public async Task<bool> SalvarMovimentacao(Movimentacao movimentacao)
        {
            if (movimentacao == null)
                return false;

            // Verifica se a data de criação é maior do que hoje ou menor do que a menor taxa di do sistema
            DateTime menorDataTaxaDI = await this._movimentacaoService.ObterMenorDataTaxaDI();

            if (movimentacao.data_criacao.Date > DateTime.Today || movimentacao.data_criacao.Date < menorDataTaxaDI.Date)
                return false;

            // Grava os dados da movimentação
            if (movimentacao.id != Guid.Empty)
                await this._adminRepository.SalvarMovimentacao_Editar(movimentacao);
            else
                await this._adminRepository.SalvarMovimentacao_Cadastrar(movimentacao);

            // Apaga o rendimento diário a partir da data desta
            await this._movimentacaoRepository.ApagarRendimentoDiario(movimentacao.usuario_id, movimentacao.data_criacao);

            // Atualiza os dados de movimentação diária
            await this._movimentacaoService.AtualizarMovimentacoesUsuario(movimentacao.usuario_id);

            return true;
        }

        public async Task<bool> TrocarSenha(string email, string senhaAtual, string novaSenha)
        {
            // Valida se os dados do usuário atual e senha estão corretos
            var usuario = await this.EfetuarLogin(email, senhaAtual);

            if (usuario == null)
                return false;

            usuario.TrocarSenha(novaSenha, false);
            await this._adminRepository.Salvar_TrocarSenha(usuario);
            return true;
        }
    }
}

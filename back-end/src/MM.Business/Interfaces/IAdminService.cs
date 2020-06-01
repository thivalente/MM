using MM.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MM.Business.Interfaces
{
    public interface IAdminService
    {
        Task EnviarEmailContato(string nome, string email, string assunto, string mensagem);
        Task<Usuario> EfetuarLogin(string email, string senha);
        Task<List<Usuario>> Listar();
        Task<Usuario> Obter(Guid usuario_id);
        Task<Tuple<decimal, decimal>> ObterTaxasAtualizadas();
        Task RecuperarSenha(string email);
        Task<bool> Salvar(Usuario usuario);
        Task<bool> SalvarMovimentacao(Movimentacao movimentacao);
        Task<bool> TrocarSenha(string email, string senhaAtual, string novaSenha);
    }
}

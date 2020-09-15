using System.Threading.Tasks;

namespace MM.Business.Interfaces
{
    public interface IHostAzulEmail
    {
        Task EnviarEmailCadastroUsuario(string emailTo, string nome, string senha);
        Task EnviarEmailContato(string emailFrom, string nome, string assunto, string mensagem);
        Task EnviarEmailRecuperarSenha(string emailTo, string nome, string senha);
    }
}

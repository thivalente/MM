using System.Threading.Tasks;

namespace MM.Business.Interfaces
{
    public interface ISendGridEmail
    {
        Task EnviarEmailContato(string emailFrom, string nome, string assunto, string mensagem);
        Task EnviarEmailRecuperarSenha(string emailTo, string nome, string senha);
    }
}

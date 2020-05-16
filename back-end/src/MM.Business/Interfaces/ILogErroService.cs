using MM.Business.Models;
using System.Threading.Tasks;

namespace MM.Business.Interfaces
{
    public interface ILogErroService
    {
        Task Salvar(LogErro logErro);
    }
}

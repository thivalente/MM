using MM.Business.Models;
using System.Threading.Tasks;

namespace MM.Business.Interfaces
{
    public interface ILogErroRepository
    {
        Task Salvar(LogErro logErro);
    }
}

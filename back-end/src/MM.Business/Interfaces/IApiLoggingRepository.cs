using MM.Business.Models;
using System.Threading.Tasks;

namespace MM.Business.Interfaces
{
    public interface IApiLoggingRepository
    {
        Task Incluir(LogApi logApi);
    }
}

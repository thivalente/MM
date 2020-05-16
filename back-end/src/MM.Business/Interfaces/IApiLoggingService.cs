using MM.Business.Models;
using System.Threading.Tasks;

namespace MM.Business.Interfaces
{
    public interface IApiLoggingService
    {
        Task Incluir(LogApi logApi);
    }
}

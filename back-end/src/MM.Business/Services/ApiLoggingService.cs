using MM.Business.Interfaces;
using MM.Business.Models;
using System.Threading.Tasks;

namespace MM.Business.Services
{
    public class ApiLoggingService : IApiLoggingService
    {
        private readonly IApiLoggingRepository _repository;

        public ApiLoggingService(IApiLoggingRepository repository)
        {
            this._repository = repository;
        }

        public Task Incluir(LogApi logApi)
        {
            return this._repository.Incluir(logApi);
        }
    }
}

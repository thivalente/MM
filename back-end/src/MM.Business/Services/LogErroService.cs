using MM.Business.Interfaces;
using MM.Business.Models;
using System.Threading.Tasks;

namespace MM.Business.Services
{
    public class LogErroService : ILogErroService
    {
        private readonly ILogErroRepository _logErroRepository;

        public LogErroService(ILogErroRepository logErroRepository)
        {
            this._logErroRepository = logErroRepository;
        }

        public Task Salvar(LogErro logErro)
        {
            return this._logErroRepository.Salvar(logErro);
        }
    }
}

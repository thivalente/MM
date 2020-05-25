using System;
using System.Threading.Tasks;

namespace MM.Business.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<bool> AceitarTermos(Guid usuario_id, DateTime data_aceitacao);
    }
}

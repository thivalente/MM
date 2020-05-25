using System;
using System.Threading.Tasks;

namespace MM.Business.Interfaces
{
    public interface IUsuarioService
    {
        Task<bool> AceitarTermos(Guid usuario_id);
    }
}

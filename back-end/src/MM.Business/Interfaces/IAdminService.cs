using MM.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MM.Business.Interfaces
{
    public interface IAdminService
    {
        Task<Usuario> EfetuarLogin(string email, string senha);
        Task<List<Usuario>> Listar();
        Task<Usuario> Obter(Guid usuario_id);
    }
}

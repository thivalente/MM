using MM.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MM.Business.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            this._usuarioRepository = usuarioRepository;
        }

        public Task<bool> AceitarTermos(Guid usuario_id)
        {
            return this._usuarioRepository.AceitarTermos(usuario_id, DateTime.Now);
        }
    }
}

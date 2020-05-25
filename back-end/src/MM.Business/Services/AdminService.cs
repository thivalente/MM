using MM.Business.Interfaces;
using MM.Business.Models;
using MM.Business.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TGV.Framework.Core.Helper;

namespace MM.Business.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            this._adminRepository = adminRepository;
        }

        public async Task<Usuario> EfetuarLogin(string email, string senha)
        {
            return await this._adminRepository.EfetuarLogin(email, senha);
        }

        public async Task<List<Usuario>> Listar()
        {
            return await this._adminRepository.Listar();
        }

        public async Task<Usuario> Obter(Guid usuario_id)
        {
            return await this._adminRepository.Obter(usuario_id);
        }
    }
}

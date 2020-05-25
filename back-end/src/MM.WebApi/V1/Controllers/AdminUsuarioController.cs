using Microsoft.AspNetCore.Mvc;
using MM.Business.Interfaces;
using MM.WebApi.Helpers;
using MM.WebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MM.WebApi.V1.Controllers
{
    //[Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/admin")]
    public class AdminUsuarioController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminUsuarioController(IAdminService adminService)
        {
            this._adminService = adminService;
        }

        [Route("usuario/{usuario_id:guid}")]
        [HttpGet()]
        public async Task<UsuarioViewModel> Obter(Guid usuario_id)
        {
            // Busca o usuário
            var usuario = await this._adminService.Obter(usuario_id);

            return usuario.ToUsuarioViewModel();
        }

        [Route("usuario")]
        [HttpGet()]
        public async Task<IEnumerable<UsuarioViewModel>> Listar()
        {
            // Monta a lista 
            var lista = await this._adminService.Listar();

            return lista.ConvertAll(u => u.ToUsuarioViewModel());
        }
    }
}

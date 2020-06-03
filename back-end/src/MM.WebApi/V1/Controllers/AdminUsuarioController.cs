using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MM.Business.Interfaces;
using MM.WebApi.Controllers;
using MM.WebApi.Helpers;
using MM.WebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MM.WebApi.V1.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/admin")]
    public class AdminUsuarioController : MainController
    {
        private readonly IAdminService _adminService;

        public AdminUsuarioController(IAdminService adminService, INotificador notificador) : base(notificador)
        {
            this._adminService = adminService;
        }

        [Route("usuario")]
        [HttpGet()]
        public async Task<IEnumerable<UsuarioViewModel>> Listar()
        {
            // Monta a lista 
            var lista = await this._adminService.Listar();

            return lista.ConvertAll(u => u.ToUsuarioViewModel());
        }

        [Route("usuario/{usuario_id}")]
        [HttpGet()]
        public async Task<UsuarioViewModel> Obter(string usuario_id)
        {
            // Transforma string em guid
            Guid.TryParse(usuario_id, out Guid gUsuario_id);

            // Busca o usuário
            var usuario = await this._adminService.Obter(gUsuario_id);

            return usuario.ToUsuarioViewModel();
        }

        [HttpPost("usuario/salvar")]
        public async Task<ActionResult> Salvar(UsuarioViewModel usuario)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await this._adminService.Salvar(usuario.ToUsuarioModel());

            if (result)
                return CustomResponse();

            NotificarErro("Houve um erro ao tentar salvar este usuário");
            return CustomResponse();
        }

        [HttpPost("usuario/salvarmovimentacao")]
        public async Task<ActionResult> SalvarMovimentacao(MovimentacaoViewModel movimentacao)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await this._adminService.SalvarMovimentacao(movimentacao.ToMovimentacaoModel());

            if (result)
                return CustomResponse();

            NotificarErro("Houve um erro ao tentar salvar este usuário");
            return CustomResponse();
        }
    }
}

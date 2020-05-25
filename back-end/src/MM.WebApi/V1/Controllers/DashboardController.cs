using Microsoft.AspNetCore.Mvc;
using MM.Business.Interfaces;
using MM.WebApi.Controllers;
using MM.WebApi.Helpers;
using MM.WebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace MM.WebApi.V1.Controllers
{
    //[Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}")]
    public class DashboardController : MainController
    {
        private readonly IMovimentacaoService _movimentacaoService;
        private readonly IUsuarioService _usuarioService;

        public DashboardController(IMovimentacaoService movimentacaoService, IUsuarioService usuarioService, INotificador notificador) : base(notificador)
        {
            this._movimentacaoService = movimentacaoService;
            this._usuarioService = usuarioService;
        }

        [Route("aceitartermos")]
        [HttpPut()]
        public async Task<ActionResult> AceitarTermos(AceitouTermosViewModel aceitouTermos)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await this._usuarioService.AceitarTermos(aceitouTermos.usuario_id);

            if (result)
                return CustomResponse();

            NotificarErro("Ocorreu um erro ao aceitar os termos de uso");
            return CustomResponse();
        }

        [Route("dashboard/{usuario_id:guid}")]
        [HttpGet()]
        public async Task<IEnumerable<MovimentacaoDiariaViewModel>> Obter(Guid usuario_id)
        {
            // Atualiza a taxa DI (verifica se é necessário e chama a api)
            await this._movimentacaoService.AtualizarTaxaDI();

            // Atualiza os dados do usuário
            await this._movimentacaoService.AtualizarMovimentacoesUsuario(usuario_id);

            // Monta a lista 
            var lista = await this._movimentacaoService.Obter(usuario_id);

            return lista.ToDashboardViewModel();
        }
    }
}
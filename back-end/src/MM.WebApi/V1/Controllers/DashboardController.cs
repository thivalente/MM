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
    [Route("api/v{version:apiVersion}/dashboard")]
    public class DashboardController : Controller
    {
        private readonly IMovimentacaoService _movimentacaoService;

        public DashboardController(IMovimentacaoService movimentacaoService)
        {
            this._movimentacaoService = movimentacaoService;
        }

        [HttpGet("{usuario_id:guid}")]
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
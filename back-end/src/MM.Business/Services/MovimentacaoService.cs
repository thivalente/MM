using MM.Business.Interfaces;
using MM.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MM.Business.Services
{
    public class MovimentacaoService : IMovimentacaoService
    {
        private readonly IMovimentacaoRepository _movimentacaoRepository;

        public MovimentacaoService(IMovimentacaoRepository movimentacaoRepository)
        {
            this._movimentacaoRepository = movimentacaoRepository;
        }

        public async Task<List<MovimentacaoDiaria>> Obter(Guid usuario_id)
        {
            return await this._movimentacaoRepository.Obter(usuario_id);
        }
    }
}

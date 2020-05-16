using MM.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MM.Business.Interfaces
{
    public interface IMovimentacaoRepository
    {
        Task AtualizarTaxaDI(decimal taxa_di, decimal taxa_poupanca, List<DateTime> diasUteis);
        Task<List<TaxaDiaria>> ListarTaxasDesatualizadas(Guid usuario_id);
        Task<List<MovimentacaoDiaria>> Obter(Guid usuario_id);
        Task<dynamic> ObterSaldoETaxaUsuario(Guid usuario_id);
        Task<DIFromExternalAPI> ObterTaxaDIDoDia();
        Task<TaxaDiaria> ObterUltimaTaxaDI();
        Task SalvarMovimentacoes(List<MovimentacaoDiaria> movimentacoes);
    }
}

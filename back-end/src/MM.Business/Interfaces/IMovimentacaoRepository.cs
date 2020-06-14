using MM.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MM.Business.Interfaces
{
    public interface IMovimentacaoRepository
    {
        Task ApagarRendimentoDiario(Guid usuario_id);
        Task AtualizarTaxaDI(decimal taxa_di, decimal taxa_poupanca, List<DateTime> diasUteis);
        Task<List<Movimentacao>> ListarMovimentacoes(Guid? usuario_id = null);
        Task<List<TaxaDiaria>> ListarTaxasDesatualizadas(Guid usuario_id, bool recriar);
        Task<List<MovimentacaoDiaria>> Obter(Guid usuario_id);
        Task<DateTime> ObterMenorDataTaxaDI();
        Task<dynamic> ObterSaldoETaxaUsuario(Guid usuario_id, bool recriar);
        Task<DIFromExternalAPI> ObterTaxaDIDoDia();
        Task<TaxaDiaria> ObterUltimaTaxaDI();
        Task SalvarMovimentacoes(List<MovimentacaoDiaria> movimentacoes);
    }
}

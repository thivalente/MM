using MM.Business.Interfaces;
using MM.Business.Models;
using MM.Business.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task AtualizarMovimentacoesUsuario(Guid usuario_id)
        {
            // Lista as datas que estão desatualizadas deste usuário
            List<TaxaDiaria> taxasDesatualizadas = await this._movimentacaoRepository.ListarTaxasDesatualizadas(usuario_id);

            if (taxasDesatualizadas.Count == 0)
                return;

            var usuarioInfo = await this._movimentacaoRepository.ObterSaldoETaxaUsuario(usuario_id);
            decimal saldoAtualUsuario = usuarioInfo.Saldo;
            decimal taxaUsuario = usuarioInfo.Taxa;
            List<MovimentacaoDiaria> movimentacoes = new List<MovimentacaoDiaria>();

            foreach (var taxaDesatualizada in taxasDesatualizadas)
            {
                var valor = (saldoAtualUsuario * taxaUsuario * taxaDesatualizada.taxa_di) / 100;
                var valor_di = (saldoAtualUsuario * taxaDesatualizada.taxa_di) / 100;
                var valor_poupanca = (saldoAtualUsuario * taxaDesatualizada.taxa_poupanca) / 100;

                movimentacoes.Add(new MovimentacaoDiaria(Guid.NewGuid(), usuario_id, taxaDesatualizada.data_criacao, true, true, valor, valor_di, valor_poupanca));

                saldoAtualUsuario += valor;
            }

            await this._movimentacaoRepository.SalvarMovimentacoes(movimentacoes);
        }

        public async Task AtualizarTaxaDI()
        {
            // Busca a última atualização da taxa di
            var ultima_taxa_di = await this._movimentacaoRepository.ObterUltimaTaxaDI();
            var hoje = DateTime.Today;

            List<DateTime> workingDays = FeriadosHelper.GetWorkingDaysBetweenTwoDates(ultima_taxa_di.data_criacao.AddDays(1), hoje).ToList();

            // Se a última data for igual a hoje ou não tiver dias úteis entre a última data e hoje, não precisa atualizar
            if (ultima_taxa_di.data_criacao.Date >= hoje || workingDays.Count == 0)
                return;

            int totalDiasUteisAno = FeriadosHelper.GetTotalWorkingDaysInAYear(hoje.Year);

            var diModel = await this._movimentacaoRepository.ObterTaxaDIDoDia();
            var taxa_diaria_di = diModel.cdi_daily / totalDiasUteisAno;
            var taxa_diaria_selic = diModel.selic_daily / totalDiasUteisAno;
            var taxa_diaria_poupanca = diModel.selic_daily > 8.5m ? taxa_diaria_selic * 0.5m : taxa_diaria_selic * 0.7m;

            // Se não foi, atualiza
            await this._movimentacaoRepository.AtualizarTaxaDI(taxa_diaria_di, taxa_diaria_poupanca, workingDays);
        }

        public async Task<List<MovimentacaoDiaria>> Obter(Guid usuario_id)
        {
            return await this._movimentacaoRepository.Obter(usuario_id);
        }
    }
}

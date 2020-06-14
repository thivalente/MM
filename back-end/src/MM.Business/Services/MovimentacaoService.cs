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

        private async Task AtualizarMovimentacoesUsuario(Guid usuario_id, List<TaxaDiaria> taxasDesatualizadas, bool recriar)
        {
            if (taxasDesatualizadas.Count == 0)
                return;

            List<Movimentacao> movimentacoesUsuario = await this._movimentacaoRepository.ListarMovimentacoes(usuario_id);

            var usuarioInfo = await this._movimentacaoRepository.ObterSaldoETaxaUsuario(usuario_id, recriar);
            decimal saldoAtualUsuario = usuarioInfo.Saldo;
            decimal taxaUsuario = usuarioInfo.Taxa;
            List<MovimentacaoDiaria> movimentacoesDiarias = new List<MovimentacaoDiaria>();

            // Apaga todas as movimentações anteriores a primeira taxa desatualizada
            movimentacoesUsuario.RemoveAll(m => taxasDesatualizadas.First().data_criacao.Date >= m.data_criacao.Date);

            foreach (var taxaDesatualizada in taxasDesatualizadas)
            {
                var valor = (saldoAtualUsuario * taxaUsuario * taxaDesatualizada.taxa_di) / 100;
                var valor_di = (saldoAtualUsuario * taxaDesatualizada.taxa_di) / 100;
                var valor_poupanca = (saldoAtualUsuario * taxaDesatualizada.taxa_poupanca) / 100;

                movimentacoesDiarias.Add(new MovimentacaoDiaria(Guid.NewGuid(), usuario_id, taxaDesatualizada.data_criacao, true, true, valor, valor_di, valor_poupanca));

                saldoAtualUsuario += valor;

                if (movimentacoesUsuario.Any(m => taxaDesatualizada.data_criacao.Date >= m.data_criacao.Date)) // Se a data da taxa for maior ou igual a de alguma movimentação, adiciona o saldo
                {
                    // Pega a movimentação que for igual ou mais antiga do que a taxa
                    var movimentacaoUsuario = movimentacoesUsuario.OrderBy(m => m.data_criacao).First(m => m.data_criacao.Date <= taxaDesatualizada.data_criacao.Date);
                    var valorMovimentacao = movimentacaoUsuario.valor;

                    movimentacoesUsuario.Remove(movimentacaoUsuario); // Apaga a movimentação, pois já foi tratada
                    saldoAtualUsuario += valorMovimentacao; // Adiciona o valor ao saldo do cliente
                }
            }

            await this._movimentacaoRepository.SalvarMovimentacoes(movimentacoesDiarias);
        }


        public async Task AtualizarMovimentacoesUsuario(Guid usuario_id)
        {
            // Lista as datas que estão desatualizadas deste usuário
            List<TaxaDiaria> taxasDesatualizadas = await this._movimentacaoRepository.ListarTaxasDesatualizadas(usuario_id, false);

            await AtualizarMovimentacoesUsuario(usuario_id, taxasDesatualizadas, false);
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

            // Remove da lista de dias úteis, os dias em que ainda não foi atualizada a taxa di
            workingDays.RemoveAll(wd => wd.Date > diModel.creation_date.Date);

            if (workingDays.Count == 0)
                return;

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

        public async Task<DateTime> ObterMenorDataTaxaDI()
        {
            return await this._movimentacaoRepository.ObterMenorDataTaxaDI();
        }

        public async Task RecriarMovimentacoesUsuario(Guid usuario_id)
        {
            // Lista as datas que estão desatualizadas deste usuário
            List<TaxaDiaria> taxasDesatualizadas = await this._movimentacaoRepository.ListarTaxasDesatualizadas(usuario_id, true);

            await AtualizarMovimentacoesUsuario(usuario_id, taxasDesatualizadas, true);
        }
    }
}

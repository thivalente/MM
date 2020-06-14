using MM.Business.Interfaces;
using MM.Business.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Mime;
using System.Dynamic;
using Newtonsoft.Json;

namespace MM.Data.Repositories
{
    public class MovimentacaoRepository : BaseRepository, IMovimentacaoRepository
    {
        public MovimentacaoRepository(IConfiguration configuration) : base(configuration) { }


        public Task ApagarRendimentoDiario(Guid usuario_id)
        {
            using (var db = new SqlConnection(this.ConnectionString))
            {
                db.Execute("DELETE FROM movimentacao_diaria WHERE rendimento = 1 AND usuario_id = @usuario_id;", new { usuario_id });
            }

            return Task.CompletedTask;
        }

        public Task AtualizarTaxaDI(decimal taxa_di, decimal taxa_poupanca, List<DateTime> diasUteis)
        {
            using (var db = new SqlConnection(this.ConnectionString))
            {
                db.Open();

                using (var tran = db.BeginTransaction())
                {
                    foreach (var diaUtil in diasUteis)
                    {
                        db.Execute("INSERT INTO taxa_diaria (data_criacao, taxa_di, taxa_poupanca, ativo) VALUES (@data_criacao, @taxa_di, @taxa_poupanca, 1);",
                            new { data_criacao = diaUtil.Date, taxa_di, taxa_poupanca }, tran);
                    }

                    tran.Commit();
                }
            }

            return Task.CompletedTask;
        }

        public async Task<List<Movimentacao>> ListarMovimentacoes(Guid? usuario_id = null)
        {
            using (var db = new SqlConnection(this.ConnectionString))
            {
                var lista = (db.Query<Movimentacao>(
                    @" 
                        SELECT	DISTINCT
                                id,
		                        usuario_id,
                                valor,
                                data_criacao,
                                entrada,
                                ativo
                        FROM	movimentacao m
                        WHERE   (@usuario_id IS NULL OR m.usuario_id = @usuario_id)
                        ORDER BY m.usuario_id, m.data_criacao DESC;
                    ", new { usuario_id })).ToList();

                return await Task.FromResult(lista);
            }
        }

        public async Task<List<TaxaDiaria>> ListarTaxasDesatualizadas(Guid usuario_id, bool recriar)
        {
            using (var db = new SqlConnection(this.ConnectionString))
            {
                var query = recriar ?
                    @"
                        SELECT	DISTINCT
		                        td.data_criacao,
		                        td.taxa_di,
		                        td.taxa_poupanca,
		                        td.ativo
                        FROM	taxa_diaria td
                        WHERE	td.data_criacao > (SELECT TOP 1 md.data_criacao FROM movimentacao_diaria md WHERE md.usuario_id = @usuario_id ORDER BY md.data_criacao);
                    " :
                    @" 
                        SELECT	DISTINCT
		                        td.data_criacao,
		                        td.taxa_di,
		                        td.taxa_poupanca,
		                        td.ativo
                        FROM	taxa_diaria td
                        WHERE	NOT EXISTS (
                            SELECT	1
                            FROM	movimentacao_diaria md
                            WHERE	(md.usuario_id = @usuario_id
                            AND		td.data_criacao <= md.data_criacao)
                            OR		0 = (SELECT COUNT(md.id) FROM movimentacao_diaria md WHERE md.usuario_id = @usuario_id))
                        ORDER BY td.data_criacao;
                    ";

                var lista = (db.Query<TaxaDiaria>(query, new { usuario_id })).ToList();

                return await Task.FromResult(lista);
            }
        }

        public async Task<List<MovimentacaoDiaria>> Obter(Guid usuario_id)
        {
            using (var db = new SqlConnection(this.ConnectionString))
            {
                var lista = (db.Query<MovimentacaoDiaria>(
                    @" 
                        SELECT	md.id,
		                        md.usuario_id,
		                        md.valor,
		                        md.valor_di,
		                        md.valor_poupanca,
		                        md.data_criacao,
		                        md.entrada,
		                        md.rendimento,
		                        md.ativo
                        FROM	movimentacao_diaria md
                        WHERE	md.usuario_id = @usuario_id
                        ORDER BY md.data_criacao DESC, md.rendimento DESC
                    ", new { usuario_id })).ToList();

                return await Task.FromResult(lista);
            }
        }

        public async Task<DateTime> ObterMenorDataTaxaDI()
        {
            using (var db = new SqlConnection(this.ConnectionString))
            {
                var taxa = (db.Query<DateTime>(@"SELECT TOP 1 td.data_criacao FROM taxa_diaria td ORDER BY td.data_criacao")).FirstOrDefault();

                return await Task.FromResult(taxa);
            }
        }

        public async Task<dynamic> ObterSaldoETaxaUsuario(Guid usuario_id, bool recriar)
        {
            dynamic result = new ExpandoObject();
            result.Saldo = 0;
            result.Taxa = 0;

            using (var db = new SqlConnection(this.ConnectionString))
            {
                var query = recriar ?
                    @"
                        SELECT	TOP 1
		                        m.valor AS Saldo,
		                        u.taxa_acima_cdi AS Taxa
                        FROM	movimentacao m
                        INNER JOIN usuario u ON m.usuario_id = u.id
                        WHERE	u.id = @usuario_id
                        ORDER BY m.data_criacao;
                        " :
                    @" 
                        SELECT	SUM(valor) AS Saldo,
		                        u.taxa_acima_cdi AS Taxa
                        FROM	movimentacao_diaria md
                        INNER JOIN usuario u ON md.usuario_id = u.id
                        WHERE	md.usuario_id = @usuario_id
                        GROUP BY u.taxa_acima_cdi;
                    ";

                var item = (db.Query<dynamic>(query, new { usuario_id })).FirstOrDefault();

                if (item == null)
                    return result;

                result.Saldo = item.Saldo;
                result.Taxa = item.Taxa;

                return await Task.FromResult(result);
            }
        }

        public async Task<DIFromExternalAPI> ObterTaxaDIDoDia()
        {
            var httpClient = HttpClientFactory.Create();
            var url = "https://api.hgbrasil.com/finance/taxes?key=0cfb754b";
            var response = await httpClient.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = response.Content;
                var data = await content.ReadAsStringAsync();

                if (data == null)
                    return null;

                DIFromExternalAPI_Estrutura jsonData = JsonConvert.DeserializeObject<DIFromExternalAPI_Estrutura>(data);
                var di = jsonData.results.First();

                return new DIFromExternalAPI(di.date, di.cdi, di.selic, di.daily_factor, di.selic_daily, di.cdi_daily);
            }

            return null;
        }

        public async Task<TaxaDiaria> ObterUltimaTaxaDI()
        {
            using (var db = new SqlConnection(this.ConnectionString))
            {
                var taxa = (db.Query<TaxaDiaria>(
                    @" 
                        SELECT	TOP 1
                                td.data_criacao,
                                td.taxa_di,
                                td.taxa_poupanca,
		                        td.ativo
                        FROM	taxa_diaria td
                        ORDER BY td.data_criacao DESC
                    ")).FirstOrDefault();

                return await Task.FromResult(taxa);
            }
        }

        public async Task<bool> PrecisaAtualizarUsuario(Guid usuario_id)
        {
            using (var db = new SqlConnection(this.ConnectionString))
            {
                var atualizado = (db.Query<DateTime>(
                    @" 
                        SELECT	TOP 1
                                td.data_criacao
                        FROM	taxa_diaria td
                        INNER JOIN movimentacao_diaria md ON td.data_criacao = md.data_criacao
                        WHERE	md.usuario_id = '13226661-4927-46F3-969A-2A3919183747'
                    ", new { usuario_id })).FirstOrDefault();

                return await Task.FromResult(atualizado == null);
            }
        }

        public Task SalvarMovimentacao(MovimentacaoDiaria movimentacao)
        {
            return SalvarMovimentacoes(new List<MovimentacaoDiaria>() { movimentacao });
        }

        public Task SalvarMovimentacoes(List<MovimentacaoDiaria> movimentacoes)
        {
            using (var db = new SqlConnection(this.ConnectionString))
            {
                db.Open();

                using (var tran = db.BeginTransaction())
                {
                    foreach (var movimentacao in movimentacoes)
                    {
                        db.Execute(@"
                            INSERT INTO movimentacao_diaria (id, usuario_id, valor, valor_di, valor_poupanca, data_criacao, entrada, rendimento, ativo)
                            VALUES (@id, @usuario_id, @valor, @valor_di, @valor_poupanca, @data_criacao, @entrada, @rendimento, 1);",
                            new { movimentacao.id, movimentacao.usuario_id, movimentacao.valor, movimentacao.valor_di, movimentacao.valor_poupanca, movimentacao.data_criacao, 
                            movimentacao.entrada, movimentacao.rendimento}, tran);
                    }

                    tran.Commit();
                }
            }

            return Task.CompletedTask;
        }
    }
}

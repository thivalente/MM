using MM.Business.Interfaces;
using MM.Business.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace MM.Data.Repositories
{
    public class MovimentacaoRepository : BaseRepository, IMovimentacaoRepository
    {
        public MovimentacaoRepository(IConfiguration configuration) : base(configuration) { }

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
    }
}

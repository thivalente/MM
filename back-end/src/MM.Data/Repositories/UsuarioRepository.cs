using Dapper;
using Microsoft.Extensions.Configuration;
using MM.Business.Interfaces;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MM.Data.Repositories
{
    public class UsuarioRepository : BaseRepository, IUsuarioRepository
    {
        public UsuarioRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<bool> AceitarTermos(Guid usuario_id, DateTime data_aceitacao)
        {
            using (var db = new SqlConnection(this.ConnectionString))
            {
                db.Open();
                db.Execute("UPDATE usuario SET aceitou_termos = 1, data_aceitou_termos = @data_aceitacao WHERE id = @usuario_id;", new { usuario_id, data_aceitacao });
            }

            return await Task.FromResult(true);
        }
    }
}

using Dapper;
using Microsoft.Extensions.Configuration;
using MM.Business.Interfaces;
using MM.Business.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TGV.Framework.Core.Helper;

namespace MM.Data.Repositories
{
    public class AdminRepository : BaseRepository, IAdminRepository
    {
        public AdminRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<Usuario> EfetuarLogin(string email, string senha)
        {
            var senhaCripto = senha.Criptografar(BaseRepository.ParametroSistema);

            using (var db = new SqlConnection(this.ConnectionString))
            {
                var usuario = (db.Query<Usuario>(
                    @" 
                        SELECT	DISTINCT
                                id,
		                        nome,
		                        cpf,
		                        email,
		                        senha,
		                        taxa_acima_cdi,
		                        data_criacao,
		                        data_aceitou_termos,
		                        aceitou_termos,
		                        is_admin,
		                        ativo
                        FROM	usuario u
                        WHERE   u.email = @email
                        AND     u.senha = @senhaCripto;
                    ", new { email, senhaCripto })).FirstOrDefault();

                return await Task.FromResult(usuario);
            }
        }

        public async Task<List<Usuario>> Listar()
        {
            using (var db = new SqlConnection(this.ConnectionString))
            {
                var lista = (db.Query<Usuario>(
                    @" 
                        SELECT	DISTINCT
                                id,
		                        nome,
		                        cpf,
		                        email,
		                        senha,
		                        taxa_acima_cdi,
		                        data_criacao,
		                        data_aceitou_termos,
		                        aceitou_termos,
		                        is_admin,
		                        ativo
                        FROM	usuario u
                        ORDER BY u.nome;
                    ")).ToList();

                return await Task.FromResult(lista);
            }
        }

        public async Task<Usuario> Obter(Guid usuario_id)
        {
            using (var db = new SqlConnection(this.ConnectionString))
            {
                var usuario = (db.Query<Usuario>(
                    @" 
                        SELECT	DISTINCT
                                id,
		                        nome,
		                        cpf,
		                        email,
		                        senha,
		                        taxa_acima_cdi,
		                        data_criacao,
		                        data_aceitou_termos,
		                        aceitou_termos,
		                        is_admin,
		                        ativo
                        FROM	usuario u
                        WHERE   u.id = @usuario_id;
                    ", new { usuario_id })).FirstOrDefault();

                return await Task.FromResult(usuario);
            }
        }
    }
}

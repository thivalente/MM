using Dapper;
using Microsoft.Extensions.Configuration;
using MM.Business.Interfaces;
using MM.Business.Models;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MM.Data.Repositories
{
    public class LogErroRepository : BaseRepository, ILogErroRepository
    {
        public LogErroRepository(IConfiguration configuration) : base(configuration) { }

        public Task Salvar(LogErro logErro)
        {
            using (var db = new SqlConnection(this.ConnectionString))
            {
                db.Open();

                db.Execute(@"
                    INSERT INTO log_erro (id, data_criacao, endpoint, mensagem_erro, inner_exception, stack_trace)
                    VALUES (@id, @data_criacao, @endpoint, @mensagem_erro, @inner_exception, @stack_trace);",
                    new { logErro.id, logErro.data_criacao, logErro.endpoint, logErro.mensagem_erro, logErro.inner_exception, logErro.stack_trace });
            }

            return Task.CompletedTask;
        }
    }
}

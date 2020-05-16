using Dapper;
using Microsoft.Extensions.Configuration;
using MM.Business.Interfaces;
using MM.Business.Models;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MM.Data.Repositories
{
    public class ApiLoggingRepository : BaseRepository, IApiLoggingRepository
    {
        public ApiLoggingRepository(IConfiguration configuration) : base(configuration) { }

        public Task Incluir(LogApi logApi)
        {
            using (var db = new SqlConnection(this.ConnectionString))
            {
                db.Open();

                db.Execute(@"
                    INSERT INTO log_api (id, usuario_id, request_time, response_millis, status_code, method, path, query_string, request_body, response_body)
                    VALUES (@id, @usuario_id, @request_time, @response_millis, @status_code, @method, @path, @query_string, @request_body, @response_body);",
                    new
                    {
                        logApi.id,
                        logApi.usuario_id,
                        logApi.request_time,
                        logApi.response_millis,
                        logApi.status_code,
                        logApi.method,
                        logApi.path,
                        logApi.query_string,
                        logApi.request_body,
                        logApi.response_body
                    });
            }

            return Task.CompletedTask;
        }
    }
}

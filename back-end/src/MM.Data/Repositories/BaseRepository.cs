using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MM.Data.Settings;
using System;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using TGV.Framework.Core.Helper;

namespace MM.Data.Repositories
{
    public class BaseRepository
    {
        #region [ Propriedades ]

        protected readonly string ConnectionString;
        protected readonly IConfiguration _config;

        internal const string ParametroSistema = "MM1nv3st";

        #endregion [ FIM - Propriedades ]

        public BaseRepository(IConfiguration configuration)
        {
            this._config = configuration;
            ConnectionString = GetSqlConnectionStringDapper();
        }

        private string GetSqlConnectionStringDapper()
        {
            // Verifica se está criptografada
            bool isEncripted = _config.GetSection("Criptografada").Exists() ? _config.GetSection("Criptografada").Value.Equals("true", StringComparison.InvariantCultureIgnoreCase) : false;

            string connStringCripto = _config.GetConnectionString("MMConnString");
            string connString = isEncripted ? connStringCripto.Descriptografar(ParametroSistema) : connStringCripto;

            if (!String.IsNullOrEmpty(connString))
            {
                EntityConnectionStringBuilder builderEF = new EntityConnectionStringBuilder(connString);
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(builderEF.ProviderConnectionString);

                return GetSqlConnectionStringDapper(builder.DataSource, builder.InitialCatalog, builder.UserID, builder.Password);
            }

            var dbSettings = Options.Create<DataBaseSettings>(_config.GetSection("DataBaseSettings").Get<DataBaseSettings>());

            string serverName = isEncripted ? dbSettings.Value.DB_ServerName.Descriptografar(ParametroSistema) : dbSettings.Value.DB_ServerName; ;
            string databaseName = isEncripted ? dbSettings.Value.DB_DatabaseName.Descriptografar(ParametroSistema) : dbSettings.Value.DB_DatabaseName;
            string userId = isEncripted ? dbSettings.Value.DB_UserId.Descriptografar(ParametroSistema) : dbSettings.Value.DB_UserId;
            string password = isEncripted ? dbSettings.Value.DB_Password.Descriptografar(ParametroSistema) : dbSettings.Value.DB_Password;

            return GetSqlConnectionStringDapper(serverName, databaseName, userId, password);
        }

        private static string GetSqlConnectionStringDapper(string serverName, string databaseName, string userId, string password)
        {
            if (!String.IsNullOrEmpty(serverName) && !String.IsNullOrEmpty(databaseName) && !String.IsNullOrEmpty(userId) && !String.IsNullOrEmpty(password))
                return $"data source={serverName};initial catalog={databaseName};persist security info=True;user id={userId};password={password};MultipleActiveResultSets=True;";

            return String.Empty;
        }
    }
}

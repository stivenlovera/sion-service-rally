
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using service_rally_diciembre_2023.Utils;
namespace service_rally_diciembre_2023.Context
{
    public class DBComisionesContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public DBComisionesContext(IConfiguration configuration)
        {
            _configuration = configuration;
            //Cadena de conexion
            var getStringConnectionGuardian = _configuration.GetSection("connectionSqlServerDBComisiones").Get<ConectionString>();
            _connectionString = $"Server={getStringConnectionGuardian.IpServer};Database={getStringConnectionGuardian.Database};User={getStringConnectionGuardian.User};Password={getStringConnectionGuardian.Password};TrustServerCertificate=True;";
        }
        public IDbConnection CreateConnection()
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            return new SqlConnection(_connectionString);
        }
    }
}
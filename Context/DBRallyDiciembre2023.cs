using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using service_rally_diciembre_2023.Utils;
namespace service_rally_diciembre_2023.Context
{
    public class DBRallyDiciembre2023
    {
            private readonly IConfiguration _configuration;
            private readonly string _connectionString;
            public DBRallyDiciembre2023(IConfiguration configuration)
            {
                _configuration = configuration;
                //Cadena de conexion
                var getStringConnectionGuardian = _configuration.GetSection("connectionMysqlRallyDiciembre2023").Get<ConectionString>();
                _connectionString = $"Server={getStringConnectionGuardian.IpServer};Port={getStringConnectionGuardian.Port};Database={getStringConnectionGuardian.Database};User={getStringConnectionGuardian.User};Password={getStringConnectionGuardian.Password};Connection Timeout=0;default command timeout=0;";
            }
            public IDbConnection CreateConnection()
            {
                DefaultTypeMap.MatchNamesWithUnderscores = true;
                return new MySqlConnection(_connectionString);
            }
    }
}
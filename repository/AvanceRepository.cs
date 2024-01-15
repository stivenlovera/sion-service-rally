using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using api_guardian.Utils;
using Dapper;
using service_rally_diciembre_2023.Context;
using service_rally_diciembre_2023.Entities;
using service_rally_diciembre_2023.Entities.DBRallyDiciembre2023;
using service_rally_diciembre_2023.Sql;

namespace service_rally_diciembre_2023.repository
{
    public class AvanceRepository
    {
        private readonly DBRallyDiciembre2023 dBRallyDiciembre2023;
        private readonly ILogger<MiembroAvanceRepository> logger;
        private readonly IDbConnection connection;
        public AvanceRepository(
            DBRallyDiciembre2023 dBRallyDiciembre2023,
            ILogger<MiembroAvanceRepository> logger
        )
        {
            this.dBRallyDiciembre2023 = dBRallyDiciembre2023;
            this.logger = logger;
            this.connection = this.dBRallyDiciembre2023.CreateConnection();
        }
        public async Task<int> Insertar(Avance avance)
        {
            this.logger.LogInformation("AvanceRepository/Insertar({miembroAvanzes})", Helper.Log(avance));
            var query = AvanceSql.Insertar();
            var insert = await connection.QueryAsync<InserGetId>(query, avance);
            this.logger.LogInformation("AvanceRepository/Insertar => SUCCESS {consolidado} resultados", insert);
            return insert.FirstOrDefault().Id;
        }
    }
}
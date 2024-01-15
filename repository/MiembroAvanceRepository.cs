using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using api_guardian.Utils;
using Dapper;
using service_rally_diciembre_2023.Context;
using service_rally_diciembre_2023.Entities.DBRallyDiciembre2023;
using service_rally_diciembre_2023.Entities.DBRallyDiciembre2023.Querys;
using service_rally_diciembre_2023.Sql;

namespace service_rally_diciembre_2023.repository
{
    public class MiembroAvanceRepository
    {
        private readonly DBRallyDiciembre2023 dBRallyDiciembre2023;
        private readonly ILogger<MiembroAvanceRepository> logger;
        private readonly IDbConnection connection;
        public MiembroAvanceRepository(
            DBRallyDiciembre2023 dBRallyDiciembre2023,
            ILogger<MiembroAvanceRepository> logger
        )
        {
            this.dBRallyDiciembre2023 = dBRallyDiciembre2023;
            this.logger = logger;
            this.connection = this.dBRallyDiciembre2023.CreateConnection();
        }
        public async Task<int> Insertar(List<MiembroAvance> miembroAvanzes)
        {
            this.logger.LogInformation("MiembroAvanceRepository/Insertar({miembroAvanzes})", Helper.Log(miembroAvanzes));
            var query = MiembroAvanzeSql.Insertar();
            var consolidado = await connection.ExecuteAsync(query, miembroAvanzes);
            this.logger.LogInformation("MiembroAvanceRepository/Insertar => SUCCESS {consolidado} resultados", consolidado);
            return consolidado;
        }
        public async Task<List<VentasRealizadas>> ObtenerVentasActualizada(int AvanceId)
        {
            this.logger.LogInformation("MiembroAvanceRepository/ObtenerVentasActualizada({avanze})", AvanceId);
            var query = MiembroAvanzeSql.ObtenerVentas();
            var ventas = await connection.QueryAsync<VentasRealizadas>(query, new { AvanceId });
            this.logger.LogInformation("MiembroAvanceRepository/ObtenerVentasActualizada => SUCCESS {consolidado} resultados", ventas.Count());
            return ventas.ToList();
        }
    }
}
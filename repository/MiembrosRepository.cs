using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using api_guardian.Utils;
using Dapper;
using service_rally_diciembre_2023.Context;
using service_rally_diciembre_2023.Entities.DBRallyDiciembre2023;
using service_rally_diciembre_2023.Sql;

namespace service_rally_diciembre_2023.repository
{
    public class MiembrosRepository
    {
        private readonly DBRallyDiciembre2023 dBRallyDiciembre2023;
        private readonly ILogger<MiembrosRepository> logger;
        private readonly IDbConnection connection;
        public MiembrosRepository(
            DBRallyDiciembre2023 dBRallyDiciembre2023,
            ILogger<MiembrosRepository> logger
        )
        {
            this.dBRallyDiciembre2023 = dBRallyDiciembre2023;
            this.logger = logger;
            this.connection = this.dBRallyDiciembre2023.CreateConnection();
        }
        public async Task<List<Miembros>> ObtenerTodo()
        {
            this.logger.LogInformation("MiembrosRepository/ObtenerTodo()");
            var query = MiembrosSql.ObtenerTodo();
            var consolidado = await connection.QueryAsync<Miembros>(query);
            this.logger.LogInformation("MiembrosRepository/ObtenerTodo => SUCCESS {consolidado} resultados", consolidado.Count());
            return consolidado.ToList();
        }
        public async Task<int> Modificar(Miembros miembros)
        {
            this.logger.LogInformation("MiembrosRepository/Modificar({miembros})", Helper.Log(miembros));
            var query = MiembrosSql.Modificar();
            var consolidado = await connection.ExecuteAsync(query, miembros);
            this.logger.LogInformation("MiembrosRepository/ObtenerTodo => SUCCESS {consolidado} resultados", consolidado);
            return consolidado;
        }
    }
}
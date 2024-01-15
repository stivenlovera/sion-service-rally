using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using service_rally_diciembre_2023.Context;
using service_rally_diciembre_2023.Entities.DBComisiones;
using service_rally_diciembre_2023.Sql;

namespace service_rally_diciembre_2023.repository
{
    public class VwListaVentaGralRepository
    {
        private readonly DBComisionesContext dBComisionesContext;
        private readonly ILogger<VwListaVentaGralRepository> logger;
        private readonly IDbConnection connection;
        public VwListaVentaGralRepository(
            DBComisionesContext dBComisionesContext,
            ILogger<VwListaVentaGralRepository> logger
        )
        {
            this.dBComisionesContext = dBComisionesContext;
            this.logger = logger;
            this.connection = this.dBComisionesContext.CreateConnection();
        }
        public async Task<List<CantidadVentaAgrupadaQuery>> ObtenerVentasAgrupadas(List<string> idVendedores)
        {
            this.logger.LogInformation("VwListaVentaGralRepository/ObtenerVentasAgrupadas({idVendedores})", idVendedores);
            var query = VwListaVentaGralSql.ObtenerVentasAgrupadas(idVendedores);
            this.logger.LogInformation("Query {query} ", query);
            var ventas = await connection.QueryAsync<CantidadVentaAgrupadaQuery>(query);
            this.logger.LogInformation("VwListaVentaGralRepository/ObtenerVentasAgrupadas => SUCCESS {ventas} resultados", ventas.Count());
            return ventas.ToList();
        }
        public async Task<List<CantidadVentaQuery>> ObtenerVentasIndividuales(List<string> idVendedores)
        {
            this.logger.LogInformation("VwListaVentaGralRepository/ObtenerVentasIndividuales({idVendedores})", idVendedores);
            var query = VwListaVentaGralSql.ObtenerVentas(idVendedores);
            this.logger.LogInformation("Query {query} ", query);
            var ventas = await connection.QueryAsync<CantidadVentaQuery>(query);
            this.logger.LogInformation("VwListaVentaGralRepository/ObtenerVentasIndividuales => SUCCESS {ventas} resultados", ventas.Count());
            return ventas.ToList();
        }
    }
}
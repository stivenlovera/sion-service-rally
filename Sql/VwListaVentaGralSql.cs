using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace service_rally_diciembre_2023.Sql
{
    public class VwListaVentaGralSql
    {
        public static string ObtenerVentasAgrupadas(List<string> idVendedores)
        {
            return @$"
                SELECT
                    COUNT(ventas.CI_VENDEDOR) AS cantidad,
                    SUM(ventas.PRECIOVENTA) AS total_venta,
                    ventas.CI_VENDEDOR,
                    ventas.NOMBRE_VENDEDOR
                FROM BDComisiones.dbo.vwLISTAVENTAS_GRAL as ventas
                WHERE
                ventas.FECHAVENTA >= '20231201'
                    and ventas.FECHAVENTA <= '20231231'
                    and ventas.CI_VENDEDOR in
                (
                    '{String.Join("', '", idVendedores.ToArray())}'  
                )
                GROUP BY ventas.CI_VENDEDOR,ventas.NOMBRE_VENDEDOR
            ";
        }
        public static string ObtenerVentas(List<string> idVendedores)
        {
            return @$"
                SELECT
                    ventas.FECHAVENTA,
                    ventas.CI_CLIENTE,
                    ventas.NOMBRE_CLIENTE,
                    ventas.IDVENTA,
                    ventas.IDPRODUCTO,
                    ventas.PRECIOVENTA,
                    ventas.CI_VENDEDOR,
                    ventas.NOMBRE_VENDEDOR
                FROM BDComisiones.dbo.vwLISTAVENTAS_GRAL as ventas
                WHERE
                ventas.FECHAVENTA >= '20231201'
                    and ventas.FECHAVENTA <= '20231231'
                    and ventas.CI_VENDEDOR in
                (
                    '{String.Join("', '", idVendedores.ToArray())}'  
                )
                order by NOMBRE_VENDEDOR;
            ";
        }
    }
}
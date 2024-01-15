using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace service_rally_diciembre_2023.Sql
{
    public class MiembroAvanzeSql
    {
        public static string Insertar()
        {
            return @"
             insert into 
                miembro_avance (
                miembros_id, 
                avance_id, 
                cantidad, 
                monto_total,
                cantidad_red,
                monto_total_red,
                nota,
                nota_red
                )
            values
                (
                @MiembrosId, 
                @AvanceId, 
                @Cantidad, 
                @MontoTotal,
                @CantidadRed,
                @MontoTotalRed,
                @Nota,
                @NotaRed
                );
            ";
        }
        public static string ObtenerVentas()
        {
            return @"
             SELECT
                m.nombre_completo,
                m.num_identidad,
                d.codigo,
                tm.nombre_tipo_miembro,
                e.nombre_equipo,
                ma.cantidad,
                ma.monto_total,
                ma.cantidad_red,
                ma.monto_total_red
            FROM miembros as m
                INNER JOIN miembro_avance as ma on ma.miembros_id = m.miembros_id
                INNER JOIN tipo_miembro as tm on tm.tipo_miembro_id = m.tipo_miembro_id
                INNER JOIN equipo as e on e.equipo_id = m.equipo_id
                INNER JOIN departamento as d on d.departamento_id = e.departamento_id
                INNER JOIN avance as a on a.avance_id = ma.avance_id
            WHERE a.avance_id=@AvanceId
            ORDER BY
                e.nombre_equipo,
                tm.tipo_miembro_id ASC;
            ";
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace service_rally_diciembre_2023.Sql
{
    public class MiembrosSql
    {
        public static string ObtenerTodo()
        {
            return @"
            SELECT
                miembros_id,
                lcontacto_id,
                nombre_completo,
                num_identidad,
                equipo_id,
                tipo_miembro_id,
                departamento_id,
                nota
            FROM miembros;
            ";
        }
        public static string Modificar()
        {
            return @"
            update 
                miembros 
                set 
                nombre_completo = @NombreCompleto,
                num_identidad = @NumIdentidad
                where 
                miembros_id = @MiembrosId;
            ";
        }
    }
}
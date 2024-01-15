using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace service_rally_diciembre_2023.Sql
{
    public class AvanceSql
    {
        public static string Insertar()
        {

            return @"
                 insert into 
                    avance (
                    fecha_revision, 
                    detalle
                    )
                values
                    (
                    @FechaRevision, 
                    @Detalle
                    );
                SELECT LAST_INSERT_ID() as id;
            ";
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace service_rally_diciembre_2023.Entities.DBRallyDiciembre2023
{
    public class Avance
    {
        public int AvanceId { get; set; }
        public DateTime FechaRevision { get; set; }
        public string Detalle { get; set; }
    }
}
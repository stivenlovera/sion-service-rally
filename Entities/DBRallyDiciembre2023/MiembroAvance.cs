using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace service_rally_diciembre_2023.Entities.DBRallyDiciembre2023
{
    public class MiembroAvance
    {
        public int MiembroAvanceId { get; set; }
        public int MiembrosId { get; set; }
        public int AvanceId { get; set; }
        public int Cantidad { get; set; }
        public decimal MontoTotal { get; set; }
        public int CantidadRed { get; set; }
        public decimal MontoTotalRed { get; set; }
        public string NotaRed { get; set; }
        public string Nota { get; set; }
    }
}
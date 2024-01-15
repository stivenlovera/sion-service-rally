using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace service_rally_diciembre_2023.Entities.DBRallyDiciembre2023.Querys
{
    public class VentasRealizadas
    {
        public string NombreCompleto { get; set; }
        public string NumIdentidad { get; set; }
        public string Codigo { get; set; }
        public string NombreTipoMiembro { get; set; }
        public string NombreEquipo { get; set; }
        public int Cantidad { get; set; }
        public decimal MontoTotal { get; set; }
        public int CantidadRed { get; set; }
        public decimal MontoTotalRed { get; set; }
    }
}
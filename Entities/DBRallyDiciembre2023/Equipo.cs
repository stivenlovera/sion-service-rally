using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace service_rally_diciembre_2023.Entities.DBRallyDiciembre2023
{
    public class Equipo
    {
        public int EquipoId { get; set; }
        public string NombreEquipo { get; set; }
        public decimal TotalVenta { get; set; }
        public int DepartamentoId { get; set; }
    }
}
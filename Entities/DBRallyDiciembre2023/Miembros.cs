using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace service_rally_diciembre_2023.Entities.DBRallyDiciembre2023
{
    public class Miembros
    {
        public int MiembrosId { get; set; }
        public string NombreCompleto { get; set; }
        public string NumIdentidad { get; set; }
        public int EquipoId { get; set; }
        public int TipoMiembroId { get; set; }
        public int DepartamentoId { get; set; }
        public string Nota { get; set; }
    }
}
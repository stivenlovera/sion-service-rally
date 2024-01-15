using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace service_rally_diciembre_2023.Entities.DBComisiones
{
    public class VwlistaventasGral
    {
        public string Empresa { get; set; }
        public int Idalamcen { get; set; }
        public int Idventa { get; set; }
        public DateTime Fechaventa { get; set; }
        public string Idproducto { get; set; }
        public int Idcliente { get; set; }
        public string CiCliente { get; set; }
        public string NombreCliente { get; set; }
        public int Idvendedor { get; set; }
        public string CiVendedor { get; set; }
        public string NombreVendedor { get; set; }
        public int Idtipoventa { get; set; }
        public decimal Precioventa { get; set; }
        public decimal Cuotainicial { get; set; }
        public int ValorCi { get; set; }
        public int Montoabonado { get; set; }
        public decimal Totaldeuda { get; set; }
        public string Tipoventa { get; set; }
        public int IdestadoVenta { get; set; }
        public int Idestado { get; set; }
        public string Glosa { get; set; }
        public string Nrodoc { get; set; }
        public int Kit { get; set; }
        public DateTime FechaReserva { get; set; }
    }
    public class CantidadVentaAgrupadaQuery
    {
        public int Cantidad { get; set; }
        public decimal TotalVenta { get; set; }
        public string CiVendedor { get; set; }
        public string NombreVendedor { get; set; }
    }
    public class CantidadVentaQuery
    {
        public DateTime Fechaventa { get; set; }
        public string CiCliente { get; set; }
        public string NombreCliente { get; set; }
        public int Idventa { get; set; }
        public string Idproducto { get; set; }
        public decimal Precioventa { get; set; }
        public string CiVendedor { get; set; }
        public string NombreVendedor { get; set; }
    }
}
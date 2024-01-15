using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using api_guardian.Utils;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using service_rally_diciembre_2023.Entities.DBRallyDiciembre2023;
using service_rally_diciembre_2023.Entities.DBRallyDiciembre2023.Querys;
using service_rally_diciembre_2023.repository;
using service_rally_diciembre_2023.TemplateHtml;
using service_rally_diciembre_2023.Utils;

namespace service_rally_diciembre_2023.Modules
{
    public class ActualizacionRallyModulo
    {
        private readonly ILogger<ActualizacionRallyModulo> logger;
        private readonly MiembrosRepository miembrosRepository;
        private readonly VwListaVentaGralRepository vwListaVentaGralRepository;
        private readonly MiembroAvanceRepository miembroAvanceRepository;
        private readonly AvanceRepository avanceRepository;
        private readonly EmailHttpClient emailHttpClient;
        private readonly EmailRequest emailRequest;

        public ActualizacionRallyModulo(
            ILogger<ActualizacionRallyModulo> logger,
            MiembrosRepository miembrosRepository,
            VwListaVentaGralRepository vwListaVentaGralRepository,
            MiembroAvanceRepository miembroAvanceRepository,
            AvanceRepository avanceRepository,
            EmailHttpClient EmailHttpClient,
            EmailRequest emailRequest
        )
        {
            this.logger = logger;
            this.miembrosRepository = miembrosRepository;
            this.vwListaVentaGralRepository = vwListaVentaGralRepository;
            this.miembroAvanceRepository = miembroAvanceRepository;
            this.avanceRepository = avanceRepository;
            this.emailHttpClient = EmailHttpClient;
            this.emailRequest = emailRequest;
        }
        public async Task<string> ActualizacionRally()
        {
            this.logger.LogInformation("ActualizacionRallyModulo/ActualizacionRally()");
            var listaMiembros = await this.miembrosRepository.ObtenerTodo();
            var carnetIdentidad = listaMiembros.Select(x => x.NumIdentidad.Trim()).ToList();
            this.logger.LogInformation("listaMiembros => {listaMiembros}", Helper.Log(carnetIdentidad.Count));

            var listaVentas = await this.vwListaVentaGralRepository.ObtenerVentasAgrupadas(carnetIdentidad);
            //avance
            var avanze = new Avance()
            {
                AvanceId = 0,
                Detalle = $"REPORTE RALLY DE VENTAS DICIEMBRE DESDE 04-12-2023 AL 31-12-2023",
                FechaRevision = DateTime.Now
            };
            var insertAvance = await this.avanceRepository.Insertar(avanze);

            //miembros avance
            var avanzeMiembros = new List<MiembroAvance>();
            foreach (var miembro in listaMiembros)
            {
                var verificar = listaVentas.Where(x => x.CiVendedor == miembro.NumIdentidad).FirstOrDefault();
                if (verificar == null)
                {
                    avanzeMiembros.Add(new MiembroAvance
                    {
                        Cantidad = 0,
                        AvanceId = insertAvance,
                        MiembrosId = miembro.MiembrosId,
                        MontoTotal = 0,
                        CantidadRed = 0,
                        MontoTotalRed = 0,
                        Nota = "",
                        NotaRed = ""
                    });
                }
                else
                {
                    var validando = await this.VerificarVenta(miembro);
                    avanzeMiembros.Add(new MiembroAvance
                    {
                        Cantidad = verificar.Cantidad,
                        AvanceId = insertAvance,
                        MiembrosId = miembro.MiembrosId,
                        MontoTotal = verificar.TotalVenta,
                        CantidadRed = validando.CantidadRed,
                        MontoTotalRed = validando.MontoTotalRed,
                        Nota = validando.Nota,
                        NotaRed = validando.NotaRed
                    });
                }
            }

            var insertMiembroAvance = await this.miembroAvanceRepository.Insertar(avanzeMiembros);
            var resultado = await this.miembroAvanceRepository.ObtenerVentasActualizada(insertAvance);

            var base64Excel = this.ExportExcelPlantilla(resultado, avanze, insertAvance);
            await this.EmailNotificacion(base64Excel, avanze, insertAvance);
            //enviando email

            return $"inserciones {insertMiembroAvance}";
        }

        public async Task<bool> EmailNotificacion(string base64, Avance avance, int AvanceId)
        {
            this.emailRequest.Proyecto = "COMISIONES - Rally diciembre 2023";
            this.emailRequest.Modulo = "";
            this.emailRequest.Asunto = $"Reporte automatico rally de ventas Id: {AvanceId} Fecha: {avance.FechaRevision.ToString("dd-MM-yyyy HH:mm:ss")} ";
            this.emailRequest.Mensaje = HtmlAlertaMessage.MessageAlerta();
            this.emailRequest.ArchivoAdjuntos = new List<ArchivoAdjuntos>(){
                new ArchivoAdjuntos
                {
                    Archivo = base64,
                    Nombre = $"{avance.Detalle}.xlsx"
                }
            };

            await this.emailHttpClient.Run("correoservice.gruposion.bo", "api/mensajeria/EnvioCorreo", this.emailRequest);
            return true;
        }

        public async void Correcion(List<Miembros> miembros)
        {
            foreach (var miembro in miembros)
            {
                miembro.NumIdentidad = Regex.Replace(miembro.NumIdentidad, @"\t|\n|\r", "");
                miembro.NombreCompleto = Regex.Replace(miembro.NombreCompleto, @"\t|\n|\r", "");
                var moficado = await this.miembrosRepository.Modificar(miembro);
                this.logger.LogInformation("modificado => {moficado}", Helper.Log(moficado));
            }

        }

        public string ExportExcelPlantilla(List<VentasRealizadas> ventasRealizadas, Avance avance, int AvanceId)
        {
            this.logger.LogInformation("ExportExcel({ventasRealizadas},{avance})", Helper.Log(ventasRealizadas), Helper.Log(avance));
            //OBTENER plantilla
            var path = System.IO.Path.Combine(Environment.CurrentDirectory, "Plantilla", "VentasRealizadas.xlsx");
            this.logger.LogInformation("ubicacion({path})", Helper.Log(path));

            //Modificacion de plantilla
            XLWorkbook workbook = new XLWorkbook(path);
            var hoja = workbook.Worksheets.Worksheet(1);
            //titulo
            hoja.Cell(2, 2).SetValue(avance.Detalle);
            hoja.Cell(4, 2).SetValue($"Id: {AvanceId}");
            hoja.Cell(4, 3).SetValue($"Fecha generacion {avance.FechaRevision.ToString("dd-MM-yyyy HH:mm:ss")}");
            var row = 6;

            int i = 0;
            foreach (var data in ventasRealizadas)
            {
                hoja.Cell(row, 2).SetValue(i + 1);
                hoja.Cell(row, 3).SetValue(data.NombreCompleto);
                hoja.Cell(row, 4).SetValue(data.NumIdentidad);
                hoja.Cell(row, 5).SetValue(data.Codigo);
                hoja.Cell(row, 6).SetValue(data.NombreEquipo);
                hoja.Cell(row, 7).SetValue(data.NombreTipoMiembro);
                hoja.Cell(row, 8).SetValue(data.Cantidad);
                hoja.Cell(row, 9).SetValue(data.MontoTotal);
                hoja.Cell(row, 10).SetValue(data.CantidadRed);
                hoja.Cell(row, 11).SetValue(data.MontoTotalRed);
                row++;
                i++;
            }
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            String AsBase64String = Convert.ToBase64String(content);

            return AsBase64String;
        }
        public async Task<MiembroAvance> VerificarVenta(Miembros miembros)
        {
            var listaVentas = await this.vwListaVentaGralRepository.ObtenerVentasIndividuales(new List<string>() { miembros.NumIdentidad });
            var ciClientes = listaVentas.Select(x => x.CiCliente).ToList();
            logger.LogWarning("Verificando venta rally del cliente {miembros}", Helper.Log(miembros));
            //verificando ventas en red
            var data = await this.RastreoNiveles(ciClientes);
            foreach (var venta in listaVentas)
            {
                data.Nota += $"[precio:{venta.Precioventa} Idventa:{venta.Idventa}, Idproducto:{venta.Idproducto.Trim()} cliente: {venta.CiCliente}-{venta.NombreCliente.Trim()} vendedor: {venta.CiVendedor}-{venta.NombreVendedor.Trim()} {(venta.CiCliente == venta.CiVendedor ? "autocompra" : "")}]";
            }
            return data;
        }
        public async Task<MiembroAvance> RastreoNiveles(List<string> ciClientes)
        {
            var niveles = new List<int>() { 1, 2, 3, 4, 5, 6, 7 };
            var ciClienteMovimiento = ciClientes;
            var cantVenta = 0;
            decimal totalVenta = 0;
            var nota = "";
            foreach (var nivel in niveles)
            {
                var listaRed = await this.vwListaVentaGralRepository.ObtenerVentasIndividuales(ciClienteMovimiento);
                if (listaRed.Count() > 0)
                {
                    logger.LogWarning("Tiene ventas cliente {nivel} venta {listaRed}", nivel, Helper.Log(listaRed));
                    foreach (var red in listaRed)
                    {
                        //verificando si no es autocompra
                        if (red.CiCliente != red.CiVendedor)
                        {
                            cantVenta++;
                            totalVenta += red.Precioventa;
                            nota += $"[nivel:{nivel} precio:{red.Precioventa} Idventa:{red.Idventa}, Idproducto:{red.Idproducto.Trim()} cliente: {red.CiCliente}-{red.NombreCliente.Trim()} vendedor: {red.CiVendedor}-{red.NombreVendedor.Trim()} {(red.CiCliente == red.CiVendedor ? "autocompra" : "")}]";
                        }
                    };
                    var filtroNoAutocompra = listaRed.Where(x => x.CiCliente != x.CiVendedor).ToList();
                    ciClienteMovimiento = filtroNoAutocompra.Select(x => x.CiCliente).ToList();
                }
                else
                {
                    logger.LogInformation("No Tiene ventas cliente");
                    break;
                }
            }
            return new MiembroAvance
            {
                CantidadRed = cantVenta,
                NotaRed = nota,
                MontoTotalRed = totalVenta
            };
        }
    }
}
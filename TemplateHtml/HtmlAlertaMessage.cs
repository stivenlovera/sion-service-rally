using service_rally_diciembre_2023.Model;

namespace service_rally_diciembre_2023.TemplateHtml
{
    public class HtmlAlertaMessage
    {
        public static string MessageAlerta()
        {
            string IpOrigen = "";

            System.Net.IPHostEntry entry = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());

            foreach (System.Net.IPAddress ip in entry.AddressList)
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    IpOrigen = ip.ToString();

            string style = @"
                <style>
                    body {
                        font: Serif;
                        font-size: 13px;
                        text-rendering: geometricPrecision;
                        margin-top: 20px;
                    }

                    .centrado {
                        text-align: center;
                        font-size: 19px;
                        margin: 0px;
                        padding: 0px;
                    }

                    .titulo {
                        margin-left: 5%;
                        margin-right: 5%;
                    }

                    p {
                        padding-bottom: 0px;
                    }

                    p.seguido {
                        margin-top: 0px;
                        margin-bottom: 0px;
                    }

                    .container {
                        text-align: center;
                    }

                    .left {
                        float: left;
                    }

                    .right {
                        float: right;
                    }

                    .center {

                    }
                    table, td, th {
                        padding: 5px;
                        border: 1px solid;
                    }

                    table {
                        border-collapse: collapse;
                    }
                </style>
            ";

            return $@"
                <!DOCTYPE html>
                <html lang='es'>

                    <head>
                        <meta charset='UTF-8'>
                        <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <title>Reporte</title>
                        {style}
                    </head>
                    
                    <body>
                        <h3>REPORTE AUTOMATICO</h3>
                        <h4>Ventas realizadas Rally diciembre 2023</h4>
                        <p><strong>IP ORIGEN:</strong> {IpOrigen}</p>
                    </body>
                </html>
            ";
        }
    }
}

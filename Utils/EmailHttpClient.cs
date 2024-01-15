using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using api_guardian.Utils;
using Newtonsoft.Json;

namespace service_rally_diciembre_2023.Utils
{
    public class EmailHttpClient
    {
        static readonly HttpClient client = new HttpClient();
        private readonly ILogger<EmailHttpClient> _logger;

        public EmailHttpClient(
            ILogger<EmailHttpClient> logger
        )
        {
            this._logger = logger;
        }
        public async Task<Response> Run(string host, string url, EmailRequest emailRequest)
        {
            try
            {
                this._logger.LogInformation("Enviando Email a las siguente personas {emailRequest}....", Helper.Log(emailRequest));
                var stringContent = new StringContent(JsonConvert.SerializeObject(emailRequest), UnicodeEncoding.UTF8, "application/json");
                this._logger.LogWarning("data  {responseBody}", Helper.Log(stringContent));
                HttpResponseMessage response = await client.PostAsync($"https://{host}/{url}", stringContent);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                this._logger.LogWarning("Enviado correctamente {responseBody}", Helper.Log(responseBody));
                return new Response()
                {
                    estado = true,
                    data = responseBody
                };

            }
            catch (HttpRequestException e)
            {
                this._logger.LogCritical($"Error al enviar email {e}");
                return new Response()
                {
                    estado = false,
                    data = "error"
                };
            }
        }
    }
    public class Response
    {
        public bool estado { get; set; }
        public string data { get; set; }
    }
    public class EmailRequest
    {
        private readonly IConfiguration _configuration;

        public EmailRequest(
            IConfiguration configuration
        )
        {
            this._configuration = configuration;
            this.Destinatarios = this._configuration.GetSection("destinatarios").Get<List<string>>();
            this.CcDestinatarios = this._configuration.GetSection("ccDestinatarios").Get<List<string>>();
        }
        [JsonProperty(PropertyName = "proyecto")]
        public string Proyecto { get; set; } = "Comisiones Prueba";
        [JsonProperty(PropertyName = "modulo")]
        public string Modulo { get; set; } = "Comisiones";
        [JsonProperty(PropertyName = "destinatarios")]
        public List<string> Destinatarios { get; set; } //informacion appsetting
        [JsonProperty(PropertyName = "ccDestinatarios")]
        public List<string> CcDestinatarios { get; set; } //informacion appsetting
        [JsonProperty(PropertyName = "asunto")]
        public string Asunto { get; set; } = "prueba servicio job";
        [JsonProperty(PropertyName = "mensaje")]
        public string Mensaje { get; set; }
        [JsonProperty(PropertyName = "archivoAdjuntos")]
        public List<ArchivoAdjuntos> ArchivoAdjuntos { get; set; }
    }
    public class ArchivoAdjuntos
    {
        [JsonProperty(PropertyName = "nombre")]
        public string Nombre { get; set; }
        [JsonProperty(PropertyName = "archivo")]
        public string Archivo { get; set; }
    }
}
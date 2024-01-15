using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace api_guardian.Utils
{
    public class Helper
    {
        public static string Log(object data)
        {
            var resultado = JsonConvert.SerializeObject(data, Formatting.Indented);
            return resultado;
        }
        public static string MayusMinus(string texto)
        {
            return Regex.Replace(texto.ToLower(), @"((^\w)|(\s|\p{P})\w)", match => match.Value.ToUpper());
        }
    }
}
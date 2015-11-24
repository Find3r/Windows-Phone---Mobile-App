using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pineable.Model
{
    public class Noticia_usuario
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("idusuario")]
        public string IdUsuario { get; set; }

        [JsonProperty("idnoticia")]
        public string IdNoticia { get; set; }

        [JsonProperty("estado_seguimiento")]
        public bool Estado { get; set; }
    }
}

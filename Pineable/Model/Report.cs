using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pineable.Model
{
    public class Report
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("descripcion")]
        public string Description { get; set; }

        [JsonProperty("idnoticia")]
        public string IdNew { get; set; }

        [JsonProperty("idusuario")]
        public string IdUser { get; set; }
    }
}

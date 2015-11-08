using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pineable.Model
{
    public class Notification
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "idnoticia")]
        public string IdNew { get; set; }

        [JsonProperty(PropertyName = "__createdAt")]
        public DateTime DateCreation { get; set; }

        [JsonProperty(PropertyName = "descripcion")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "fecha")]
        public string Date { get; set; }

        [JsonProperty(PropertyName = "hora")]
        public string Hour { get; set; }
    }
}

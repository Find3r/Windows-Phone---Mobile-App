using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pineable.Model
{
    public class Notificacionusuario
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "idusuario")]
        public string IdUser { get; set; }

        [JsonProperty(PropertyName = "idnoticia")]
        public string IdNew { get; set; }

        [JsonProperty(PropertyName = "__createdAt")]
        public DateTime DateCreated { get; set; }

        [JsonProperty(PropertyName = "idnotificacion")]
        public string IdNotification { get; set; }

        [JsonProperty(PropertyName = "estadoleido")]
        public bool StatusRead { get; set; }

        [JsonProperty(PropertyName = "descripcion")]
        public string Description { get; set; }

        public string DateShort
        {
            get
            {
                return DateCreated.ToString("dd-MM-yyyy", new CultureInfo("es-ES"));
            }
        }
    }
}

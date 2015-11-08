using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pineable.Model
{
    public class User
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "nombre")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "primerapellido")]
        public string FirstLastName { get; set; }

        [JsonProperty(PropertyName = "segundoapellido")]
        public string SecondLastName { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "telefonomovil")]
        public string PhoneNumber { get; set; }

        [JsonProperty(PropertyName = "telefonohogar")]
        public string HomePhone { get; set; }

        [JsonProperty(PropertyName = "urlimagen")]
        public string PictureUrl {get; set; }

        [JsonProperty(PropertyName = "cover_picture")]
        public string CoverPicture { get; set; }

        [JsonProperty(PropertyName = "idpais")]
        public string IdCountry { get; set; }
    }
}

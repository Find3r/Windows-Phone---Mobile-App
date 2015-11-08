using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pineable.Model
{
    public class New
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "nombre")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "descripcion")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "urlimagen")]
        public string PictureURL { get; set; }

        [JsonProperty(PropertyName = "fechadesaparicion")]
        public DateTime DateLost { get; set; }

        [JsonProperty(PropertyName = "idusuario")]
        public string IdUser { get; set; }

        [JsonProperty(PropertyName = "idestado")]
        public string IdStatus { get; set; }

        [JsonProperty(PropertyName = "idcategoria")]
        public string IdCategory { get; set; }

        [JsonProperty(PropertyName = "idprovincia")]
        public string IdZone { get; set; }

        [JsonProperty(PropertyName = "cantidad_reportes")]
        private int ? quantityReports { get; set; }

        [JsonProperty(PropertyName = "latitud")]
        public string Latitude { get; set; }

        [JsonProperty(PropertyName = "longitud")]
        public string Longitude { get; set; }
        
        public string DateShort
        {
            get
            {
                return DateLost.ToString("dd-MM-yyyy", new CultureInfo("es-ES"));
            }
        }

        public int QuantityReports
        {
            get
            {
                if (quantityReports == null)
                {
                    return 0;
                }
                else
                {
                    return (int) quantityReports;
                }
            }
            set { quantityReports = value; }
        }

    }
}

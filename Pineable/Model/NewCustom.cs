using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pineable.Model
{
    public class NewCustom : New
    {
        [JsonProperty(PropertyName = "Column13")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "Column14")]
        public string UserPictureURL { get; set; }

        [JsonProperty(PropertyName = "Column15")]
        public string NameZone { get; set; }

        [JsonProperty(PropertyName = "Column16")]
        private int? quantityComments { get; set; }

        [JsonProperty(PropertyName = "estado_seguimiento")]
        public bool StatusFollow { get; set; }

        public int QuantityComments
        {
            get
            {
                if (quantityComments == null)
                {
                    return 0;
            
                }
                else
                {
                    return (int)quantityComments;
                }
            }
            set { quantityComments = value; }
        }




        public string StatusPictureURL
        {
            get
            {
                switch (IdStatus)
                {
                    case "0":
                        return "ms-appx:///Assets/lost.png";

                    case "1":
                        return "ms-appx:///Assets/found.png";

                    default:
                        return "";
                }

            }
        }

        public string StatusName
        {
            get
            {

                switch (IdStatus)
                {
                    case "0":
                        return "Desaparecido(a)";

                    case "1":
                        return "Encontrado(a)";

                    default:
                        return "";
                }
            }
        }

    }
}

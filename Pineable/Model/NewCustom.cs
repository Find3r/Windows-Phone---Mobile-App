using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pineable.Model
{
    public class NewCustom : Noticia
    {
        [JsonProperty(PropertyName = "nombre_usuario")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "urlimagen_perfil_usuario")]
        public string UserPictureURL { get; set; }

        [JsonProperty(PropertyName = "nombre_provincia")]
        public string NameZone { get; set; }

        [JsonProperty(PropertyName = "cantidad_comentarios")]
        private int? quantityComments { get; set; }

        [JsonProperty(PropertyName = "estado_follow")]
        public bool ?StatusFollow { get; set; }

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

        public string TypePictureURL
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



        public string StatusPictureURL
        {
            get
            {

                if (Solved == null)
                {
                    Solved = false;
                }

                return ((bool)Solved) ? "ms-appx:///Assets/solved.png" : "ms-appx:///Assets/unsolved.png";
            }
        }

        public string FollowStatusPictureURL
        {
            get
            {

                if (StatusFollow == null)
                {
                    StatusFollow = false;
                }

                return ((bool)StatusFollow) ? "ms-appx:///Assets/unfollow.png" : "ms-appx:///Assets/follow.png";

            }
        }


      





        public string StatusName
        {
            get
            {
                if(Solved == null)
                {
                    Solved = false;
                }

                return ((bool)Solved) ? "Resuelto" : "Pendiente";
            }
        }

        public int StatusId
        {
            get
            {
                if (Solved == null)
                {
                    Solved = false;
                }

                return ((bool)Solved) ? 1 : 0;
            }
        }

        public string StatusFollowName
        {
            get
            {
                if (StatusFollow == null)
                {
                    StatusFollow = false;
                }
                
                return ((bool)StatusFollow) ? "Descartar" : "Seguir";
            }
        }

        public string TypeName
        {
            get
            {

                switch (IdStatus)
                {
                    case "0":
                        return "Perdid@";

                    case "1":
                        return "Encontrad@";

                    default:
                        return "";
                }
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pineable.Model
{
    public class NewCustom : New
    {
        public string UserName { get; set; }

        public string UserPictureURL { get; set; }

        public string NameZone { get; set; }

        public int QuantityComments { get; set; }

        public bool StatusFollow { get; set; }


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

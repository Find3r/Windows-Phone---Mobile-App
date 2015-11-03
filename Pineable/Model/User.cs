using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pineable.Model
{
    public class User
    {
        //[JsonProperty("id")]
        public string Id { get; set; }

        //[JsonProperty("nombre")]
        public string Name { get; set; }

        //[JsonProperty("primerapellido")]
        public string FirstLastName { get; set; }

        //[JsonProperty("segundoapellido")]
        public string SecondLastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }


        public string HomePhone { get; set; }


        //[JsonProperty("urlimagen")]
        public string PictureUrl {get; set; }

        //[JsonProperty("cover_picture")]
        public string CoverPicture { get; set; }

        //[JsonProperty("idpais")]
        public string IdPais { get; set; }
    }
}

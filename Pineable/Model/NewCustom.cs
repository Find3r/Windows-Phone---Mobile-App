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

    }
}

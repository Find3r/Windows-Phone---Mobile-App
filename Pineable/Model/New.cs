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
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string PictureURL { get; set; }

        public DateTime DateLost { get; set; }

        public string IdUser { get; set; }

        public string IdStatus { get; set; }

        public string IdCategory { get; set; }

        public string IdZone { get; set; }

        public int QuantityReports { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string DateShort
        {
            get
            {

                return DateLost.ToString("dd-MM-yyyy", new CultureInfo("es-ES"));
            }
        }
    }
}

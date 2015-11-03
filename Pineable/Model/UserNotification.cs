using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pineable.Model
{
    public class UserNotification
    {
        public string Id { get; set; }

        public string IdUser { get; set; }

        public string IdNew { get; set; }

        public string DateCreated { get; set; }

        public string IdNotification { get; set; }

        public bool StatusRead { get; set; }

        public string Description { get; set; }
    }
}

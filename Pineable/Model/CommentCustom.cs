using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pineable.Model
{
    public class CommentCustom : Comment
    {
        public string UserName { get; set; }

        public string UserPictureURL { get; set; }
    }
}

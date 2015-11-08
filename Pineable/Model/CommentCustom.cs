using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pineable.Model
{
    public class CommentCustom : Comment
    {
        [JsonProperty(PropertyName = "nombre")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "urlimagen")]
        public string UserPictureURL { get; set; }
    }
}

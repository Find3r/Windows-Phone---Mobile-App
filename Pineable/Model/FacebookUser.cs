using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pineable.Model
{
    public class FacebookUser
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "gender")]
        public string Gender { get; set; }

        [JsonProperty(PropertyName = "link")]
        public string ProfileLink { get; set; }

        [JsonProperty(PropertyName = "picture")]
        public Data PictureData { get; set; }

        [JsonProperty(PropertyName = "cover")]
        public CoverPhoto PictureCoverURL { get; set; }


    }

    public class Data
    {
        [JsonProperty(PropertyName = "data")]
        public Data2 PictureURL { get; set; }
    }

    public class Data2
    {
        [JsonProperty(PropertyName = "url")]
        public string PictureURL { get; set; }
    }

    public class CoverPhoto
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "offset_x")]
        public float Offset_X { get; set; }

        [JsonProperty(PropertyName = "offset_y")]
        public float Offset_Y { get; set; }

        [JsonProperty(PropertyName = "source")]
        public string PictureUrl { get; set; }

    }
}

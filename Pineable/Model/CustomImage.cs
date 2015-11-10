using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pineable.Model
{
    public class CustomImage
    {
        [JsonProperty("FileName")]
        public string FileName { get; set; }

        [JsonProperty("FileBytes")]
        public byte[] FileBytes { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pineable.Model
{
    public class FacebookUser
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Gender { get; set; }

        public string ProfileLink { get; set; }

        public Data PictureData { get; set; }

        public CoverPhoto PictureCoverURL { get; set; }


    }

    public class Data
    {
        public Data2 PictureURL { get; set; }
    }

    public class Data2
    {
        public string PictureURL { get; set; }
    }

    public class CoverPhoto
    {
        public string Id { get; set; }

        public float Offset_X { get; set; }

        public float Offset_Y { get; set; }

        public string PictureUrl { get; set; }

    }
}

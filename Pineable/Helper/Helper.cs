using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Pineable.Helper
{
    public class Helper
    {


        public static string GetUserId(string userId)
        {
            // separamos el string
            string[] arrayString = userId.Split(':');

            return arrayString[1];
        }

        public static ImageSource cargarImagen(string urlImagen, UriKind tipoUri)
        {
            Uri uri = new Uri(urlImagen, tipoUri);
            ImageSource imgSource = new BitmapImage(uri);
            return imgSource;
        }

        public static ImageSource cargarImagen(Uri uri, UriKind tipoUri)
        {
            ImageSource imgSource = new BitmapImage(uri);
            return imgSource;
        }

        public static ImageBrush cargarImagenFondo(string urlImagen, UriKind tipoUri)
        {
            Uri uri = new Uri(urlImagen, tipoUri);
            ImageBrush ib = new ImageBrush();

            ib.ImageSource = cargarImagen(uri, tipoUri);
            return ib;
        }

        public static ImageBrush cargarImagenFondo(Uri uri, UriKind tipoUri)
        {
            ImageBrush ib = new ImageBrush();

            ib.ImageSource = cargarImagen(uri, tipoUri);

            return ib;
        }

    }
}

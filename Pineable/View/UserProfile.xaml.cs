using Microsoft.WindowsAzure.MobileServices;
using Pineable.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Pineable.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserProfile : Page
    {
        NewCustom objNoticiaAux;
        public UserProfile()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NewCustom objNewCustom = e.Parameter as NewCustom;

            if (objNewCustom != null)
            {
                objNoticiaAux = objNewCustom;
                verificarConexion();

            }
        }

        private void lstvUltimasNoticias_ItemClick(object sender, ItemClickEventArgs e)
        {
            objNoticiaAux = e.ClickedItem as NewCustom;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            verificarConexion();
        }

        private async void verificarConexion()
        {

            if (await App.CheckInternetConnection())
            {
                //Hay conexión a Internet
                progressRing.IsActive = true;

                cargarDatos();
                //progressRing.IsActive = false;

            }
            else
            {
                //No hay conexión a Internet
                MessageDialog info = new MessageDialog("Verfique la conexión a Internet");
                await info.ShowAsync();
                cargarDatosOffline();
            }

        }

        private void cargarDatosOffline()
        {
            Usuario objUser = new Usuario();
            objUser.Id = "1";
            objUser.IdCountry = "1";
            objUser.Name = "Carlos Castro Brenes";
            objUser.PictureUrl = "ms-appx:///Assets/user.png";
            objUser.CoverPicture = "ms-appx:///Assets/Hawaiio.jpg";

            lstvUserPosts.DataContext = objUser;


            string descripcion = "Bacon ipsum dolor amet swine ham hock drumstick tail. Meatloaf jowl andouille jerky salami pork belly alcatra frankfurter prosciutto kevin.Tongue corned beef kielbasa salami t-bone, rump shoulder meatball pork loin cupim. Andouille ham flank pork shankle ham hock short loin rump salami tenderloin biltong." +

"Frankfurter andouille biltong ball tip filet mignon, sirloin turducken swine t-bone shank pork chop pastrami. Shoulder porchetta tenderloin brisket, beef ribs turkey ham pork flank alcatra ground round. Doner meatball kevin swine t - bone cupim picanha. Strip steak filet mignon ribeye, fatback pig rump pancetta. Chicken capicola drumstick doner rump tail frankfurter jowl turducken swine pastrami sausage t - bone alcatra.Capicola short loin biltong picanha doner hamburger shoulder jerky meatloaf short ribs. Boudin pork belly alcatra strip steak salami ball tip ham hock pork chop drumstick sirloin t-bone.";

            descripcion += descripcion;

            byte estado = 0;

            List<NewCustom> lstNoticias = new List<NewCustom>();
            for (int i = 0; i < 20; i++)
            {
                if (i % 2 == 0)
                {
                    estado = 0;
                }
                else
                {
                    estado = 1;
                }

                lstNoticias.Add(new NewCustom() { Id = "1", PictureURL = "ms-appx:///Assets/alm.png", Description = descripcion, DateLost = DateTime.Now, IdCategory = "1", IdStatus = estado.ToString(), IdZone = "1", IdUser = "1", Name = "Noticia " + i.ToString(), NameZone = "Puriscal", QuantityComments = i, UserName = i.ToString(), UserPictureURL = "ms-appx:///Assets/user.png" });

            }

            lstvUserPosts.ItemsSource = lstNoticias;
        }

        private async void cargarDatos()
        {
            // se obtiene la referencia para buscar la información de usuario
            IMobileServiceTable<Usuario> userTable = App.MobileService.GetTable<Usuario>();

            // buscamos el usuario
            Usuario objUsuario = await userTable.LookupAsync(objNoticiaAux.IdUser);

            // establecemos el contexto para que cargue el header del list view
            lstvUserPosts.DataContext = objUsuario;

            // se cargan las noticias del usuario
            IEnumerable<NewCustom> lstUserPosts = await App.MobileService.InvokeApiAsync<IEnumerable<NewCustom>>("news_user", HttpMethod.Get, new Dictionary<string, string> { { "id", objUsuario.Id } });

            lstvUserPosts.ItemsSource = lstUserPosts;

            progressRing.IsActive = false;

        }

        private void AddCommentNew(object sender, TappedRoutedEventArgs e)
        {
            if (objNoticiaAux != null)
            {

                if (!this.Frame.Navigate(typeof(View.Comments), objNoticiaAux))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
        }

        private void NavigateNewDetail(object sender, TappedRoutedEventArgs e)
        {
            if (objNoticiaAux != null)
            {

                if (!this.Frame.Navigate(typeof(View.NewDetail), objNoticiaAux))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
        }

        private void OptionsNew(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }


        private void ReportNew(object sender, RoutedEventArgs e)
        {
            var a = objNoticiaAux;
        }

        private void EditNew(object sender, RoutedEventArgs e)
        {

        }
    }
}

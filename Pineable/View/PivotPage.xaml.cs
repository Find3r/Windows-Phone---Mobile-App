using Pineable.Common;
using Pineable.Data;
using Pineable.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Net.Http;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

// The Pivot Application template is documented at http://go.microsoft.com/fwlink/?LinkID=391641

namespace Pineable
{
    public sealed partial class PivotPage : Page
    {

        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");
       
        NewCustom objNoticiaAux;

        public PivotPage()
        {
            this.InitializeComponent();
 
            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.navigationHelper = new NavigationHelper(this);
            
        }

        private async void verificarConexion()
        {
          
            if (await App.CheckInternetConnection())
            {
                grdErrorLastNews.Visibility = Visibility.Collapsed;
                grdErrorCategories.Visibility = Visibility.Collapsed;
                grdErrorNotifications.Visibility = Visibility.Collapsed;
                lstvUserPosts.Visibility = Visibility.Visible;
                grdErrorMyProfile.Visibility = Visibility.Collapsed;
                //Hay conexión a Internet
                progressRing.IsActive = true;
                lstvUserPosts.ItemsSource = null;
                lstvUltimasNoticias.ItemsSource = null;

                cargarDatos();
                
            }
            else
            {
                //No hay conexión a Internet
                MessageDialog info = new MessageDialog("Verfique la conexión a Internet");
                await info.ShowAsync();
                //cargarDatosOffline();
                grdErrorLastNews.Visibility = Visibility.Visible;
                grdErrorCategories.Visibility = Visibility.Visible;
                grdErrorNotifications.Visibility = Visibility.Visible;
                lstvUserPosts.Visibility = Visibility.Collapsed;
                grdErrorMyProfile.Visibility = Visibility.Visible;
                progressRing.IsActive = false;
            }
            
        }

        private async void cargarDatos()
        {
            /*
            var culture = CultureInfo.CurrentCulture;
            if (!culture.IsNeutralCulture)
                culture = culture.Parent;
            var a = culture.TwoLetterISOLanguageName;
            */
            IMobileServiceTable<Categoria> categoryTable = App.MobileService.GetTable<Categoria>();     
            IMobileServiceTable<Notificacionusuario> notificationsTable = App.MobileService.GetTable<Notificacionusuario>();

            lstvUserPosts.DataContext = App.objUsuarioLogueado;

            // se cargan las últimas noticias
            IEnumerable<NewCustom> collectionLastNews = await App.MobileService.InvokeApiAsync<IEnumerable<NewCustom>>("last_news",HttpMethod.Get,null);
            lstvUltimasNoticias.ItemsSource = collectionLastNews;

            // se cargan las categorías
            IEnumerable<Categoria> collectionCategories = await categoryTable.OrderBy(e => e.Name).ToEnumerableAsync();
            grdvAreas.ItemsSource = collectionCategories;

            // notificaciones
            IEnumerable<Notificacionusuario> collectionNotifications = await notificationsTable.Where(e => e.IdUser == App.objUsuarioLogueado.Id).OrderBy(e => e.DateCreated).ToEnumerableAsync();
            lstvNotificaciones.ItemsSource = collectionNotifications;

            // se cargan las noticias del usuario
            IEnumerable<NewCustom> collectionUserNews = await App.MobileService.InvokeApiAsync<IEnumerable<NewCustom>>("news_user", HttpMethod.Get, new Dictionary<string, string> { { "id", App.objUsuarioLogueado.Id } });
            lstvUserPosts.ItemsSource = collectionUserNews;

            // se cargan las noticias que un usuario sigue
            IEnumerable<NewCustom> collectionUserFollowing = await App.MobileService.InvokeApiAsync<IEnumerable<NewCustom>>("following_news", HttpMethod.Get, new Dictionary<string, string> { { "iduser", App.objUsuarioLogueado.Id } });
            lstvUserFollowingPosts.ItemsSource = collectionUserFollowing;

            // se verifica si se tiene que desplegar el layout con el mensaje de error

            // últimas noticias
            if (collectionLastNews.Count() == 0)
            {
                grdErrorLastNews.Visibility = Visibility.Visible;
            }
            else
            {
                grdErrorLastNews.Visibility = Visibility.Collapsed;
            }

            // categorías
            if (collectionCategories.Count() == 0)
            {
                grdErrorCategories.Visibility = Visibility.Visible;
            }
            else
            {
                grdErrorCategories.Visibility = Visibility.Collapsed;
            }

            // notificaciones
            if (collectionNotifications.Count() == 0)
            {
                grdErrorNotifications.Visibility = Visibility.Visible;
            }
            else
            {
                grdErrorNotifications.Visibility = Visibility.Collapsed;
            }

            // mi perfil
            if (collectionUserNews.Count() == 0)
            {
                grdErrorMyProfile.Visibility = Visibility.Visible;
            }
            else
            {
                grdErrorMyProfile.Visibility = Visibility.Collapsed;
            }

            // following
            if (collectionUserFollowing.Count() == 0)
            {
                grdErrorMyFollowing.Visibility = Visibility.Visible;
            }
            else
            {
                grdErrorMyFollowing.Visibility = Visibility.Collapsed;
            }

            progressRing.IsActive = false;
        }

        #region offline
        private void cargarDatosOffline()
        {
            App.objUsuarioLogueado.Id = "1";
            App.objUsuarioLogueado.IdCountry = "1";
            App.objUsuarioLogueado.Name = "Carlos Castro Brenes";
            App.objUsuarioLogueado.PictureUrl = "ms-appx:///Assets/user.png";
            App.objUsuarioLogueado.CoverPicture = "ms-appx:///Assets/Hawaiio.jpg";

            lstvUserPosts.DataContext = App.objUsuarioLogueado;


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

                lstNoticias.Add(new NewCustom() { Id = "1", PictureURL = "ms-appx:///Assets/alm.png", Description = descripcion, DateLost = DateTime.Now, IdCategory = "1", IdStatus = estado.ToString(), IdZone = "1", IdUser = "1", Name = "Noticia " + i.ToString(), NameZone = "Puriscal", QuantityComments = i, UserName = i.ToString(),UserPictureURL = "ms-appx:///Assets/user.png" });

            }

            List<Categoria> lstCategorias = new List<Categoria>();
            for (int i = 0; i < 5; i++)
            {
                lstCategorias.Add(new Categoria() { Id = i.ToString(), Name = i.ToString(), PictureURL = "ms-appx:///Assets/car.png" });
            }

            grdvAreas.ItemsSource = lstCategorias;

            // notificaciones
            List<Notificacionusuario> lstNotifications = new List<Notificacionusuario>();

            for (int i = 0; i < 15; i++)
            {
                lstNotifications.Add(new Notificacionusuario() { Id = i.ToString(), DateCreated = DateTime.Now, IdNew = i.ToString(), IdUser = i.ToString(), StatusRead = false, Description = "Texto " + i.ToString() });
            }
            lstvNotificaciones.ItemsSource = lstNotifications;


            //lstvMisNoticias.ItemsSource = lstNoticias;

            
            lstvUserPosts.ItemsSource = lstNoticias;
            lstvUltimasNoticias.ItemsSource = lstNoticias;


        }
        #endregion

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        #region NavigationHelper registration
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
           
            if (this.Frame.CanGoBack)
            {
                this.Frame.BackStack.RemoveAt(0);

            }

            try
            {
                if (App.FIRST_TIME)
                {
                    
                    if (await App.CheckInternetConnection())
                    {
                        progressRing.IsActive = true;
                        //Wifi or Cellular
                        App.objUsuarioLogueado.Id = e.Parameter as string;
                        await loadUserInformation();
                    }
                    else
                    {
                        // No internet
                        //MessageDialog info = new MessageDialog("Verfique la conexión a Internet");
                        //await info.ShowAsync();

                    }
                    
                }

                if (App.REFRESH_ITEMS)
                {
                   
                    verificarConexion();
                    App.REFRESH_ITEMS = false;
                    
                }
               
            }
            catch (Exception a)
            {

               
            }
            
        }

        private async Task loadUserInformation()
        {
            // verificamos si no tiene nada asignado
            if (App.objUsuarioLogueado.Name == null)
            {
                App.FIRST_TIME = false;
                FacebookUser facebookUser;
                try
                {
                    // se consulta la api   
                    facebookUser = await App.MobileService.InvokeApiAsync<FacebookUser>("userlogin", HttpMethod.Get, null);
                }
                catch (Exception e)
                {

                    throw;
                }
               

                // establecemos nuestro objeto usuario que utilizaremos
                App.objUsuarioLogueado.Name = facebookUser.Name;
                App.objUsuarioLogueado.PictureUrl = "http://graph.facebook.com/" + facebookUser.Id + "/picture?type=large";
                
                if (facebookUser.PictureCoverURL == null)
                {
                    App.objUsuarioLogueado.CoverPicture = "http://www.solosfondi.com/wp-content/uploads/2011/05/wallpaper-hakuna-matata.jpg";
                }
                else
                {
                    App.objUsuarioLogueado.CoverPicture = facebookUser.PictureCoverURL.PictureUrl;
                }

                // obtenemos referencia a la tabla usuarios
                IMobileServiceTable <Usuario> tableUsuario = App.MobileService.GetTable<Model.Usuario>();
                try
                {
                    Usuario objUsuario = await tableUsuario.LookupAsync(App.objUsuarioLogueado.Id);
                    await tableUsuario.UpdateAsync(App.objUsuarioLogueado);
                    App.objUsuarioLogueado = objUsuario;
                }
                catch (Exception a)
                {

                    // si es así entonces lo insertamos en la base de datos
                    await tableUsuario.InsertAsync(App.objUsuarioLogueado);
                }
            }
           
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void lstvUltimasNoticias_ItemClick(object sender, ItemClickEventArgs e)
        {
            objNoticiaAux = e.ClickedItem as NewCustom;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            verificarConexion();
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

        private void grdvAreas_ItemClick(object sender, ItemClickEventArgs e)
        {
            Categoria objCategory = e.ClickedItem as Categoria;

            if (!this.Frame.Navigate(typeof(NewsCategory), objCategory))
            {
                throw new Exception("Failed to create initial page");
            }
        }

        private void NavigateUserProfile(object sender, TappedRoutedEventArgs e)
        {
            if (objNoticiaAux != null)
            {

                if (!this.Frame.Navigate(typeof(View.UserProfile), objNoticiaAux))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
        }

        private void NavigateCommentsNew(object sender, ItemClickEventArgs e)
        {
            Notificacionusuario objNotification = e.ClickedItem as Notificacionusuario;

            if (objNotification != null)
            {


                if (!this.Frame.Navigate(typeof(View.Comments), new NewCustom() { Id = objNotification.IdNew}))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
        }

        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            if (!this.Frame.Navigate(typeof(View.AddNew)))
            {
                throw new Exception("Failed to create initial page");
            }
        }

        private async void btnSignOut_Click(object sender, RoutedEventArgs e)
        {
            if(App.SignOut())
            {
                if (!this.Frame.Navigate(typeof(View.Login)))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
            else
            {
                MessageDialog info = new MessageDialog("Error al cerrar la sesión");
                await info.ShowAsync();
            }
        }
    }
}

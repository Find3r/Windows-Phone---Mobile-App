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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Pivot Application template is documented at http://go.microsoft.com/fwlink/?LinkID=391641

namespace Pineable
{
    public sealed partial class PivotPage : Page
    {
        User objUsuarioLogueado;
        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        NewCustom objNoticiaAux;

        public PivotPage()
        {
            this.InitializeComponent();
            objUsuarioLogueado = new User();
            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        private  void verificarConexion()
        {
            cargarDatosOffline();

            /*
            if (App.NetworkAvailable)
            {
                //Hay conexión a Internet
                progressRing.IsActive = true;

                lstvUserPosts.ItemsSource = null;
                lstvUltimasNoticias.ItemsSource = null;

               // cargarDatos();
                //progressRing.IsActive = false;

            }
            else
            {
                //No hay conexión a Internet
                //MessageDialog info = new MessageDialog("Verfique la conexión a Internet");
                //await info.ShowAsync();
                
            }
            */
        }

        private void cargarDatosOffline()
        {
            objUsuarioLogueado.Id = "1";
            objUsuarioLogueado.IdCountry = "1";
            objUsuarioLogueado.Name = "Carlos Castro Brenes";
            objUsuarioLogueado.PictureUrl = "ms-appx:///Assets/user.png";
            objUsuarioLogueado.CoverPicture = "ms-appx:///Assets/Hawaiio.jpg";


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


            txtNombreUsuario.Text = objUsuarioLogueado.Name;
            txtDireccionUsuario.Text = "Costa Rica";

            List<Category> lstCategorias = new List<Category>();
            for (int i = 0; i < 5; i++)
            {
                lstCategorias.Add(new Category() { Id = i.ToString(), Name = i.ToString(), PictureURL = "ms-appx:///Assets/car.png" });
            }

            grdvAreas.ItemsSource = lstCategorias;

            // notificaciones
            List<UserNotification> lstNotifications = new List<UserNotification>();

            for (int i = 0; i < 15; i++)
            {
                lstNotifications.Add(new UserNotification() { Id = i.ToString(), DateCreated = DateTime.Now, IdNew = i.ToString(), IdUser = i.ToString(), StatusRead = false, Description = "Texto " + i.ToString() });
            }
            lstvNotificaciones.ItemsSource = lstNotifications;


            //lstvMisNoticias.ItemsSource = lstNoticias;

            if (lstNoticias.Count == 0)
            {
                txtMiSeguimiento.Visibility = Visibility.Visible;
                txtMiSeguimiento.Text = "Aún no sigues ninguna noticia";
            }
            else
            {
                txtMiSeguimiento.Visibility = Visibility.Collapsed;

            }

            if (lstNoticias.Count == 0)
            {
                txtComentarios.Visibility = Visibility.Visible;
                txtComentarios.Text = "El usuario aún no tiene reportes";
            }
            else
            {
                txtComentarios.Visibility = Visibility.Collapsed;

            }
            lstvUserPosts.ItemsSource = lstNoticias;
            lstvUltimasNoticias.ItemsSource = lstNoticias;


        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>.
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            var sampleDataGroup = await SampleDataSource.GetGroupAsync("Group-1");
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache. Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/>.</param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            // TODO: Save the unique state of the page here.
        }

        /// <summary>
        /// Adds an item to the list when the app bar button is clicked.
        /// </summary>
       

        /// <summary>
        /// Invoked when an item within a section is clicked.
        /// </summary>
        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
            if (!Frame.Navigate(typeof(ItemPage), itemId))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        /// <summary>
        /// Loads the content for the second pivot item when it is scrolled into view.
        /// </summary>
        private async void SecondPivot_Loaded(object sender, RoutedEventArgs e)
        {
            var sampleDataGroup = await SampleDataSource.GetGroupAsync("Group-2");
           
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
            objUsuarioLogueado = new User();

            if (this.Frame.CanGoBack)
            {
                this.Frame.BackStack.RemoveAt(0);

            }
            //objUsuarioLogueado.Id = e.Parameter as string;
            verificarConexion();

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void lstvUltimasNoticias_ItemClick(object sender, ItemClickEventArgs e)
        {

            objNoticiaAux = e.ClickedItem as NewCustom;
   

        }

        private void lstvUserPosts_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {

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

    }
}

using Pineable.Common;
using Pineable.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Pineable
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewsCategory : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        Categoria OBJ_CATEGORY;
        NewCustom objNoticiaAux;

        public NewsCategory()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            
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
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
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

            Categoria objCategory = e.Parameter as Categoria;

            if (objCategory != null)
            {
                OBJ_CATEGORY = objCategory;
                verificarConexion();

            }
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

        private async void cargarDatos()
        {
            // se establece el título
            txtTitle.Text = OBJ_CATEGORY.Name;

            // se cargan las noticias de una categoría
            IEnumerable<NewCustom> collectionCategoryNews = await App.MobileService.InvokeApiAsync<IEnumerable<NewCustom>>("news_category", HttpMethod.Get, new Dictionary<string,string> { { "id", OBJ_CATEGORY.Id } });
            lstvUltimasNoticias.ItemsSource = collectionCategoryNews;

            if (collectionCategoryNews.Count() == 0)
            {
                grdErrorCategoryNews.Visibility = Visibility.Visible;
            }
            else
            {
                grdErrorCategoryNews.Visibility = Visibility.Collapsed;
            }

            progressRing.IsActive = false;
        }

        private void cargarDatosOffline()
        {

            txtTitle.Text = OBJ_CATEGORY.Name;

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

                //lstNoticias.Add(new NewCustom() { Id = "1", PictureURL = "ms-appx:///Assets/alm.png", Description = descripcion, DateLost = DateTime.Now, IdCategory = "1", IdStatus = estado.ToString(), IdZone = "1", IdUser = "1", Name = "Noticia " + i.ToString(), NameZone = "Puriscal", QuantityComments = i, UserName = i.ToString(), UserPictureURL = "ms-appx:///Assets/user.png" });

            }

            lstvUltimasNoticias.ItemsSource = lstNoticias;

            progressRing.IsActive = false;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

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

        private void lstvUltimasNoticias_ItemClick(object sender, ItemClickEventArgs e)
        {
            objNoticiaAux = e.ClickedItem as NewCustom;
        }

        private void ReportNew(object sender, RoutedEventArgs e)
        {

        }

        private void EditNew(object sender, RoutedEventArgs e)
        {

        }

        private void OptionsNew(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            verificarConexion();
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
    }
}

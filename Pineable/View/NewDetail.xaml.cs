using Pineable.Common;
using Pineable.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

namespace Pineable.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewDetail : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private NewCustom OBJ_NOTICIA;

        public NewDetail()
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
            NewCustom objNoticia = e.Parameter as NewCustom;

            if (objNoticia != null)
            {
                OBJ_NOTICIA = objNoticia;
                verificarConexion();

            }
        }

        private async void verificarConexion()
        {
            cargarDatos();

            if (App.NetworkAvailable)
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
            }
        }

        private void cargarDatos()
        {
           
            txtvNombre.Text = OBJ_NOTICIA.Name;
            txtDescripcion.Text = OBJ_NOTICIA.Description;

     
            Uri uri;
            imgvNew.Source = Helper.Helper.cargarImagen(OBJ_NOTICIA.PictureURL, UriKind.Absolute);

            if (OBJ_NOTICIA.IdStatus.Equals("0"))
            {
                uri = new Uri("ms-appx:///Assets/lost.png", UriKind.Absolute);
                txtStatus.Text = "Desaparecido(a)";
            }
            else
            {
                uri = new Uri("ms-appx:///Assets/found.png", UriKind.Absolute);
                txtStatus.Text = "Encontrado(a)";
            }

            imgvStatus.Source = Helper.Helper.cargarImagen(uri, UriKind.Absolute);

            progressRing.IsActive = false;

        }

        

        #endregion

        private void imgvComentar_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void Comment(object sender, TappedRoutedEventArgs e)
        {
            if (!this.Frame.Navigate(typeof(View.NewDetail)))
            {
                throw new Exception("Failed to create initial page");
            }
        }

 
    }
}

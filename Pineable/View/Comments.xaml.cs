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
    public sealed partial class Comments : Page
    {
        NewCustom OBJ_NOTICIA;
        List<CommentCustom> lstComentarios = new List<CommentCustom> ();

        public Comments()
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
            NewCustom objNoticia = e.Parameter as NewCustom;

            if (objNoticia != null)
            {
                OBJ_NOTICIA = objNoticia;
                verificarConexion();

            }
        }

        private async void verificarConexion()
        {
           

            if (App.NetworkAvailable)
            {
                //Hay conexión a Internet
                progressRing.IsActive = true;
                cargarDatos();
                
               

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
            // se cargan las noticias de una categoría
            IEnumerable<CommentCustom> lstComments = await App.MobileService.InvokeApiAsync<IEnumerable<CommentCustom>>("postcomments", HttpMethod.Get, new Dictionary<string, string> { { "id", OBJ_NOTICIA.Id } });
            lstvComentarios.ItemsSource = lstComments;

            progressRing.IsActive = false;
        }

        private void cargarDatosOffline()
        {
             lstComentarios = new List<CommentCustom>();

            for (int i = 0; i < 15; i++)
            {
                lstComentarios.Add(new CommentCustom() {Id = i.ToString(),Date = DateTime.Now,IdNew = "1",IdUser = "1", UserName = "Nombre usuario" , Description = "Ejemplo Comentario", UserPictureURL = "ms-appx:///Assets/user.png" });

            }

            lstvComentarios.ItemsSource = lstComentarios;

            progressRing.IsActive = false;
        }

        private async void btnAddComment_Click(object sender, RoutedEventArgs e)
        {
            // se verifica si hay contenido en el textblock
            if (txtComment.Text.Trim().Equals(""))
            {
                MessageDialog info = new MessageDialog("Debe ingresar contenido en el comentario");
                await info.ShowAsync();
            }
            else
            {
                IMobileServiceTable<Comentario> commentTable = App.MobileService.GetTable<Comentario>();

                // mostramos la barra de carga
                progressRing.IsActive = true;

                //IMobileServiceTable<Comentario> tableComentario = App.MobileService.GetTable<Comentario>();
                Comentario objComentarioNuevo = new Comentario();

                // establecemos el id de la noticia
                objComentarioNuevo.IdNew = OBJ_NOTICIA.Id;

                // establecemos el id del usuario, en este caso obtenemos este id mediante el que se encuentra autenticado
                objComentarioNuevo.IdUser = App.objUsuarioLogueado.Id;

                // establecemos la descripción
                objComentarioNuevo.Description = txtComment.Text.Trim();


                // obtenmos la fecha
                objComentarioNuevo.Date = DateTime.Now;

                // agregamos el comentario
                try
                {
                    await commentTable.InsertAsync(objComentarioNuevo);

                    // limpiamos el textbox
                    txtComment.Text = "";

                    verificarConexion();
                }
                catch (Exception)
                {
                    MessageDialog info = new MessageDialog("Vaya ha ocurrido un error al agregar el comentario");
                    await info.ShowAsync();

                }

                // ocultamos la barra de carga
                progressRing.IsActive = false;
            }
            
        }

        private void NavigateUserProfile(object sender, TappedRoutedEventArgs e)
        {
            if (OBJ_NOTICIA != null)
            {

                if (!this.Frame.Navigate(typeof(View.UserProfile), OBJ_NOTICIA))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
        }
    }
}

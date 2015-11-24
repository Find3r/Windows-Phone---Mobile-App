using Microsoft.WindowsAzure.MobileServices;
using Pineable.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public sealed partial class ReportNew : Page
    {
        NewCustom OBJ_NOTICIA;
        public ReportNew()
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
            // obtenemos el objeto noticia que se nos envía y lo guardamos
            OBJ_NOTICIA = e.Parameter as NewCustom;
        }

        private async void btnReport_Tapped(object sender, TappedRoutedEventArgs e)
        {
           
            string descriptionReport = txtReport.Text;
            string message = "";
            bool statusReport = false;

            // se verifica si hay contenido
            if (!String.IsNullOrEmpty(descriptionReport))
            {
                Report report = new Report();
                IMobileServiceTable<Report> tableReport = App.MobileService.GetTable<Report>();

                // se establecen los datos del reporte
                report.Description = descriptionReport;
                report.IdNew = OBJ_NOTICIA.Id;
                report.IdUser = App.objUsuarioLogueado.Id;

                try
                {
                    // se inserta el registro
                    await tableReport.InsertAsync(report);
                    message = "Reporte realizado exitosamente";
                    statusReport = true;
                }
                catch (Exception)
                {

                    message = "Error al insertar el reporte, intente de nuevo";
                }
               
            }
            else
            {
                message = "Debe ingresar la descripción";
            }

            MessageDialog info = new MessageDialog(message);
            await info.ShowAsync();

            // verificamos el estado, si es true quiere decir que se insertó
            if(statusReport)
            {
                // navegamos al pivot
                if (!this.Frame.Navigate(typeof(PivotPage)))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
        }
    }
}

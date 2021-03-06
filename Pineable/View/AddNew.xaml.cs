﻿using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using Pineable.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
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
    public sealed partial class AddNew : Page
    {
        CoreApplicationView view;
        string ImagePath;
        CustomImage customImage = new CustomImage();
        NewCustom OBJ_NOTICIA;

        public AddNew()
        {
            this.InitializeComponent();
            view = CoreApplication.GetCurrentView();

            cboCategorias.SelectedIndex = 0;
            cboEstado.SelectedIndex = 0;
            cboLugar.SelectedIndex = 0;
            cboTipo.SelectedIndex = 0;

            // se establece la fecha mínima
            DateTime date = new DateTime(2000,1,1);
            
            dtpFecha.MinYear = date;

            // se establece la fecha máxima
            dtpFecha.MaxYear = DateTime.Now;

          
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            OBJ_NOTICIA = e.Parameter as NewCustom;
            if (OBJ_NOTICIA != null)
            {
                btnAgregarNoticia.Content = "Actualizar";
                LoadNewInformation();
                
            }
        }

        private void LoadNewInformation()
        {
            txtNombre.Text = OBJ_NOTICIA.Name;
            txtDescripcion.Text = OBJ_NOTICIA.Description;
            dtpFecha.Date = OBJ_NOTICIA.DateLost;

            int idAux = 0;
            int.TryParse(OBJ_NOTICIA.IdCategory, out idAux);

            cboCategorias.SelectedIndex = idAux;

            int.TryParse(OBJ_NOTICIA.IdStatus, out idAux);

            cboTipo.SelectedIndex = idAux;

            ObservableCollection<string> collection = new ObservableCollection<string>() {"Pendiente","Resuelto" };

            cboEstado.ItemsSource = collection;

            
            cboEstado.SelectedIndex = OBJ_NOTICIA.StatusId;


        }

        private async void btnAgregarNoticia_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // se verifica si hay conexión a internet
            if (await App.CheckInternetConnection())
            {
                string nombre = txtNombre.Text.Trim();
                string descripcion = txtDescripcion.Text.Trim();

                Noticia objNew = new Noticia();

                if (OBJ_NOTICIA != null)
                {
                    objNew = OBJ_NOTICIA;
                }

                // verificamos que se haya ingresado los datos
                if (String.IsNullOrEmpty(nombre) || String.IsNullOrEmpty(descripcion) || String.IsNullOrEmpty(customImage.FileName) && String.IsNullOrEmpty(objNew.PictureURL))
                {
                    MessageDialog info = new MessageDialog("Debe completar todos los datos");
                    await info.ShowAsync();
                }
                else if (dtpFecha.Date > DateTime.Now)
                {
                    MessageDialog info = new MessageDialog("Verifique la fecha seleccionada, no puede ser mayor a la actual");
                    await info.ShowAsync();
                }
                else
                {


                    objNew.IdUser = App.objUsuarioLogueado.Id;
                    objNew.Name = nombre;
                    objNew.Description = descripcion;

                    // si no está vacío el campo de la imagen es porque va subir una
                    if(!String.IsNullOrEmpty(customImage.FileName))
                    {

                        objNew.PictureURL = "https://purisinfo.blob.core.windows.net/img/" + customImage.FileName.Trim();

                        uploadImage(customImage);
                    }

                    objNew.IdZone = (cboLugar.SelectedIndex + 1).ToString();

                    objNew.DateLost = dtpFecha.Date.DateTime;
                    objNew.IdCategory = (cboCategorias.SelectedIndex + 1).ToString();
                    objNew.IdStatus = cboTipo.SelectedIndex.ToString();
                    objNew.Solved = cboEstado.SelectedIndex == 0 ? false : true;
                    

                    IMobileServiceTable<Noticia> newTable = App.MobileService.GetTable<Noticia>();

                    string mensaje = "";
                    bool error = false;

                    try
                    {
                        // verificamos si se debe insertar
                        if (OBJ_NOTICIA == null)
                        {
                            await newTable.InsertAsync(objNew);
                            mensaje = "Noticia agregada";
                        }
                        else
                        {
                            await newTable.UpdateAsync(objNew);
                            mensaje = "Noticia actualizada";
                        }

                    }
                    catch (Exception ea)
                    {
                        mensaje = "Error al procesar la acción";
                        error = true;
                    }

                    MessageDialog info = new MessageDialog(mensaje);
                    await info.ShowAsync();

                    if(!error)
                    {
                        // indicamos que debe actualizar
                        App.REFRESH_ITEMS = true;

                        if (!this.Frame.Navigate(typeof(PivotPage)))
                        {

                        }
                    }
                   
                }
            }
            else
	        {

                MessageDialog info = new MessageDialog("Verifique la conexión a internet");
                await info.ShowAsync();

            }
        }

        private async void View_Activated(CoreApplicationView sender, IActivatedEventArgs pargs)
        {
            FileOpenPickerContinuationEventArgs args = pargs as FileOpenPickerContinuationEventArgs;

            if (args != null)
            {
                if (args.Files.Count == 0)
                    return;


                view.Activated -= View_Activated;
                StorageFile storageFile = args.Files[0];

                var stream = await storageFile.OpenAsync(Windows.Storage.FileAccessMode.Read);
                
                var bitmapImage = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
                await bitmapImage.SetSourceAsync(stream);

                var decoder = await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(stream);

                imgvSeleccionarImagen.Source = bitmapImage;

                // image to byte []
                var reader = new Windows.Storage.Streams.DataReader(stream.GetInputStreamAt(0));

                await reader.LoadAsync((uint)stream.Size);

                byte[] pixels = new byte[stream.Size];

                reader.ReadBytes(pixels);

                customImage = new CustomImage();
                customImage.FileBytes = pixels;
                customImage.FileName = Guid.NewGuid() + storageFile.FileType;

               
            }

        }

        private  void uploadImage(CustomImage pCustomImage)
        {
            string urlAPI = "http://pineapple-api.azurewebsites.net/api/";

            HttpClient ApiClient = new HttpClient();
            ApiClient.BaseAddress = new Uri(urlAPI);
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var json_object = JsonConvert.SerializeObject(pCustomImage);

            var response =  ApiClient.PostAsync("UploadImage", new StringContent(json_object.ToString(), Encoding.UTF8, "application/json"));
        }

        private void imgvSeleccionarImagen_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ImagePath = string.Empty;
            FileOpenPicker filePicker = new FileOpenPicker();
            filePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            filePicker.ViewMode = PickerViewMode.Thumbnail;


            // Filter to include a sample subset of file types 
            filePicker.FileTypeFilter.Clear();
            filePicker.FileTypeFilter.Add(".bmp");
            filePicker.FileTypeFilter.Add(".png");
            filePicker.FileTypeFilter.Add(".jpeg");
            filePicker.FileTypeFilter.Add(".jpg");


            filePicker.PickSingleFileAndContinue();
            view.Activated += View_Activated;
        }

        
    }
}

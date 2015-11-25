
using Microsoft.WindowsAzure.MobileServices;
using Pineable.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Pineable.API
{
    public class APIConnection
    {
     
        // carga las noticias recientes
        public async Task<ObservableCollection<NewCustom>> GETLastNews()
        {
            return await App.MobileService.InvokeApiAsync<ObservableCollection<NewCustom>>("last_newsaux", HttpMethod.Get, new Dictionary<string, string> { { "id", App.objUsuarioLogueado.Id } });
        }

        // carga las categorías
        public async Task<ObservableCollection<Categoria>> GETCategories()
        {
            IMobileServiceTable<Categoria> categoryTable = App.MobileService.GetTable<Categoria>();
            return await categoryTable.OrderBy(e => e.Name).ToCollectionAsync();
        }

        // carga las notificaciones que me corresponden
        public async Task<ObservableCollection<Notificacionusuario>> GETMyNotifications()
        {
            IMobileServiceTable<Notificacionusuario> notificationsTable = App.MobileService.GetTable<Notificacionusuario>();
            return await notificationsTable.Where(e => e.IdUser == App.objUsuarioLogueado.Id).OrderBy(e => e.DateCreated).ToCollectionAsync();
        }

        // carga mis posts
        public async Task<ObservableCollection<NewCustom>> GETMyPosts()
        {
            return await App.MobileService.InvokeApiAsync<ObservableCollection<NewCustom>>("my_news", HttpMethod.Get, new Dictionary<string, string> { { "id", App.objUsuarioLogueado.Id } });
        }

        // carga los post que estoy siguiendo
        public async Task<ObservableCollection<NewCustom>> GETMyFollowingPosts()
        {
            return await App.MobileService.InvokeApiAsync<ObservableCollection<NewCustom>>("following_news", HttpMethod.Get, new Dictionary<string, string> { { "iduser", App.objUsuarioLogueado.Id } });
        }

        // actualiza una noticia
        public void PUTPost(NewCustom pNoticia)
        {
            try
            {
                IMobileServiceTable<Noticia_usuario> tableUserNew = App.MobileService.GetTable<Noticia_usuario>();

                Noticia_usuario noticiaUsuario = new Noticia_usuario();

                noticiaUsuario.IdNoticia = pNoticia.Id;
                noticiaUsuario.IdUsuario = pNoticia.IdUser;
                noticiaUsuario.Estado = pNoticia.StatusFollow;

                // no se aplica await ya que no devuelve resultado
                tableUserNew.InsertAsync(noticiaUsuario);
                
            }
            catch (Exception asa)
            {

               
            }
           
        }

    }
}

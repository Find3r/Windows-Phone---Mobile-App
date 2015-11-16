using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;

// http://go.microsoft.com/fwlink/?LinkId=290986&clcid=0x409

namespace Pineable
{
    internal class wantedappPush
    {
        public async static Task UploadChannel()
        {
            var channel = await Windows.Networking.PushNotifications.PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            channel.PushNotificationReceived += Channel_PushNotificationReceived;
            try
            {
                List<string> tags = new List<string>();
                tags.Add(App.objUsuarioLogueado.Id);
                tags.Add("All");
                
                await App.MobileService.GetPush().RegisterNativeAsync(channel.Uri, tags);    

               
                /*
                await App.wantedappClient.InvokeApiAsync("notifyAllUsers",
                    new JObject(new JProperty("toast", "Sample Toast")));
                    */
            }
            catch (Exception exception)
            {
                HandleRegisterException(exception);
            }
        }

        private static void Channel_PushNotificationReceived(Windows.Networking.PushNotifications.PushNotificationChannel sender, Windows.Networking.PushNotifications.PushNotificationReceivedEventArgs args)
        {
            
        }

        private static void HandleRegisterException(Exception exception)
        {

        }
    }
}

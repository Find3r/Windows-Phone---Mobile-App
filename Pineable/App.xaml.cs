using Microsoft.WindowsAzure.MobileServices;
using Pineable.Common;
using Pineable.Model;
using Pineable.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Connectivity;
using Windows.Phone.UI.Input;
using Windows.Security.Credentials;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Pivot Application template is documented at http://go.microsoft.com/fwlink/?LinkID=391641

namespace Pineable
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        // http://go.microsoft.com/fwlink/?LinkId=290986&clcid=0x409
        public static Microsoft.WindowsAzure.MobileServices.MobileServiceClient wantedappClient = new Microsoft.WindowsAzure.MobileServices.MobileServiceClient(
        "https://wantedapp.azure-mobile.net/",
        "MIqlLCMyhKNIonsgsNuFlpBXzqqNWj11");


        // http://go.microsoft.com/fwlink/?LinkId=290986&clcid=0x409


        private TransitionCollection transitions;
        private MobileServiceUser user;

        public static bool FIRST_TIME = true;
        public static bool REFRESH_ITEMS = true;

        public static Usuario objUsuarioLogueado = new Usuario();
        public static MobileServiceClient MobileService = new MobileServiceClient(
            "https://wantedapp.azure-mobile.net/",
            "MIqlLCMyhKNIonsgsNuFlpBXzqqNWj11"
        );

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;


            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            
        }
        
        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            Frame frame = Window.Current.Content as Frame;
            if (frame == null)
            {
                return;
            }

            if (frame.CanGoBack)
            {
                frame.GoBack();
                e.Handled = true;
            }
        }

        public async static Task<bool> CheckInternetConnection()
        {
            return await Task.Run(() => NetworkInterface.GetIsNetworkAvailable());
        }


        protected override void OnActivated(IActivatedEventArgs args)
        {
            // Windows Phone 8.1 requires you to handle the respose from the WebAuthenticationBroker.
#if WINDOWS_PHONE_APP
            if (args.Kind == ActivationKind.WebAuthenticationBrokerContinuation)
            {
                // Completes the sign-in process started by LoginAsync.
                // Change 'MobileService' to the name of your MobileServiceClient instance. 
                App.MobileService.LoginComplete(args as WebAuthenticationBrokerContinuationEventArgs);
            }
#endif

            base.OnActivated(args);
        }
        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active.
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page.
                rootFrame = new Frame();

                // Associate the frame with a SuspensionManager key.
                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

                // TODO: Change this value to a cache size that is appropriate for your application.
                rootFrame.CacheSize = 1;

                // Set the default language
                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Restore the saved session state only when appropriate.
                    try
                    {
                        await SuspensionManager.RestoreAsync();
                    }
                    catch (SuspensionManagerException)
                    {
                        // Something went wrong restoring state.
                        // Assume there is no state and continue.
                    }
                }

                // Place the frame in the current Window.
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter.

                REFRESH_ITEMS = true;

                // verificamos si existe la credencial almacenada
                if (Authenticate())
                {
                    // si es así enviamos al pivot
                    if (!rootFrame.Navigate(typeof(PivotPage), Helper.Helper.GetUserId(user.UserId)))
                    {
                        //throw new Exception("Failed to create initial page");
                    }
                }
                else
                {
                    // en caso que no existe entonces enviamos a la ventana de login
                    if (!rootFrame.Navigate(typeof(Login)))
                    {
                        //throw new Exception("Failed to create initial page");
                    }
                }
            }

            // Ensure the current window is active.
            Window.Current.Activate();
            // http://go.microsoft.com/fwlink/?LinkId=290986&clcid=0x409

            // se verifica la conexión a internet
            if (await CheckInternetConnection())
            {
                // si hay entonces se intenta actualizar el canal de push notifications
                await Pineable.wantedappPush.UploadChannel();
            }

           
        }

        private bool Authenticate()
        {
            // Use the PasswordVault to securely store and access credentials.
            PasswordVault vault = new PasswordVault();
            PasswordCredential credential = null;
            
            try
            {
                // Try to get an existing credential from the vault.
                credential = vault.FindAllByResource("Facebook").First();

                //vault.Remove(credential);
                
            }
            catch (Exception a)
            {
                // When there is no matching resource an error occurs, which we ignore.
            }

            if (credential != null)
            {
                // Create a user from the stored credentials.
                user = new MobileServiceUser(credential.UserName);
                credential.RetrievePassword();
                user.MobileServiceAuthenticationToken = credential.Password;

                // Set the user from the stored credentials.
                MobileService.CurrentUser = user;

                return true;
            }
            else
            {
                return false;
            }

        }

        public static bool SignOut()
        {
            // Use the PasswordVault to securely store and access credentials.
            PasswordVault vault = new PasswordVault();
            PasswordCredential credential = null;

            try
            {
                // Try to get an existing credential from the vault.
                credential = vault.FindAllByResource("Facebook").First();

                vault.Remove(credential);

                MobileService.Logout();
                return true;
            }
            catch (Exception a)
            {
                // When there is no matching resource an error occurs, which we ignore.

                return false;
            }

           

        }

        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }


    }
}

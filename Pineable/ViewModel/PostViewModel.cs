using Pineable.API;
using Pineable.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Pineable.ViewModel
{
    public class PostViewModel : NotificationEnabledObject
    {
        private ObservableCollection<NewCustom> _collectionPosts = new ObservableCollection<NewCustom>();

        public  PostViewModel()
        {
            _collectionPosts = new ObservableCollection<NewCustom>();
            _collectionPosts.CollectionChanged += (sender, e) =>
            {
                OnPropertyChanged();
            };

        }

        private void _collectionPosts_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged();
        }

        public ObservableCollection<NewCustom> CollectionPosts
        {
            get
            {
                if (_collectionPosts == null)
                {
                    _collectionPosts = new ObservableCollection<NewCustom>();
                }

                // esto es para agregar elementos y tener blendability, si se está en tiempo de diseño entonces se muestren datos de ejemplo
                if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                {
                   string descripcion = "Bacon ipsum dolor amet swine ham hock drumstick tail. Meatloaf jowl andouille jerky salami pork belly alcatra frankfurter prosciutto kevin.Tongue corned beef kielbasa salami t-bone, rump shoulder meatball pork loin cupim. Andouille ham flank pork shankle ham hock short loin rump salami tenderloin biltong." +

"Frankfurter andouille biltong ball tip filet mignon, sirloin turducken swine t-bone shank pork chop pastrami. Shoulder porchetta tenderloin brisket, beef ribs turkey ham pork flank alcatra ground round. Doner meatball kevin swine t - bone cupim picanha. Strip steak filet mignon ribeye, fatback pig rump pancetta. Chicken capicola drumstick doner rump tail frankfurter jowl turducken swine pastrami sausage t - bone alcatra.Capicola short loin biltong picanha doner hamburger shoulder jerky meatloaf short ribs. Boudin pork belly alcatra strip steak salami ball tip ham hock pork chop drumstick sirloin t-bone.";

                    descripcion += descripcion;
                    byte estado;
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

                        _collectionPosts.Add(new NewCustom() { Id = "1", PictureURL = "ms-appx:///Assets/alm.png", Description = descripcion, DateLost = DateTime.Now, IdCategory = "1", IdStatus = estado.ToString(), IdZone = "1", IdUser = "1", Name = "Noticia " + i.ToString(), NameZone = "Puriscal", QuantityComments = i, UserName = i.ToString(), UserPictureURL = "ms-appx:///Assets/user.png" });

                    }
                }
             

                return _collectionPosts;
            }
            set
            {
                
                _collectionPosts = value;
                OnPropertyChanged();
            }
        }

    
    }

}

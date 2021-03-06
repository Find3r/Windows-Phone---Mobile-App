﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pineable.Model
{
    public class NotificationEnabledObject : INotifyPropertyChanged
    {
        protected virtual void OnPropertyChanged([CallerMemberName] string pAtributo = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(pAtributo));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

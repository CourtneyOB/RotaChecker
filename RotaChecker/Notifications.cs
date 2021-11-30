using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RotaChecker
{
    public class Notifications : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //this is only used by property setters, so CallerMemberName will find the name of the property that sent it, but you still need to be able to set
        //the property name because it might be referring to other properties
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

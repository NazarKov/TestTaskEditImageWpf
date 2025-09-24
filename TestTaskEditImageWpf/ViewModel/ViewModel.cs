using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskEditImageWpf.ViewModel
{
    internal abstract class ViewModel<TViewModel> : INotifyPropertyChanged
    {
        static ViewModel(){ }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }  
    }
}

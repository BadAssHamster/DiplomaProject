using DiplomaProject.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomaProject.MVVM.ViewModel
{
    class ChooseTestViewModel : ObservableObject
    {
        public RelayCommand EcpTestCommand { get; set; }
        public EcpTestViewModel EcpTestVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public ChooseTestViewModel()
        {
            EcpTestVM = new EcpTestViewModel();

            EcpTestCommand = new RelayCommand(o =>
            {
                CurrentView = EcpTestVM;
            });
        }
    }
}

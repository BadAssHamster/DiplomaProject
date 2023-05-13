using DiplomaProject.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DiplomaProject.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public RelayCommand TheoryViewCommand { get; set; }
        public ICommand ChooseTestCommand { get; set; }
        public ICommand EcpTestCommand { get; set; }
        public TheoryViewModel TheoryPageVM { get; set; }
        public HelloViewModel HelloVM { get; set; }
        public ChooseTestViewModel ChooseTestVM { get; set; }
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

        public MainViewModel()
        { 
            TheoryPageVM = new TheoryViewModel();
            HelloVM = new HelloViewModel();
            ChooseTestVM = new ChooseTestViewModel();
            EcpTestVM = new EcpTestViewModel();
            CurrentView = HelloVM;

            TheoryViewCommand = new RelayCommand(o =>
            {
                CurrentView = TheoryPageVM;
            });

            ChooseTestCommand = new RelayCommand(o =>
            {
                CurrentView = ChooseTestVM;
            });

            EcpTestCommand = new RelayCommand(o =>
            {
                CurrentView = EcpTestVM;
            });
            
        }

    }
}

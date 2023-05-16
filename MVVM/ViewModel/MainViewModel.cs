using DiplomaProject.Core;
using Org.BouncyCastle.Security;
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
        public RelayCommand ChallengeAccessCommand { get; set; }
        public RelayCommand RegisterCommand { get; set; }
        public TheoryViewModel TheoryPageVM { get; set; }
        public HelloViewModel HelloVM { get; set; }
        public ChooseTestViewModel ChooseTestVM { get; set; }
        public EcpTestViewModel EcpTestVM { get; set; } 
        public ChallengeAccessViewModel ChallengeAccessVM { get; set; }
        public RegisterViewModel RegisterVM { get; set; }

        private object _currentView;
        private object _authView;

        public object AuthView
        {
            get { return _authView; }
            set
            {
                _authView = value;
                OnPropertyChanged();
            }
        }

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
            ChallengeAccessVM = new ChallengeAccessViewModel();
            RegisterVM = new RegisterViewModel();
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

            ChallengeAccessCommand = new RelayCommand(o =>
            {
                CurrentView = ChallengeAccessVM;
            });

            RegisterCommand = new RelayCommand(o =>
            {
                AuthView = RegisterVM;
            });

             
            
        }

    }
}

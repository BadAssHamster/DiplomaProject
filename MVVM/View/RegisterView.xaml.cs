using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DiplomaProject.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для RegisterView.xaml
    /// </summary>
    public partial class RegisterView : UserControl
    {
        public RegisterView()
        {
            InitializeComponent();
        }

        private void RegLoginBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void RegisterCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            var authWindow = Application.Current.Windows.OfType<AuthorisationWindow>().FirstOrDefault();
            if (authWindow != null)
            {
                authWindow.Close();
            }
        }

        private void TxtUserName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^a-zA-Z0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TxtPassword_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^a-zA-Z0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void CheckPassword_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^a-zA-Z0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}

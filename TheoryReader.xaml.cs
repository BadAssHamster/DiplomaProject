using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DiplomaProject
{
    /// <summary>
    /// Логика взаимодействия для TheoryReader.xaml
    /// </summary>
    public partial class TheoryReader : Window
    {
        public TheoryReader()
        {
            var mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            InitializeComponent();
            TextRange range = new TextRange(Content.Document.ContentStart, Content.Document.ContentEnd);
            range.Load(new MemoryStream(Encoding.UTF8.GetBytes(mainWindow.TheoryText)), DataFormats.Rtf);
        }

        private void ReaderCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}

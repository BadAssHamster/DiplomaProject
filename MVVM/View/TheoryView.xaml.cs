using DiplomaProject.Core;
using MySql.Data.MySqlClient;
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
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DiplomaProject.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для TheoryPage.xaml
    /// </summary>
    public partial class TheoryView : UserControl
    {
        public TheoryView()
        {
            InitializeComponent();
        }

        public void getTheory(int theoryNumber, RichTextBox TextContent)
        {
            MySqlConnection conn = new MySqlConnection("server=localhost;port=3307;database=programdb;uid=root;password=jdqVM74kzuvu3I0w;");
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader myData;
            string SQL = $"SELECT FileSize, FileBlob FROM theory WHERE idtheory={theoryNumber}";
            byte[] dataBuffer = null;
            TextRange range = new TextRange(TextContent.Document.ContentStart, TextContent.Document.ContentEnd);

            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = SQL;
                myData = cmd.ExecuteReader();

                //внесение данных из блоб в байтовый буфер
                if (myData.Read())
                {
                    dataBuffer = (byte[])myData["fileBlob"];
                }

                // преобразование буфера в строку и сохранение ее в переменную
                string rtfText = string.Empty;
                if (dataBuffer != null)
                {
                    using (MemoryStream ms = new MemoryStream(dataBuffer))
                    {
                        StreamReader sr = new StreamReader(ms);
                        rtfText = sr.ReadToEnd();
                    }
                }
                //вывод данных в ртб
                range.Load(new MemoryStream(Encoding.UTF8.GetBytes(rtfText)), DataFormats.Rtf);

                myData.Close();
                conn.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error " + ex.Number + " has occurred: " + ex.Message,
                    "Error");
            }
        }

        private void OpenReaderBtn_Click(object sender, RoutedEventArgs e)
        {
            TheoryReader TheoryWindow = new TheoryReader();
            TheoryWindow.Show();

        }

        private void EcpBasics_Click(object sender, RoutedEventArgs e)
        {
            getTheory(1, TextContent);
        }

        private void EcpMethods_Click(object sender, RoutedEventArgs e)
        {
            getTheory(2, TextContent);
        }

        private void EcpDef_Click(object sender, RoutedEventArgs e)
        {
            getTheory(3, TextContent);
        }

        private void Bscp1_Click(object sender, RoutedEventArgs e)
        {
            getTheory(4, TextContent);
        }

        private void Bscp2_Click(object sender, RoutedEventArgs e)
        {
            getTheory(5, TextContent);
        }

        private void Bscp3_Click(object sender, RoutedEventArgs e)
        {
            getTheory(6, TextContent);
        }

        private void WinInstall_Click(object sender, RoutedEventArgs e)
        {
            getTheory(8, TextContent);
        }

        private void LinuxInstall_Click(object sender, RoutedEventArgs e)
        {
            getTheory(7, TextContent);
        }

        private void MacInstall_Click(object sender, RoutedEventArgs e)
        {
            getTheory(9, TextContent);
        }
    }
}

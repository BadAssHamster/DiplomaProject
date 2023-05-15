using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DiplomaProject.MVVM.View
{
    
    public partial class ChallengeAccessView : UserControl
    {
        public List<(int mark1, int mark2, int mark3, int access)> Marks = new List<(int mark1, int mark2, int mark3, int access)>();

        #region Метод получения оценок пользователя
        public List<(int mark1, int mark2, int mark3, int access)> GetMarks(List<(int mark1, int mark2, int mark3, int access)> marks)
        {
            string connectionString = "server=localhost;port=3307;user id=root;password=jdqVM74kzuvu3I0w;database=programdb";
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand($"SELECT Test1Users, Test2Users, Test3Users, gameAccesUsers FROM users WHERE nameUsers LIKE 'denis'", connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    marks.Add((reader.GetInt16(0), reader.GetInt16(1), reader.GetInt16(2), reader.GetInt16(3)));
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return marks;
        }
        #endregion

    public ChallengeAccessView()
        {
            InitializeComponent();
            GetMarks(Marks);
            var marks = Marks[0];
            Mark1Lbl.Content = marks.mark1.ToString();
            Mark2Lbl.Content = marks.mark2.ToString();
            Mark3Lbl.Content = marks.mark3.ToString();

            Mark1Lbl.Foreground = marks.mark1 == 0 && marks.mark1 < 4 ? Brushes.DarkOrange : Brushes.Green;
            Mark2Lbl.Foreground = marks.mark2 == 0 && marks.mark2 < 4 ? Brushes.DarkOrange : Brushes.Green;
            Mark3Lbl.Foreground = marks.mark3 == 0 && marks.mark3 < 4 ? Brushes.DarkOrange : Brushes.Green;
            StartChallenge.IsEnabled = marks.mark1 > 3 && marks.mark2 > 3  && marks.mark3 > 3 ? true : false;

        }
    }
}

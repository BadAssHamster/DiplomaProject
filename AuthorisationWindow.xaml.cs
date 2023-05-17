using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для AuthorisationWindow.xaml
    /// </summary>
    public partial class AuthorisationWindow : Window
    {
        public List<(int userId, string userName)> UserDataList = new List<(int userId, string userName)>();
        public bool Logon;
        private MainWindow _mainWindow;
        #region Метод авторизации пользователя
        public List<(int userId, string userName)> GetUser (List<(int userId, string userName)> User, string UserName, string userPass)
        {

            string connectionString = "server=localhost;port=3307;user id=root;password=jdqVM74kzuvu3I0w;database=programdb";
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand($"SELECT idUsers, nameUsers FROM users WHERE nameUsers = '{UserName}' AND passUsers = '{userPass}';", connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    User.Add((reader.GetInt32(0), reader.GetString(1)));
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

            return User;
        }
        #endregion
        public AuthorisationWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void AuthCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            GetUser(UserDataList, TxtUserName.Text.ToString(), TxtPassword.Password.ToString());
            if (UserDataList != null)
            {
                
                var userData = UserDataList[0];
                _mainWindow.UserData = UserDataList;
                _mainWindow.Logon = true;
                _mainWindow.userName.Text = userData.userName.ToString();
                _mainWindow.LoginBtn.Content = "Выйти";
                this.Close();
            }
            else
            {
                MessageBox.Show("Введены неверные логин или пароль", "Внимание!");
            }

        }
    }
}

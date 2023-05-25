using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
    
    public partial class RegisterView : UserControl
    {
        public bool IsRegistered { get; set; }
        public List<(int userId, string userName)> UserDataList = new List<(int userId, string userName)>();
        public bool Logon;

        #region Метод поиска пользователя
        public bool CheckUser(string userName)
        {
            bool checker = false;
            string connectionString = "server=localhost;port=3307;user id=root;password=jdqVM74kzuvu3I0w;database=programdb";
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand($"SELECT nameUsers FROM users WHERE nameUsers = '{userName}';", connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if (reader.GetString(0) != "")
                    {
                        checker = true; ;
                    }

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

            return checker;

        }
        #endregion

        #region Метод добавления пользователя в базу данных
        public void Register(string userName, string userPass)
        {
            if (TxtUserName.Text.ToString().ToLower().Contains("drop") || TxtPassword.Password.ToString().ToLower().Contains("drop"))
            {
                MessageBox.Show("Без SQL инъекций");
            }
            else
            {
                string connectionString = "server=localhost;port=3307;user id=root;password=jdqVM74kzuvu3I0w;database=programdb";
                MySqlConnection connection = new MySqlConnection(connectionString);

                try
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand($"INSERT INTO users (nameUsers, passUsers) VALUES ('{userName}', '{userPass}');", connection);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Пользователь зарегистрирован");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                
            }
        }
        #endregion

        #region Метод авторизации пользователя
        public List<(int userId, string userName)> GetUser(List<(int userId, string userName)> User, string UserName, string userPass)
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
        public RegisterView()
        {
            InitializeComponent();
        }

        private void RegLoginBtn_Click(object sender, RoutedEventArgs e)
        {
            IsRegistered = CheckUser(TxtUserName.Text.ToString());
            if (IsRegistered == true)
            {
                MessageBox.Show("Такой пользователь уже зарегистрирован");
                UserDataList = null;
            }
            else
            {
                if (TxtUserName.Text.ToString() == "")
                {
                    MessageBox.Show("Введите имя пользователя");

                }
                else 
                {
                    if (TxtPassword.Password != CheckPassword.Password)
                    {
                        MessageBox.Show("Пароли не совпадают");
                    }
                    else
                    {
                        Register(TxtUserName.Text.ToString().ToLower().Trim(), TxtPassword.Password.ToString().ToLower().Trim());
                        GetUser(UserDataList, TxtUserName.Text.ToString().ToLower().Trim(), TxtPassword.Password.ToString().ToLower().Trim());
                        if (UserDataList != null)
                        {
                            var authWindow = Application.Current.Windows.OfType<AuthorisationWindow>().FirstOrDefault();
                            var userData = UserDataList[0];
                            authWindow._mainWindow.UserData = UserDataList;
                            authWindow._mainWindow.Logon = true;
                            authWindow._mainWindow.userName.Text = userData.userName.ToString().ToLower().Trim();
                            authWindow._mainWindow.LoginBtn.Content = "Выйти";


                            if (authWindow != null)
                            {
                                authWindow.Close();
                            }

                        }
                    }
                }       
            }
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

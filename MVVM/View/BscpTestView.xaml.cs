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
using System.Windows.Threading;

namespace DiplomaProject.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для BscpTestView.xaml
    /// </summary>
    public partial class BscpTestView : UserControl
    {
        public int TestIndex = 2;
        public int questionCounter = -1;
        public int EcpScore = 0;
        public int secondsLeft = 0;
        public bool answerChecked = false;
        public List<(string question, int questionId, int questionType)> Questions = new List<(string questionList, int questionId, int questionType)>();
        public List<(string answer, int checkAnswer)> Answers = new List<(string answer, int checkAnswer)>();
        DispatcherTimer timer = new DispatcherTimer();
        private void Timer_Tick(object sender, EventArgs e)
        {
            TimerProgBar.Value++;
            secondsLeft--;
            TimeLeftLbl.Content = $"Секунд осталось: {secondsLeft}";

            if (TimerProgBar.Value >= TimerProgBar.Maximum)
            {
                timer.Stop();
                CheckAnswer();
                answerChecked = true;
            }
        }
        private void ResetTimer()
        {
            TimerProgBar.Value = 0;
            timer.Stop();
        }
        #region Метод выборки 10 рандомных вопросов из базы данных
        public List<(string question, int questionId, int questionType)> GetQuestions(List<(string question, int questionId, int questionType)> questionList, int surveyIndex)
        {

            string connectionString = "server=localhost;port=3307;user id=root;password=jdqVM74kzuvu3I0w;database=programdb";
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand($"SELECT Question, idQuestions, typeQuestion FROM questions WHERE themeQuestion={surveyIndex} ORDER BY RAND() LIMIT 10", connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    questionList.Add((reader.GetString(0), reader.GetInt32(1), reader.GetInt32(2)));
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

            return questionList;
        }
        #endregion

        #region Метод выборки ответов на текущий вопрос
        public List<(string answer, int checkAnswer)> GetAnswers(List<(string answer, int checkAnswer)> answers, int questionId)
        {
            Answers.Clear();
            string connectionString = "server=localhost;port=3307;user id=root;password=jdqVM74kzuvu3I0w;database=programdb";
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand($"SELECT answer, checkAnswer FROM answers WHERE idQuestion={questionId}", connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    answers.Add((reader.GetString(0), reader.GetInt32(1))); // добавляем значения из каждой строки в кортеж
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return answers;
        }
        #endregion

        #region Метод открытия слудющего вопроса
        public void NextQuestion()
        {
            ResetTimer();
            CheckBtn.IsEnabled = true;
            questionCounter++;
            QuestionNumberLbl.Content = $"Вопрос: {questionCounter + 1} / 10";
            var question = Questions[questionCounter];
            QuestionText.Text = question.question.ToString();
            switch (question.questionType)
            {
                case 1:
                    RdbStack.Visibility = Visibility.Visible;
                    CheckStack.Visibility = Visibility.Collapsed;
                    TextStack.Visibility = Visibility.Collapsed;
                    TimerProgBar.Maximum = 30;
                    secondsLeft = 30;
                    TimeLeftLbl.Content = $"Секунд осталось: {secondsLeft}";
                    timer.Start();
                    break;
                case 2:
                    RdbStack.Visibility = Visibility.Collapsed;
                    CheckStack.Visibility = Visibility.Visible;
                    TextStack.Visibility = Visibility.Collapsed;
                    TimerProgBar.Maximum = 60;
                    secondsLeft = 60;
                    TimeLeftLbl.Content = $"Секунд осталось: {secondsLeft}";
                    timer.Start();
                    break;
                case 3:
                    RdbStack.Visibility = Visibility.Collapsed;
                    CheckStack.Visibility = Visibility.Collapsed;
                    TextStack.Visibility = Visibility.Visible;
                    TimerProgBar.Maximum = 90;
                    secondsLeft = 90;
                    TimeLeftLbl.Content = $"Секунд осталось: {secondsLeft}";
                    timer.Start();
                    break;
            }

            GetAnswers(Answers, question.questionId);

            if (question.questionType != 3)
            {
                for (int answerNumber = 0; answerNumber < 4; answerNumber++)
                {
                    var answer = Answers[answerNumber];
                    switch (question.questionType)
                    {
                        case 1:
                            var find_rdb = RdbStack.FindName($"rdb_{answerNumber + 1}") as RadioButton;
                            if (find_rdb != null)
                            {
                                find_rdb.Background = SystemColors.ControlBrush;
                                find_rdb.IsChecked = false;
                                find_rdb.IsEnabled = true;
                                find_rdb.Content = answer.answer;
                                find_rdb.Tag = answer.checkAnswer;
                            }
                            break;
                        case 2:
                            var find_check = CheckStack.FindName($"check_{answerNumber + 1}") as CheckBox;
                            if (find_check != null)
                            {
                                find_check.Foreground = SystemColors.ControlBrush;
                                find_check.IsChecked = false;
                                find_check.IsEnabled = true;
                                find_check.Content = answer.answer;
                                find_check.Tag = answer.checkAnswer;
                            }
                            break;
                    }
                }
            }
            else
            {
                var answer = Answers[0];
                var find_txtbox = TextStack.FindName($"AnswerTextBox") as TextBox;
                if (find_txtbox != null)
                {
                    find_txtbox.Background = SystemColors.ControlBrush;
                    find_txtbox.IsEnabled = true;
                    find_txtbox.Text = "";
                    find_txtbox.Tag = answer.answer;
                }
            }
        }
        #endregion

        #region Метод проверки ответа на вопрос
        public void CheckAnswer()
        {
            var question = Questions[questionCounter];
            if (question.questionType != 3)
            {
                int chb_right_answers_counter = 0;
                int chb_case_counter = 0;
                for (int answerNumber = 0; answerNumber < 4; answerNumber++)
                {
                    var answer = Answers[answerNumber];
                    string textAnswer = answer.answer.ToString();
                    int answerCheck = answer.checkAnswer;
                    chb_right_answers_counter += answerCheck;

                    switch (question.questionType)
                    {
                        case 1:
                            var find_rdb = RdbStack.FindName($"rdb_{answerNumber + 1}") as RadioButton;
                            if (find_rdb != null && find_rdb.IsChecked == true)
                            {
                                if (find_rdb.Tag.Equals(1))
                                {
                                    EcpScore++;
                                    find_rdb.Background = Brushes.YellowGreen;
                                }
                                else
                                {
                                    find_rdb.Background = Brushes.DarkRed;
                                }
                                CheckBtn.IsEnabled = false;
                            }
                            else
                            {
                                CheckBtn.IsEnabled = false;
                                find_rdb.IsEnabled = false;
                            }
                            break;
                        case 2:
                            var find_check = CheckStack.FindName($"check_{answerNumber + 1}") as CheckBox;
                            if (find_check != null && find_check.IsChecked == true)
                            {
                                if (find_check.Tag.Equals(1))
                                {
                                    chb_case_counter++;
                                    find_check.Foreground = Brushes.Green;
                                }
                                else
                                {
                                    find_check.Foreground = Brushes.Red;
                                }

                            }
                            else
                            {
                                find_check.IsEnabled = false;
                            }
                            if (answerNumber == 3 && chb_case_counter == chb_right_answers_counter)
                            {
                                EcpScore++;
                            }
                            CheckBtn.IsEnabled = false;
                            break;
                    }
                }
            }
            else
            {
                var answer = Answers[0];
                var find_txtbox = TextStack.FindName("AnswerTextBox") as TextBox;
                if (find_txtbox != null)
                {
                    if (find_txtbox.Text.ToLower() == answer.answer.ToLower())
                    {
                        EcpScore++;
                        find_txtbox.Background = Brushes.LightGreen;
                        find_txtbox.IsEnabled = false;
                        CheckBtn.IsEnabled = false;
                    }
                    else
                    {
                        find_txtbox.Background = Brushes.Red;
                        find_txtbox.IsEnabled = false;
                        CheckBtn.IsEnabled = false;
                    }
                }
            }
        }
        #endregion

        #region Метод проверки оценки
        public bool MarkCheck(int userId, int mark)
        {
            bool flag = false;
            string connectionString = "server=localhost;port=3307;user id=root;password=jdqVM74kzuvu3I0w;database=programdb";
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand($"SELECT test1Users FROM users WHERE idUsers={userId};", connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if (mark > reader.GetInt32(0))
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return flag;

        }
        #endregion
        public BscpTestView()
        {
            InitializeComponent();
            var mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            mainWindow.AboutBtn.IsEnabled = false;
            mainWindow.TheoryBtn.IsEnabled = false;
            mainWindow.TestsBtn.IsEnabled = false;
            mainWindow.ChallengeBtn.IsEnabled = false;
            GetQuestions(Questions, TestIndex);
            NextQuestion();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += new EventHandler(Timer_Tick);
        }

        private void ExitTestBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Если сейчас Вы закроете тест, Ваш результ не будет сохранен. Вы уверенны?", "Внимание!", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                var mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                mainWindow.AboutBtn.IsEnabled = true;
                mainWindow.TheoryBtn.IsEnabled = true;
                mainWindow.TestsBtn.IsEnabled = true;
                mainWindow.ChallengeBtn.IsEnabled = true;
                timer.Stop();
                EndTestBtn.Command.Execute(null);
            }
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (answerChecked == false)
            {
                MessageBox.Show("Вы не ответили на вопрос!");
            }
            else
            {
                answerChecked = false;
                NextQuestion();
            }
            if (questionCounter == 9)
            {
                NextBtn.Visibility = Visibility.Collapsed;
            }
        }

        private void CheckBtn_Click(object sender, RoutedEventArgs e)
        {
            CheckAnswer();
            answerChecked = true;
            timer.Stop();
            if (questionCounter == 9)
            {
                CheckBtn.Visibility = Visibility.Collapsed;
                ExitTestBtn.Visibility = Visibility.Collapsed;
                EndTestBtn.Visibility = Visibility.Visible;
            }
        }

        private void EndTestBtn_Click(object sender, RoutedEventArgs e)
        {
            int testMark = 0;

            if (EcpScore <= 4)
            {
                testMark = 2;
            }
            else if (EcpScore > 4 && EcpScore < 7)
            {
                testMark = 3;
            }
            else if (EcpScore > 6 && EcpScore < 9)
            {
                testMark = 4;
            }
            else
            {
                testMark = 5;
            }

            var mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            if (mainWindow.Logon == true)
            {
                var data = mainWindow.UserData[0];
                bool isBetter = MarkCheck(data.userId, testMark);
                if (isBetter == true)
                {
                    string connectionString = "server=localhost;port=3307;user id=root;password=jdqVM74kzuvu3I0w;database=programdb";
                    MySqlConnection connection = new MySqlConnection(connectionString);

                    try
                    {
                        connection.Open();

                        MySqlCommand command = new MySqlCommand($"UPDATE users SET test1Users = {testMark} WHERE idUsers = {data.userId};", connection);
                        command.ExecuteNonQuery();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                    MessageBox.Show($"Вы ответили на {EcpScore} из 10 вопросов.\nВаша оценка обновлена на {testMark}", "Результат");
                }
                else
                {
                    MessageBox.Show($"Вы ответили на {EcpScore} из 10 вопросов.\nВаша оценка: {testMark}. Оценка не была обновлена.", "Результат");
                }
            }
            else
            {
                MessageBox.Show($"Вы ответили на {EcpScore} из 10 вопросов.\nВаша оценка могла бы быть: {testMark}. Но вы не авторизованный пользователь.", "Результат");
            }

            mainWindow.AboutBtn.IsEnabled = true;
            mainWindow.TheoryBtn.IsEnabled = true;
            mainWindow.TestsBtn.IsEnabled = true;
            mainWindow.ChallengeBtn.IsEnabled = true;

        }
    }
}

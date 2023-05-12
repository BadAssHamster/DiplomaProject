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
using MySql.Data.MySqlClient;

namespace DiplomaProject.MVVM.View
{
    
    public partial class EcpTestView : UserControl
    {
        public int TestIndex = 1;
        public int questionCounter = -1;
        public List<(string question, int questionId, int questionType)> Questions = new List<(string questionList, int questionId, int questionType)>();
        public List<(string answer, int checkAnswer)> Answers = new List<(string answer, int checkAnswer)>();

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
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return answers;
        }
        public void NextQuestion()
        {
            questionCounter++;
            var question = Questions[questionCounter];
            QuestionText.Text = question.question.ToString();
            switch (question.questionType)
            {
                case 1:
                    RdbStack.Visibility = Visibility.Visible;
                    CheckStack.Visibility = Visibility.Collapsed;
                    TextStack.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    RdbStack.Visibility = Visibility.Collapsed;
                    CheckStack.Visibility = Visibility.Visible;
                    TextStack.Visibility = Visibility.Collapsed;
                    break;
                case 3:
                    RdbStack.Visibility = Visibility.Collapsed;
                    CheckStack.Visibility = Visibility.Collapsed;
                    TextStack.Visibility = Visibility.Visible;
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
                                find_rdb.IsChecked = false;
                                find_rdb.Content = answer.answer;
                                find_rdb.Tag = answer.checkAnswer;
                            }                            
                            break;
                        case 2:
                            var find_check = CheckStack.FindName($"check_{answerNumber + 1}") as CheckBox;
                            if(find_check != null)
                            {
                                find_check.IsChecked = false;
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
                    find_txtbox.Text = "";
                    find_txtbox.Tag = answer.checkAnswer;
                }
            }
        }
        public EcpTestView()
        {
            InitializeComponent();
            GetQuestions(Questions, TestIndex);
            NextQuestion();

        }
               
        private void ExitTestBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            NextQuestion();
            if (questionCounter == 9)
            {
                NextBtn.Visibility = Visibility.Collapsed;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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
    /// Логика взаимодействия для ChallengeView.xaml
    /// </summary>
    public partial class ChallengeView : UserControl
    {
        public string Answer;
        public string AnswerForTb;
        public List<(string Key, string Value)> Answers;
        public int seconds = 0;
        public int score = 0;
        DispatcherTimer timer = new DispatcherTimer();
        private void Timer_Tick(object sender, EventArgs e)
        {
            seconds += 1;
        }
        private void ResetTimer()
        {
            seconds = 0;
            timer.Stop();
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject parent) where T : DependencyObject
        {
            List<T> children = new List<T>();
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                {
                    children.Add((T)child);
                }
                else
                {
                    IEnumerable<T> foundChildren = FindVisualChildren<T>(child);
                    if (foundChildren != null)
                    {
                        children.AddRange(foundChildren);
                    }
                }
            }
            return children;
        }
        public ChallengeView()
        {
            InitializeComponent();
            TextDecorationCollection defaultDecorations = Answer_1.TextDecorations;
            Answers = new List<(string Key, string Value)>()
            {
                ("Answer_1", "КРИПТОЛОГИЯ"),
                ("Answer_2", "ЦЕЛОСТНОСТЬ"),
                ("Answer_3", "КЛЮЧ"),
                ("Answer_4", "ДЕШИФРАТОР"),
                ("Answer_5", "ИДЕНТИФИЦИРУЕМОСТЬ"),
                ("Answer_6", "ШИФРОВАНИЕ"),
                ("Answer_7", "КОНФЕДЕНЦИАЛЬНОСТЬ"),
                ("Answer_8", "АУТЕНТИФИКАЦИЯ"),
                ("Answer_9", "ШИФРОСИСТЕМА"),
                ("Answer_10", "СИММЕТРИЧНОСТЬ"),
                ("Answer_11", "ПОДПИСЬ"),
                ("Answer_12", "ТОКЕН"),
                ("Answer_13", "СЕРТИФИКАТ"),
                ("Answer_14", "БРАУЗЕР"),
                ("Answer_15", "НЕОТРЕКАЕМОСТЬ"),
                ("Answer_16", "АЛЛАДИН"),
                ("Answer_17", "ПОДЛИННОСТЬ"),
                ("Answer_18", "ПИНКОД"),
                ("Answer_19", "ПЛАГИН")
            };
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += new EventHandler(Timer_Tick);
            ResetTimer();
            timer.Start();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
         {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null && checkBox.IsChecked == true)
            {
                string content = checkBox.Content.ToString();
                AnswerForTb += content + " ";
                Answer += content;
                AnswerTb.Text = AnswerForTb;
            }
        }

        private void DropWordBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox checkBox in FindVisualChildren<CheckBox>(Application.Current.Windows.OfType<MainWindow>().FirstOrDefault()))
            {
                if (checkBox.IsChecked == true && checkBox.IsEnabled == true)
                {
                    checkBox.IsChecked = false;
                }
            }
            AnswerForTb = "";
            Answer = "";
            AnswerTb.Text = "";
        }

        private void DropProgressBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox checkBox in FindVisualChildren<CheckBox>(Application.Current.Windows.OfType<MainWindow>().FirstOrDefault()))
            {
                if (checkBox.IsChecked == true)
                {
                    checkBox.IsChecked = false;
                    checkBox.IsEnabled = true;
                }
            }
            for (int i = 0; i < 19; i++)
            {
                var couple = Answers[i];
                if (i < 10)
                {
                    var find_block = LeftAnswersPanel.FindName($"{couple.Key}") as TextBlock;
                    if (find_block != null)
                    {
                        
                        find_block.TextDecorations = default;
                        
                    }
                }
                else
                {
                    var find_block = RightAnswersPanel.FindName($"{couple.Key}") as TextBlock;
                    if (find_block != null)
                    {
                        find_block.TextDecorations = default;
                    }
                }
            }
            AnswerForTb = "";
            Answer = "";
            AnswerTb.Text = "";
            score = 0;
            ResetTimer();
        }

        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 19; i++)
            {
                var couple = Answers[i];
                if (Answer == couple.Value.ToString())
                {
                    if (i < 10)
                    {
                        var find_block = LeftAnswersPanel.FindName($"{couple.Key}") as TextBlock;

                        if(find_block != null)
                        {
                            find_block.TextDecorations = TextDecorations.Strikethrough;
                            foreach (CheckBox checkBox in FindVisualChildren<CheckBox>(Application.Current.Windows.OfType<MainWindow>().FirstOrDefault()))
                            {
                                if (checkBox.IsChecked == true)
                                {
                                    checkBox.IsEnabled = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        var find_block = RightAnswersPanel.FindName($"{couple.Key}") as TextBlock;

                        if (find_block != null)
                        {
                            find_block.TextDecorations = TextDecorations.Strikethrough;
                            foreach (CheckBox checkBox in FindVisualChildren<CheckBox>(Application.Current.Windows.OfType<MainWindow>().FirstOrDefault()))
                            {
                                if (checkBox.IsChecked == true)
                                {
                                    checkBox.IsEnabled = false;
                                }
                            }
                        }
                    }
                    score++;
                } 

            }
            AnswerForTb = "";
            Answer = "";
            AnswerTb.Text = "";
            if(score == 19)
            {
                timer.Stop();
                MessageBox.Show($"Поздравляем!!!\n Вы справились за {seconds / 60} минут {seconds % 60} секунд", "Поздравления");
            }
        }
    }
}

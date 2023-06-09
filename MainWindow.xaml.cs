﻿using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DiplomaProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<(int userId, string userName)> UserData { get; set; }
        public bool Logon { get; set; }

        public string TheoryText { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            MessageBox.Show("Полный функционал программы доступен только авторизованным пользователям!", "ВНИМАНИЕ!");
        }

        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            // Видимость подсказок при раскрытой навигационной панели

            if(TglBtn.IsChecked == true)
            {
                TtTheory.Visibility = Visibility.Collapsed;
                TtTest.Visibility = Visibility.Collapsed;
                TtChallenge.Visibility = Visibility.Collapsed;
                TtAbout.Visibility = Visibility.Collapsed;
            }
            else
            {
                TtTheory.Visibility = Visibility.Visible;
                TtTest.Visibility = Visibility.Visible;
                TtChallenge.Visibility = Visibility.Visible;
                TtAbout.Visibility = Visibility.Visible;
            }
        }

        // Затемнение основного окна при раскрытии бокового меню
        private void TglBtn_Checked(object sender, RoutedEventArgs e)
        {
            ContentGrid.Opacity = 0.1;
            NavPnl.Opacity = 1;
        }

        private void TglBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            NavPnl.Opacity = 1;
            ContentGrid.Opacity = 1;
        }

        // Выключение программы
        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // Перетягивание окна программы
        private void UpperBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void ContentGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TglBtn.IsChecked = false;
        }

        private void TheoryBtn_Click(object sender, RoutedEventArgs e)
        {
            TheoryLVI.IsSelected = true;
            TestsLVI.IsSelected = false;
            ChallengeLVI.IsSelected = false;
            AboutLVI.IsSelected = false;    
        }

        private void TestsBtn_Click(object sender, RoutedEventArgs e)
        {
            TheoryLVI.IsSelected = false;
            TestsLVI.IsSelected = true;
            ChallengeLVI.IsSelected = false;
            AboutLVI.IsSelected = false;
        }

        private void ChallengeBtn_Click(object sender, RoutedEventArgs e)
        {
            TheoryLVI.IsSelected = false;
            TestsLVI.IsSelected = false;
            ChallengeLVI.IsSelected = true;
            AboutLVI.IsSelected = false;
        }

        private void AboutBtn_Click(object sender, RoutedEventArgs e)
        {
            TheoryLVI.IsSelected = false;
            TestsLVI.IsSelected = false;
            ChallengeLVI.IsSelected = false;
            AboutLVI.IsSelected = true;

            AboutWindow ShowAbout = new AboutWindow();
            ShowAbout.ShowDialog();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            AuthorisationWindow authorisationWindow = new AuthorisationWindow(this);
            if (Logon == false)
            {
                authorisationWindow.ShowDialog();
            }
            else
            {
                Logon = false;
                LoginBtn.Content = "Войти";
                userName.Text = "Не авторизован";
            }
        }
    }
}

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

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginBox_GotFocus(object sender, RoutedEventArgs e)
        {
            LoginLabel.Visibility= Visibility.Collapsed;
            LoginBox.Background = null;
            LoginBox.ToolTip= null;
        }
        private void LoginBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(LoginBox.Text))
            {
                LoginLabel.Visibility= Visibility.Visible;
            }
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordLabel.Visibility= Visibility.Collapsed;
            PasswordBox.Background = null;
            PasswordBox.ToolTip= null;
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordBox.Password))
            {
                PasswordLabel.Visibility= Visibility.Visible;
            }
        }

        private void CloseBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void CloseBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = null;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {    
            if (MessageBox.Show("Вы действительно хотите выйти из приложения?", "Подтвердите выход из приложения", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                // закрыть приложение
                Application.Current.Shutdown();
            }
        }
        private void LogInBtn_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF13636"));
            var Login = ConnectionClass.db.Users.FirstOrDefault(u => u.Username == LoginBox.Text);
            var Password = ConnectionClass.db.Users.FirstOrDefault(u => u.Password == PasswordBox.Password);
            if (Login != null && Password != null)
            {
                this.Hide();
                var Mp = new MainPage();
                Mp.Show();
            }
            else if (Login == null && Password == null)
            {
                LoginBox.ToolTip = "Неверный логин";
                PasswordBox.ToolTip = "Неверный пароль";
                LoginBox.Background = brush;
                PasswordBox.Background = brush;
            }
            else if (Login == null)
            {
                LoginBox.ToolTip = "Неверный логин";
                LoginBox.Background = brush;
            }

            else 
            {
                PasswordBox.ToolTip = "Неверный пароль";
                PasswordBox.Background = brush;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Window
    {
        public MainPage()
        {
            InitializeComponent();
            DateOfBirth.DisplayDateStart= DateTime.Now.AddYears(-120);
            DateOfBirth.DisplayDateEnd=DateTime.Now.AddYears(-18);
            ClientList.IsReadOnly = false;
            string connectionString = @"Data Source= UMBRE-LL-A\SQLEXPRESS; Initial Catalog = MasterBankDB; Integrated Security = true; Connect Timeout = 15;" +
        "Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
            SqlConnection Con = new SqlConnection(connectionString); 
            Con.Open(); //Открываем соединение
            SqlDataAdapter da = new SqlDataAdapter(" SELECT FirstName LastName, DateOfBirth, Phone, Email, CardNumber, PassportSeries, PassportNumber FROM Clients", Con);
            DataSet ds = new DataSet();
            da.Fill(ds, "Clients");
            ClientList.ItemsSource = ds.Tables["Clients"].DefaultView;
            ClientList.IsReadOnly = true;
            Con.Close(); //Закрываем соединение
            MonthlyPaymentAmount.IsReadOnly = true;
            OverpaymentOfInterestOnTheLoan.IsReadOnly = true;
            TotalOverpaymentForTheEntirePeriod.IsReadOnly = true;
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

        private void InfoMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Версия программы: 1.0\nРазработчик: Жадан Д. В.");
        }

        private void NewClientMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            ShowClientsPanel.Visibility = Visibility.Collapsed;
            NewApplicationPanel.Visibility = Visibility.Collapsed;
            CalculatePanel.Visibility = Visibility.Collapsed;
            AddClientPanel.Visibility= Visibility.Visible;
            MenuItem menuItem = sender as MenuItem;
            menuItem.LostFocus += ShoClientsMenuBtn_LostFocus;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            bool isChecked = true;
            foreach (var textBox in AddClientPanel.Children.OfType<TextBox>())
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    isChecked = false;
                }
            }
            if (isChecked&&DateOfBirth.SelectedDate!=null)
            {
                try
                {
                    string query = "INSERT INTO Clients (FirstName, LastName, DateOfBirth, Phone, Email, CardNumber, PassportSeries, PassportNumber) " +
                    "VALUES (@FirstName, @LastName, @DateOfBirth, @Phone, @Email, @CardNumber, @PassportSeries, @PassportNumber)";
                    SqlParameter[] parameters =
                    {
                     new SqlParameter("@FirstName",FirstName.Text),
                     new SqlParameter("@LastName", LastName.Text),
                     new SqlParameter("@DateOfBirth", DateOfBirth.SelectedDate),
                     new SqlParameter("@Phone",Phone.Text),
                     new SqlParameter("@Email", Email.Text),
                     new SqlParameter("@CardNumber",CardNumber.Text),
                     new SqlParameter("@PassportSeries", PassportSeries.Text),
                     new SqlParameter("@PassportNumber", PassportNumber.Text)
                };
                    ConnectionClass.db.Database.ExecuteSqlCommand(query, parameters);
                    ConnectionClass.db.SaveChanges();

                    foreach (var textBox in AddClientPanel.Children.OfType<TextBox>())
                    {
                        textBox.Text = null;
                    }
                    DateOfBirth.SelectedDate = null;
                    MessageBox.Show("Клиент успешно добавлен");
                    AddClientPanel.Visibility = Visibility.Collapsed;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка добавления" + ex.StackTrace);
                }
            }
            else
            {
                MessageBox.Show("Предупреждение", "Не все поля заполнены", MessageBoxButton.OK);
            }
        }

        private void ShoClientsMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            AddClientPanel.Visibility=Visibility.Collapsed;
            CalculatePanel.Visibility=Visibility.Collapsed;
            NewApplicationPanel.Visibility = Visibility.Collapsed;
            ShowClientsPanel.Visibility = Visibility.Visible;
            MenuItem menuItem = sender as MenuItem;
            menuItem.LostFocus += ShoClientsMenuBtn_LostFocus;
        }


        private void ShoClientsMenuBtn_LostFocus(object sender, RoutedEventArgs e)
        {
            AddClientPanel.Visibility = Visibility.Collapsed;
            ShowClientsPanel.Visibility = Visibility.Collapsed;
            NewApplicationPanel.Visibility = Visibility.Collapsed;
            CalculatePanel.Visibility = Visibility.Collapsed;
            foreach (var textBox in AddClientPanel.Children.OfType<TextBox>())
            {
                textBox.Text = null;
            }
            foreach (var textBox in CalculatePanel.Children.OfType<TextBox>())
            {
                textBox.Text = null;
            }
            DateOfBirth.SelectedDate = null;
            MenuItem menuItem = sender as MenuItem;
            menuItem.LostFocus -= ShoClientsMenuBtn_LostFocus;
        }

        private void CalculateMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            AddClientPanel.Visibility = Visibility.Collapsed;
            ShowClientsPanel.Visibility = Visibility.Collapsed;
            NewApplicationPanel.Visibility = Visibility.Collapsed;
            CalculatePanel.Visibility = Visibility.Visible;
            MenuItem menuItem = sender as MenuItem;
            menuItem.LostFocus += ShoClientsMenuBtn_LostFocus;
        }

        private void CalculateBtn_Click(object sender, RoutedEventArgs e)
        {
            double LA = double.Parse(LoanAmount.Text);//Сумма кредита
            double IR = double.Parse(InterestRate.Text)/1200;//Процентная ставка
            double LT = double.Parse(LoanTerm.Text);//Срок кредита
            double MPA = (LA * IR * Math.Pow((1.0 + IR),LT))/(Math.Pow((1+IR),LT)-1);
            double OOPA = (MPA*LT)-LA;
            double TOFTEP = (OOPA/LA)*100;
            MonthlyPaymentAmount.IsReadOnly = false;
            OverpaymentOfInterestOnTheLoan.IsReadOnly = false;
            TotalOverpaymentForTheEntirePeriod.IsReadOnly = false;
            MonthlyPaymentAmount.Text = Math.Ceiling(MPA).ToString() + " ₽";
            OverpaymentOfInterestOnTheLoan.Text = Math.Ceiling(OOPA).ToString() + " ₽";
            TotalOverpaymentForTheEntirePeriod.Text =Math.Round(TOFTEP,2).ToString() + " %";
            MonthlyPaymentAmount.IsReadOnly = true;
            OverpaymentOfInterestOnTheLoan.IsReadOnly = true;
            TotalOverpaymentForTheEntirePeriod.IsReadOnly = true;
        }

        private void Newapplication_Click(object sender, RoutedEventArgs e)
        {
            AddClientPanel.Visibility = Visibility.Collapsed;
            ShowClientsPanel.Visibility = Visibility.Collapsed;
            CalculatePanel.Visibility = Visibility.Collapsed;
            NewApplicationPanel.Visibility = Visibility.Visible;
        }
    }
}

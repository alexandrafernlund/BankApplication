using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static BankApplication.MainWindow;

namespace BankApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Account> accounts;
        public MainWindow()
        {
            InitializeComponent();
            DummyData dummyData = new DummyData();
            accounts = dummyData.GetDummyData();

            AccountCombobox.ItemsSource = accounts;
            ToCombobox.ItemsSource = accounts;
            FromCombobox.ItemsSource = accounts;
        }

        public class Account
        {
            public string Name { get; set; }
            public decimal Balance { get; set; }

            public Account(string name, decimal balance)
            {
                Name = name;
                Balance = balance;
            }
        }

        public class DummyData
        {
            public List<Account> GetDummyData()
            {
                List<Account> accounts = new List<Account>
                {
                    new Account("Spending", 1000),
                    new Account("Saving", 1500)
                };

                return accounts;
            }
        }

        public class Transaction
        {
            public string From { get; set; }
            public string To { get; set; }
            public DateTime Date { get; set; }
            public decimal Sum { get; set; }
            public decimal Balance { get; set; }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

        }


        public void AddAccount_Click(object sender, RoutedEventArgs e)
        {
            NewAccountPopup.IsOpen = true;
        }

        public void NewTransaction_Click(object sender, RoutedEventArgs e)
        {
            NewTransactionPopup.IsOpen = true;
        }

        public void ClosePopup_Click(object sender, RoutedEventArgs e)
        {
            NewAccountPopup.IsOpen = false;
            NewTransactionPopup.IsOpen = false;
        }
    }
}
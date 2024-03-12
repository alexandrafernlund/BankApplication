using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace BankApplication
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            DummyData dummyData = new DummyData();
            accounts = dummyData.GetDummyData();
            transactions = new List<Transaction>();

            AccountCombobox.ItemsSource = accounts;
            ToCombobox.ItemsSource = accounts;
            FromCombobox.ItemsSource = accounts;
        }

        private string amountText;
        public string AmountText
        {
            get { return amountText; }
            set
            {
                if (amountText != value)
                {
                    amountText = value;
                    OnPropertyChanged(nameof(AmountText));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private List<Account> accounts;
        private List<Transaction> transactions;

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

        private void TransferOKButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if both accounts are selected
            if (FromCombobox.SelectedItem == null || ToCombobox.SelectedItem == null)
            {
                NewTransactionPopup.IsOpen = false;
                MessageBox.Show("Please select both source and destination accounts.");
                return;
            }

            Account fromAccount = (Account)FromCombobox.SelectedItem;
            Account toAccount = (Account)ToCombobox.SelectedItem;

            // Validate the amount
            decimal amount;
            if (!decimal.TryParse(AmountTextbox.Text, out amount))
            {
                NewTransactionPopup.IsOpen = false;
                MessageBox.Show("Please enter a valid amount.");
                return;
            }

            // Check if there's enough balance in the source account
            if (fromAccount.Balance < amount)
            {
                NewTransactionPopup.IsOpen = false;
                MessageBox.Show("Insufficient balance in the source account.");
                return;
            }

            PerformTransaction(fromAccount, toAccount, amount);
            NewTransactionPopup.IsOpen = false;

            // Reset input fields and update ComboBox data
            AmountTextbox.Text = ""; 
            FromCombobox.SelectedItem = null; 
            ToCombobox.SelectedItem = null;
            AccountCombobox.ItemsSource = null; 
            ToCombobox.ItemsSource = null; 
            FromCombobox.ItemsSource = null; 

            // Reassign ComboBox data to reflect updated account balances
            AccountCombobox.ItemsSource = accounts;
            ToCombobox.ItemsSource = accounts;
            FromCombobox.ItemsSource = accounts;

            // Provide feedback to the user
            MessageBox.Show("Transaction successful.");
        }

        private void PerformTransaction(Account fromAccount, Account toAccount, decimal transactionAmount)
        {
            fromAccount.Balance -= transactionAmount;
            toAccount.Balance += transactionAmount;

            // Create transaction record
            Transaction transaction = new Transaction
            {
                From = fromAccount.Name,
                To = toAccount.Name,
                Date = DateTime.Now,
                Sum = transactionAmount,
                Balance = fromAccount.Balance
            };

            transactions.Add(transaction);

            UpdateTransactionGrid();
        }

        private void UpdateTransactionGrid()
        {
            transactions.Reverse();
            TransactionGrid.ItemsSource = null;
            TransactionGrid.ItemsSource = transactions;
        }

        public void AddAccount_Click(object sender, RoutedEventArgs e)
        {
            NewAccountPopup.IsOpen = true;
        }

        public void AddAccountOKButton_Click(object sender, RoutedEventArgs e)
        {
            string accountName = AddAccountNameTextbox.Text;

            decimal initialBalance;
            if (!decimal.TryParse(AddAccountBalanceTextbox.Text, out initialBalance))
            {
                MessageBox.Show("Please enter a valid initial balance.");
                return;
            }

            // Create a new account instance
            Account newAccount = new Account(accountName, initialBalance);

            accounts.Add(newAccount);

            // Refresh the ComboBox to reflect the changes
            AccountCombobox.ItemsSource = null;
            AccountCombobox.ItemsSource = accounts;

            NewAccountPopup.IsOpen = false;

            MessageBox.Show("New account added successfully.");
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

        public void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            string username = IDTextbox.Text;
            string password = PasswordBox.Password;

            // Perform authentication (example: check against hardcoded credentials)
            if (username == "admin" && password == "password")
            {
                TransactionGrid.Visibility = Visibility.Visible;
                AccountCombobox.Visibility = Visibility.Visible;
                NewTransactionButton.Visibility = Visibility.Visible;
                AddAccountButton.Visibility = Visibility.Visible;

                LogInStackpanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.");
            }
        }
    }
}

using LiveCharts;
using LiveCharts.Wpf;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;


namespace WalletMentor
{

    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        
        private MongodbService _databaseService;
        private IMongoCollection<MonthlyReport> _pennyCollection;
        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            _databaseService = new MongodbService();
            _pennyCollection = _databaseService.GetCollection<MonthlyReport>("monthly_report");

            SeriesCollection = new SeriesCollection();
            Labels = new List<string>();

            welcomeBox.Visibility = Visibility.Visible;
            incomeGrid.Visibility = Visibility.Hidden;
            expenseGrid.Visibility = Visibility.Hidden;
            budgetGrid.Visibility = Visibility.Hidden;
            overviewGrid.Visibility = Visibility.Hidden;
            MonthCollection.Add("Month");
            YearCollection.Add("Year");
            MonthAdder();
            YearAdder();

        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        private ObservableCollection<string> _monthCollection = new ObservableCollection<string>();
        private ObservableCollection<string> _yearCollection = new ObservableCollection<string>();
        

        private void MonthAdder()
        {
            MonthCollection.Add("January");
            MonthCollection.Add("February");
            MonthCollection.Add("March");
            MonthCollection.Add("April");
            MonthCollection.Add("May");
            MonthCollection.Add("June");
            MonthCollection.Add("July");
            MonthCollection.Add("August");
            MonthCollection.Add("September");
            MonthCollection.Add("October");
            MonthCollection.Add("November");
            MonthCollection.Add("December");
        }

        private void YearAdder()
        {
            int startYear = 2000;
            int endYear = 2100;
            for (var i = startYear; i <= endYear; i++)
            {
              YearCollection.Add(i.ToString());   
            }
        }

        public ObservableCollection<string> MonthCollection
        {
            get => _monthCollection;
            set
            {
                _monthCollection = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> YearCollection
        {
            get => _yearCollection;
            set
            {
                _yearCollection = value;
                OnPropertyChanged();
            }
        }
       

        private void BtnIncomeClick(object sender, RoutedEventArgs e)
        {
            ResetGrids();
            incomeGrid.Visibility = Visibility.Visible;
        }

        

        private void BtnExpanseClick(object sender, RoutedEventArgs e)
        {
            ResetGrids();
            expenseGrid.Visibility = Visibility.Visible;
        }

        private void BtnBudgetClick(object sender, RoutedEventArgs e)
        {
            ResetGrids();
            budgetGrid.Visibility = Visibility.Visible;
        }

        private void BtnStatClick(object sender, RoutedEventArgs e)
        {
            ResetGrids();
            overviewGrid.Visibility = Visibility.Visible;
        }

        private async void BtnAddIncomeClick(object sender, RoutedEventArgs e)
        
        {
            try
            {
                string month = incomeMonth.Text; 
                int year = int.Parse(incomeYear.Text); 
                double income = double.Parse(incomeAmount.textInput.Text);
                string fieldName = "income";
                await _databaseService.InsertOrUpdateField(month, year, income, fieldName, _pennyCollection);
                MessageBox.Show("Income updated successfully!");
                incomeAmount.textInput.Text = "";
                incomeMonth.SelectedIndex = 0;
                incomeYear.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
        
        
        private async void BtnAddExpenseClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string month = expenseMonth.Text;
                int year = int.Parse(expenseYear.Text);
                double expense = double.Parse(expenseAmount.textInput.Text);
                string fieldName = "expense";
                await _databaseService.InsertOrUpdateField(month, year, expense, fieldName,_pennyCollection);
                MessageBox.Show("Expenses updated successfully!");
                expenseAmount.textInput.Text = "";
                expenseMonth.SelectedIndex = 0;
                expenseYear.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
        
        private async void BtnAddBudgetClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string month = budgetMonth.Text;
                int year = int.Parse(budgetYear.Text);
                double budget = double.Parse(budgetAmount.textInput.Text);
                string fieldName = "budget";

                await _databaseService.InsertOrUpdateField(month, year, budget,fieldName, _pennyCollection);
                MessageBox.Show("Budget updated successfully!");
                budgetAmount.textInput.Text = "";
                budgetMonth.SelectedIndex = 0;
                budgetYear.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }


        private void BtnGenerateReportClick(object sender, RoutedEventArgs e)
        {
            try
            {
                int startYear = int.Parse(overviewYearStart.Text);
                int endYear = int.Parse(overviewYearEnd.Text);
                _=FetchAndDisplayData(startYear, endYear);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
            

        }

        public async Task FetchAndDisplayData(int startYear, int endYear)
        {
            var filter = Builders<MonthlyReport>.Filter.Gte(r => r.Year, startYear) & Builders<MonthlyReport>.Filter.Lte(r => r.Year, endYear);
            var results = await _pennyCollection.Find(filter).ToListAsync();
        
            var incomeSeries = new ChartValues<double>();
            var budgetSeries = new ChartValues<double>();
            var expenseSeries = new ChartValues<double>();

            Labels.Clear();
            SeriesCollection.Clear();

            foreach (var year in results)
            {
                Labels.Add(year.Year.ToString());
                incomeSeries.Add(year.Income);
                budgetSeries.Add(year.Budget);
                expenseSeries.Add(year.Expense);
            }
        
            SeriesCollection.Add(new LineSeries
            {
                Title = "Income",
                Values = incomeSeries
            });
            SeriesCollection.Add(new LineSeries
            {
                Title = "Budget",
                Values = budgetSeries
            });
            SeriesCollection.Add(new LineSeries
            {
                Title = "Expense",
                Values = expenseSeries
            });
        
            OnPropertyChanged(nameof(SeriesCollection));
            OnPropertyChanged(nameof(Labels));
        }

        private void ResetGrids()
        {
            welcomeBox.Visibility = Visibility.Hidden;
            incomeGrid.Visibility = Visibility.Hidden;
            expenseGrid.Visibility = Visibility.Hidden;
            budgetGrid.Visibility = Visibility.Hidden;
            overviewGrid.Visibility = Visibility.Hidden;
        }

        
    }
}

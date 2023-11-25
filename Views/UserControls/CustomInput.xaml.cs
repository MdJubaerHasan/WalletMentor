using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace WalletMentor.Views.UserControls
{
    /// <summary>
    /// Interaction logic for CustomInput.xaml
    /// </summary>
    public partial class CustomInput : UserControl, INotifyPropertyChanged
    {
        public CustomInput()
        {
            DataContext = this;
            InitializeComponent();
        }

        private string placeholder;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Placeholder
        {
            get { return placeholder; }
            set
            {
                placeholder = value;
                // PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Placeholder"));
                OnPropertyChanged();
            }
        }


        private void BtnClear_OnClick(object sender, RoutedEventArgs e)
        {
            textInput.Clear();
            textInput.Focus();
        }

        private void TextInput_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(textInput.Text))
            {
                tblPlaceHolder.Visibility = Visibility.Visible;

            }
            else
            {
                tblPlaceHolder.Visibility = Visibility.Hidden;
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ChatBotApp.ViewModels;

namespace ChatBotApp
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            DataContext = _viewModel;
        }

        private void ApiKeyBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                _viewModel.ApiKey = passwordBox.Password;
            }
        }

        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && _viewModel.SendCommand.CanExecute(null))
            {
                _viewModel.SendCommand.Execute(null);
            }
        }
    }
}
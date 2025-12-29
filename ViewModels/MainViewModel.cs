using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using ChatBotApp.Models;
using ChatBotApp.Services;

namespace ChatBotApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly LlmService _llmService;
        private string _userInput;
        private bool _isBusy;
        private string _apiKey;

        public ObservableCollection<ChatMessage> Messages { get; } = new ObservableCollection<ChatMessage>();

        public string UserInput
        {
            get => _userInput;
            set { _userInput = value; OnPropertyChanged(); }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); }
        }

        public string ApiKey
        {
            get => _apiKey;
            set 
            { 
                _apiKey = value; 
                _llmService.SetApiKey(value);
                OnPropertyChanged(); 
            }
        }

        public ICommand SendCommand { get; }

        public MainViewModel()
        {
            _llmService = new LlmService();
            SendCommand = new RelayCommand(async _ => await SendMessageAsync(), _ => !string.IsNullOrWhiteSpace(UserInput) && !IsBusy);
            
            // Welcome message
            Messages.Add(new ChatMessage("Hello! I'm your AI Chatbot. Please enter your Gemini API Key in the settings at the top to start chatting.", false));
        }

        private async Task SendMessageAsync()
        {
            var messageText = UserInput;
            UserInput = string.Empty;
            IsBusy = true;

            Messages.Add(new ChatMessage(messageText, true));

            var response = await _llmService.GetResponseAsync(messageText);

            Messages.Add(new ChatMessage(response, false));
            IsBusy = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

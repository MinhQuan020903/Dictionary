using Dictionary.Model;
using Dictionary.Model.API;
using Dictionary.View;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Dictionary.ViewModel
{
    public class RandomWordPageViewModel : BaseViewModel
    {

        private ILoggerFactory loggerFactory;
        private ILogger<RandomWordPageViewModel> logger;

        private ObservableCollection<string> _characters;

        public ObservableCollection<string> Characters
        {
            get { return _characters; }
            set
            {
                _characters = value;
                OnPropertyChanged(nameof(Characters));
            }
        }
        private ObservableCollection<string> _randomWords;

        public ObservableCollection<string> RandomWords
        {
            get { return _randomWords; }
            set
            {
                _randomWords = value;
                OnPropertyChanged(nameof(RandomWords));
            }
        }

        private Visibility _isRandomWordsVisible;
        public Visibility IsRandomWordsVisible
        {
            get { return _isRandomWordsVisible; }
            set
            {
                _isRandomWordsVisible = value;
                OnPropertyChanged(nameof(IsRandomWordsVisible));
            }
        }

        private Visibility _isLoading;
        public Visibility IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        private NavigationService _navigationService = NavigationServiceModel.NavigationService;
        public ICommand SelectCharacterCommand { get; set; }
        public ICommand SearchCharacterCommand { get; set; }
        public RandomWordPageViewModel()
        {

            SelectCharacterCommand = new RelayCommand<object>(SelectCharacterCommandCanExecute, SelectCharacterCommandExecute);
            SearchCharacterCommand = new RelayCommand<object>(SearchCharacterCommandCanExecute, SearchCharacterCommandExecute);
            Characters = new ObservableCollection<string>(Enumerable.Range('A', 26).Select(c => ((char)c).ToString()));

            //Create logger object
            logger = LoggerProvider.CreateLogger<RandomWordPageViewModel>();

            IsRandomWordsVisible = Visibility.Hidden;
            IsLoading = Visibility.Hidden;

        }

        private void SelectCharacterCommandExecute(object obj)
        {
            GetRandomWordsFromStartCharacter(obj.ToString(), logger);
            IsRandomWordsVisible = Visibility.Hidden;
            IsLoading = Visibility.Visible;
        }



        private bool SelectCharacterCommandCanExecute(object obj)
        {
            return true;
        }

        private async void GetRandomWordsFromStartCharacter(string character, ILogger<RandomWordPageViewModel> logger)
        {
            List<string> randomWordsList = await RandomWordAPI.GetRandomWordsFromStartCharacter(character, logger);
            if (randomWordsList.Count > 0)
            {
                IsRandomWordsVisible = Visibility.Visible;
                IsLoading = Visibility.Hidden;
                RandomWords = new ObservableCollection<string>(randomWordsList);
            }
            else
            {
                IsRandomWordsVisible = Visibility.Hidden;
                IsLoading = Visibility.Hidden;
                MessageBox.Show("Lỗi trong quá trình tải dữ liệu chữ");
            }

        }


        private bool SearchCharacterCommandCanExecute(object obj)
        {
            return true;
        }
        public void NavigateCommand(object p)
        {
            NavigationServiceModel.NavigationService.Navigate(p);
        }

        private void SearchCharacterCommandExecute(object obj)
        {
            NavigateCommand(new MainPageView(obj.ToString()));
        }
    }
}

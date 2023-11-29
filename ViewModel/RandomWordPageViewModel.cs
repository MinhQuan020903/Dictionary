using Dictionary.Model;
using Dictionary.Model.API;
using Dictionary.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Dictionary.ViewModel
{
    public class RandomWordPageViewModel : BaseViewModel
    {
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

        private NavigationService _navigationService = NavigationServiceModel.NavigationService;
        public ICommand SelectCharacterCommand { get; set; }
        public ICommand SearchCharacterCommand { get; set; }
        public RandomWordPageViewModel()
        {

            SelectCharacterCommand = new RelayCommand<object>(SelectCharacterCommandCanExecute, SelectCharacterCommandExecute);
            SearchCharacterCommand = new RelayCommand<object>(SearchCharacterCommandCanExecute, SearchCharacterCommandExecute);
            Characters = new ObservableCollection<string>(Enumerable.Range('A', 26).Select(c => ((char)c).ToString()));


        }

        private void SelectCharacterCommandExecute(object obj)
        {
            GetRandomWordsFromStartCharacter(obj.ToString());
        }



        private bool SelectCharacterCommandCanExecute(object obj)
        {
            return true;
        }

        private async void GetRandomWordsFromStartCharacter(string character)
        {
            List<string> randomWordsList = await RandomWordAPI.GetRandomWordsFromStartCharacter(character);
            RandomWords = new ObservableCollection<string>(randomWordsList);
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
            MainPageViewModel mainPageViewModel = new MainPageViewModel(obj.ToString());
            NavigateCommand(new MainPageView());
        }
    }
}

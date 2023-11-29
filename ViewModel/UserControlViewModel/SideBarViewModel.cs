using Dictionary.Model;
using Dictionary.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Dictionary.ViewModel.UserControlViewModel
{
    public class SidelBarViewModel : BaseViewModel
    {
        public ICommand NavigateTranslateParagraphPageCommand { get; set; }
        public ICommand NavigatMainPageCommand { get; set; }
        public ICommand RandomWordPageCommand { get; set; }
        private NavigationService _navigationService = NavigationServiceModel.NavigationService;

        public void NavigateCommand(object p)
        {
            NavigationServiceModel.NavigationService.Navigate(p);
        }

        public SidelBarViewModel()
        {
            NavigatMainPageCommand = new RelayCommand<object>(p => true, p =>
            {
                NavigateCommand(new MainPageView());
            });

            NavigateTranslateParagraphPageCommand = new RelayCommand<object>(p => true, p =>
            {
                NavigateCommand(new TranslateParagraphPageView());
            });
            RandomWordPageCommand = new RelayCommand<object>(p => true, p =>
            {
                NavigateCommand(new RandomWordPageView());
            });

        }
    }
}

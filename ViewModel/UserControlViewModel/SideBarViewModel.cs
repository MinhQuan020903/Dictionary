using Dictionary.Model;
using Dictionary.View;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Dictionary.ViewModel.UserControlViewModel
{
    public class SidelBarViewModel : BaseViewModel
    {
        public ICommand NavigateTranslateParagraphPageCommand { get; set; }
        public ICommand NavigatMainPageCommand { get; set; }
        public ICommand RandomWordPageCommand { get; set; }

        public SidelBarViewModel()
        {
            NavigatMainPageCommand = new RelayCommand<object>(p => true, p =>
            {
                Navigate(new MainPageView());
            });

            NavigateTranslateParagraphPageCommand = new RelayCommand<object>(p => true, p =>
            {
                Navigate(new TranslateParagraphPageView());
            });

            RandomWordPageCommand = new RelayCommand<object>(p => true, p =>
            {
                // Corrected the URI here
                Navigate(new RandomWordPageView());
            });
        }


        private void Navigate(Page page)
        {
            if (Application.Current.MainWindow is IndexWindow mainWindow)
            {
                mainWindow.IndexFrame.Navigate(page);
            }
        }
    }
}

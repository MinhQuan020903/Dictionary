using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Unsplasharp;
using static System.Net.Mime.MediaTypeNames;

namespace Dictionary.ViewModel
{
    public class MainViewModel : BaseViewModel

    {
        //Data binding for text box and image
        private string _text;
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged(nameof(Text));
            }
        }
        private string _image;
        public string Image
        {
            get => _image;
            set
            {
                _image = value;
                OnPropertyChanged(nameof(Image));
            }
        }
        public ICommand ButtonCommand { get; set; }

        public MainViewModel()
        {
            //Init button command
            ButtonCommand = new RelayCommand<object>(ButtonCommandCanExecute, ButtonCommandExecute);

        }

        private bool ButtonCommandCanExecute(object obj)
        {
            return true;
        }

        private async void ButtonCommandExecute(object obj)
        {
            //Check if text box is empty
            if (Text == null || Text == "")
            {
                MessageBox.Show("Please enter a word");
            } else
            {
                string unsplashApiKey = App.Current.Resources["UnsplashApiKey"].ToString();
                //Connect to Unsplash API with API credential
                var client = new UnsplasharpClient(unsplashApiKey);
                try
                {
                    //Search images based from text
                    var photosFound = await client.SearchPhotos(Text, 1, 1);
                    //Get first image
                    string url = photosFound[0].Urls.Small;
                    //Assign imageUrl to Image component
                    Image = url;

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

    }
}

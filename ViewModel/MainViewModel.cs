using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Dictionary.ViewModel
{
    public class MainViewModel : BaseViewModel

    {
        public ICommand ButtonCommand { get; set; }
        private string _image;
        public string Image
        {
            get { return _image; }
            set
            {
                _image = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            ButtonCommand = new RelayCommand<object>(ButtonCommandCanExecute, ButtonCommandExecute);
        }

        private bool ButtonCommandCanExecute(object obj)
        {
            return true;
        }

        private void ButtonCommandExecute(object obj)
        {
            if (obj != null)
            {
                string textFromTextBox = obj.ToString();
                MessageBox.Show(textFromTextBox);
                
            }
        }
    }
}

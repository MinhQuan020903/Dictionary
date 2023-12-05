using System;
using System.Windows;
using System.Windows.Controls;

namespace Dictionary.View
{
    /// <summary>
    /// Interaction logic for TranslateParagraphPageView.xaml
    /// </summary>
    public partial class TranslateParagraphPageView : Page
    {
        public TranslateParagraphPageView()
        {
            this.Resources.MergedDictionaries.Add(new ResourceDictionary());
            this.Resources.MergedDictionaries[0].Source = new Uri("Resource/String.xaml", UriKind.Relative);


            InitializeComponent();
        }
    }
}

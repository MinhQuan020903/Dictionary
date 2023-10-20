using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary.Model.Word
{
    public class WordListViewData
    {
        public string Synonym { get; set; }
        public string PartOfSpeech { get; set; }
        public string Example { get; set; }


        public WordListViewData(string synonym, string partOfSpeech, string example)
        {
            Synonym = synonym;
            PartOfSpeech = partOfSpeech;
            Example = example;
        }
    }


    public class WordListView
    {
        public ObservableCollection<WordListViewData> ListViewItems { get; set; }

        public WordListView()
        {
            ListViewItems = new ObservableCollection<WordListViewData>();
        }

        public void PopulateWordListView(Dictionary<WordSynonym, string> ExamplesOfSynonym)
        {
            ListViewItems.Clear();

            foreach (var entry in ExamplesOfSynonym)
            {
                WordSynonym synonym = entry.Key;
                string example = entry.Value;

                // Create a new ListViewData item and set its properties
                WordListViewData listViewItem = new WordListViewData(synonym.GetSynonym(), synonym.GetPartOfSpeech(), example);

                // Add the item to the list
                ListViewItems.Add(listViewItem);
            }
        }
    }

}

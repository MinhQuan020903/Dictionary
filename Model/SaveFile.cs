using Dictionary.Model.JSON;
using Dictionary.ViewModel;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace Dictionary.Model
{
    public class SaveFile
    {
        private static string _paragraphFilePath = "SavedParagraph.json";
        private static string _wordFilePath = "SavedWord.json";
        private static string baseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "En-Vi Dictionary");
        private static string logFolderPath = Path.Combine(baseDirectory, "Log");

        public static void SaveTranslatedItemsToFile(ObservableCollection<SavedWord> SavedWords)
        {
            try
            {
                if (!Directory.Exists(logFolderPath))
                {
                    Directory.CreateDirectory(logFolderPath);
                }


                // Serialize and save translated items to a file
                string translatedItemsJson = JsonConvert.SerializeObject(SavedWords);

                string fullPath = Path.Combine(logFolderPath, _wordFilePath);
                File.WriteAllText(fullPath, translatedItemsJson);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static ObservableCollection<SavedWord> LoadTranslatedItemsFromFile()
        {
            try
            {
                // Check if the "Log" folder exists, if not, create it

                string fullPath = Path.Combine(logFolderPath, _wordFilePath);
                if (File.Exists(fullPath))
                {
                    string translatedItemsJson = File.ReadAllText(fullPath);
                    return new ObservableCollection<SavedWord>(JsonConvert.DeserializeObject<List<SavedWord>>(translatedItemsJson));

                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public static void SaveTranslatedParagraphToFile(ObservableCollection<SavedParagraph> SavedParagraphs)
        {
            try
            {
                if (!Directory.Exists(logFolderPath))
                {
                    Directory.CreateDirectory(logFolderPath);
                }


                string fullPath = Path.Combine(logFolderPath, _paragraphFilePath);
                // Serialize and save translated items to a file
                string translatedItemsJson = JsonConvert.SerializeObject(SavedParagraphs);
                File.WriteAllText(fullPath, translatedItemsJson);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static ObservableCollection<SavedParagraph> LoadSavedParagraphs()
        {
            try
            {
                string fullPath = Path.Combine(logFolderPath, _paragraphFilePath);
                if (File.Exists(fullPath))
                {
                    string translatedItemsJson = File.ReadAllText(fullPath);
                    return new ObservableCollection<SavedParagraph>(JsonConvert.DeserializeObject<List<SavedParagraph>>(translatedItemsJson));
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new ObservableCollection<SavedParagraph>();
        }
    }
}

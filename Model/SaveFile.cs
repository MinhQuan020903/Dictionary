using Dictionary.Model.JSON;
using Dictionary.ViewModel;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary.Model
{
    public class SaveFile
    {
        private static ILoggerFactory loggerFactory;
        public static void SaveTranslatedItemsToFile(string filePath, ObservableCollection<SavedWord> SavedWords)
        {
            try
            {
                // Get the base directory where the application is running

                string baseDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."));
                // Check if the "Log" folder exists, if not, create it
                string logFolderPath = Path.Combine(baseDirectory, "Log");
                if (!Directory.Exists(logFolderPath))
                {
                    Directory.CreateDirectory(logFolderPath);
                }

                // Construct the full file path
                string fullFilePath = Path.Combine(logFolderPath, filePath);

                // Serialize and save translated items to a file
                string translatedItemsJson = JsonConvert.SerializeObject(SavedWords);
                File.WriteAllText(fullFilePath, translatedItemsJson);

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
                string baseDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."));
                // Check if the "Log" folder exists, if not, create it
                string logFolderPath = Path.Combine(baseDirectory, "Log\\SavedWord.json");

                if (File.Exists(logFolderPath))
                {
                    string translatedItemsJson = File.ReadAllText(logFolderPath);
                    return new ObservableCollection<SavedWord>(JsonConvert.DeserializeObject<List<SavedWord>>(translatedItemsJson));

                }
                else
                { }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }
}

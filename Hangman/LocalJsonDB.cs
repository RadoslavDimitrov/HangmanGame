using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hangman
{
    public class LocalJsonDB
    {
        const string filePath = @"db.json";
        public Word GetRandomWordFromJsonDb()
        {
            var source = new List<Word>();

            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                source = JsonSerializer.Deserialize<List<Word>>(json);
            }

            Random rand = new Random();

            int randWordIndex = rand.Next(0, source.Count);

            var wordToReturn = source[randWordIndex];

            return wordToReturn;
        }

        public Word AddNewWordToJsonDb(string name, string description)
        {
            var source = new List<Word>();

            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                source = JsonSerializer.Deserialize<List<Word>>(json);
            }

            var newWord = new Word()
            {
                Id = source.Count + 1,
                Name = name,
                Description = description
            };

            foreach (var word in source)
            {
                if(word.Name == newWord.Name)
                {
                    return null;
                }
            }

            source.Add(newWord);

            string jsonString = JsonSerializer.Serialize(source, new JsonSerializerOptions() { WriteIndented = true });
            using (StreamWriter outputFile = new StreamWriter(filePath))
            {
                outputFile.WriteLine(jsonString);
            }

            return newWord;
        }
    }

    public class Word
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hangman
{
    public class CustomApi
    {
        public Word GetRandomWordFromJsonDb()
        {
            const string filePath = @"db.json";

            var path = Path.Combine(filePath);

            var file = File.Exists(path);

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
    }

    public class Word
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Hangman
{
    class Program
    {
        static void Main(string[] args)
        {
            bool result = StartGame();

            while (result == true)
            {
                result = StartGame();
            }



            //TODO start new game
        }

        private static bool StartGame()
        {
            int menuChooice = ShowMenuScreen();
            var api = new LocalJsonDB();

            while(menuChooice != 1)
            {
                if (menuChooice == 2)
                {
                    //add new word
                    var newWordResult = AddNewWordTemplate(api);
                    ShowNewWordScreen(newWordResult);
                    menuChooice = ShowMenuScreen();
                }
                else if (menuChooice == 3)
                {
                    //exit
                    return false;
                }
            }

            Console.Clear();
            const int maxWrongAnswers = 5;
            int wrongAnswers = 0;
            bool isWinner = false;


            var newWord = api.GetRandomWordFromJsonDb();

            string word = newWord.Name;
            string wordDescription = newWord.Description;

            List<char> wordResult = FillResultList(word);

            Console.WriteLine(DrowBegginingScreen());
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"Word description: {wordDescription}");
            Console.WriteLine();
            Console.WriteLine(GenerateEmptyWord(word.Length, word, wordResult));


            while (true)
            {
                Console.WriteLine("Type a letter that you think, the word has it");

                string isValidLetter = Console.ReadLine();
                bool? isValid = true;
                char answer;

                if (isValidLetter == string.Empty)
                {
                    continue;
                }

                answer = char.Parse(isValidLetter.Substring(0, 1));


                List<int> sameLettersIndex = new List<int>();

                for (int i = 0; i < word.Length; i++)
                {
                    if (word.ToLower()[i] == answer)
                    {
                        sameLettersIndex.Add(i);
                    }
                }



                if (sameLettersIndex.Count == 0)
                {
                    wrongAnswers++;
                    isValid = false;
                }

                for (int i = 0; i < sameLettersIndex.Count; i++)
                {
                    //already has that letter
                    if (word[sameLettersIndex[i]] == wordResult[sameLettersIndex[i]])
                    {
                        //TODO add same letter again to be with warrning message
                        wrongAnswers++;
                        isValid = null;
                        break;
                    }
                    else
                    {
                        wordResult[sameLettersIndex[i]] = word[sameLettersIndex[i]];
                    }
                }

                Console.WriteLine(UpdateScreen(wrongAnswers, word, wordDescription, wordResult, isValid));

                //TODO add method checkIsWinner
                int count = 0;

                for (int i = 0; i < word.Length; i++)
                {
                    if(word[i] == wordResult[i])
                    {
                        count++;
                    }
                }

                if(count == word.Length)
                {
                    isWinner = true;
                    break;
                }


                if (wrongAnswers == maxWrongAnswers)
                {
                    break;
                }
            }

            if (isWinner)
            {
                bool newGameAfterWin = WinScreen();

                if (newGameAfterWin)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            bool newGameAfterLose = LoseScreen();
            if (newGameAfterLose)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void ShowNewWordScreen(Word word)
        {
            Console.Clear();
            Console.WriteLine("Your new word has been added to DB");
            Console.WriteLine();
            Console.WriteLine($"Word - {word.Name}");
            Console.WriteLine();
            Console.WriteLine($"Description - {word.Description}");
            Console.WriteLine();
            Console.WriteLine("Now you and other players can enjoy guesing your word!");
            Console.WriteLine();
            Console.WriteLine("Press any key to return to menu screen!");
            Console.ReadLine();
        }

        public static Word AddNewWordTemplate(LocalJsonDB api)
        {
            Console.Clear();
            Console.WriteLine("Add new word to DB");
            Console.WriteLine();
            Console.WriteLine("Pleace enter word to be added to DB");
            Console.WriteLine();
            
            string wordInput = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(wordInput))
            {
                Console.WriteLine("Pleace enter valid word.");
                wordInput = Console.ReadLine();
            }

            Console.WriteLine();
            Console.WriteLine("Pleace enter word description, but tricky one, so player can guest the word!");
            Console.WriteLine();

            string descriptionInput = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(descriptionInput))
            {
                Console.WriteLine("Pleace enter valid description.");
                descriptionInput = Console.ReadLine();
            }

            return api.AddNewWordToJsonDb(wordInput, descriptionInput);
        }

        private static int ShowMenuScreen()
        {
            Console.Clear();
            Console.WriteLine("Hello to our game Hangman!!!");
            Console.WriteLine();
            Console.WriteLine("Pleace choose what you want to do.");
            Console.WriteLine();
            Console.WriteLine("Start game - type 1");
            Console.WriteLine();
            Console.WriteLine("Add new word to db - type 2");
            Console.WriteLine();
            Console.WriteLine("Exit game - type 3");

            int answer;

            bool input = int.TryParse(Console.ReadLine(), out answer);

            while (input == false || answer < 1 || answer > 3)
            {
                Console.WriteLine("Pleace enter valid number!");

                input = int.TryParse(Console.ReadLine(), out answer);
            }

            return answer;
        }

        private static bool WinScreen()
        {
            Console.WriteLine();
            Console.WriteLine("Congratilations, you made it.");
            Console.WriteLine();
            Console.WriteLine("Do you wanna go to menu screen? Type Y for \"Yes\" or N for \"Exit\"");
            string answer = Console.ReadLine().ToLower();

            if (answer[0] == 'y')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool LoseScreen()
        {
            Console.WriteLine();
            Console.WriteLine("You have been hanged!");
            Console.WriteLine();
            Console.WriteLine("Do you wanna go to menu screen? Type Y for \"Yes\" or N for \"Exit\"");
            string answer = Console.ReadLine().ToLower();

            if (answer[0] == 'y')
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private static string UpdateScreen(int wrongAnswers, string word, string wordDescription, List<char> wordResult, bool? isValidAnswer)
        {
            Console.Clear();
            StringBuilder result = new StringBuilder();


            result.AppendLine();

            result.AppendLine("..........");
            result.AppendLine("----------");
            result.AppendLine(".|...._...");
            result.AppendLine(".|...|_|..");


            if (wrongAnswers == 1)
            {
                result.AppendLine(".|....|...");
                result.AppendLine(".|........");
                result.AppendLine(".|........");
            }
            else if (wrongAnswers == 2)
            {
                result.AppendLine(".|....|...");
                result.AppendLine(".|.../....");
                result.AppendLine(".|........");
            }
            else if (wrongAnswers == 3)
            {
                result.AppendLine(".|....|...");
                result.AppendLine(".|.../.\\..");
                result.AppendLine(".|........");
            }
            else if (wrongAnswers == 4)
            {
                result.AppendLine(".|....|...");
                result.AppendLine(".|.../.\\..");
                result.AppendLine(".|.../....");
            }
            else if (wrongAnswers == 5)
            {
                result.AppendLine(".|....|...");
                result.AppendLine(".|.../.\\..");
                result.AppendLine(".|.../.\\..");
            }
            else if (wrongAnswers == 0)
            {
                result.AppendLine(".|........");
                result.AppendLine(".|........");
                result.AppendLine(".|........");
            }

            result.AppendLine();
            result.AppendLine();
            result.AppendLine($"Word description: {wordDescription}");
            result.AppendLine();

            if (isValidAnswer == null)
            {
                result.AppendLine(DuplicateLetterMessage());
            }
            else if(isValidAnswer == true)
            {
                result.AppendLine(ValidLetterMessage());
            }
            else if(isValidAnswer == false)
            {
                result.AppendLine(InvalidLetterMessage());
            }

            result.AppendLine();
            result.AppendLine(UpdateWord(word.Length, word, wordResult));

            return result.ToString();
        }

        private static string DuplicateLetterMessage()
        {
            return "You already choose that letter, choose another one!";
        }

        private static string ValidLetterMessage()
        {
            return "Well done, word has that letter!";
        }

        private static string InvalidLetterMessage()
        {
            return "Word does not contains that letter";
        }

        private static string UpdateWord(int length, string word, List<char> wordResult)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                sb.Append($"{wordResult[i]} ");
            }

            return sb.ToString();
        }

        private static List<char> FillResultList(string word)
        {
            var result = new List<char>();

            foreach (var ch in word)
            {
                result.Add('_');
            }

            return result;
        }

        private static string GenerateEmptyWord(int wordLength, string word, List<char> result)
        {
            Random random = new Random();
            int randIndex = random.Next(0, wordLength);

            char letter = word[randIndex];

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < wordLength; i++)
            {
                if (word[i] == letter)
                {
                    sb.Append($"{word[i]} ");
                    result[i] = word[i];
                }
                else
                {
                    sb.Append("_ ");
                }
            }

            return sb.ToString();
        }

        private static string DrowBegginingScreen()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("..........");
            sb.AppendLine("----------");
            sb.AppendLine(".|...._...");
            sb.AppendLine(".|...|_|..");
            sb.AppendLine(".|........");
            sb.AppendLine(".|........");
            sb.AppendLine(".|........");

            return sb.ToString();
        }

        
    }
}

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
            Console.Clear();
            const int maxWrongAnswers = 5;
            int wrongAnswers = 0;
            bool isWinner = false;

            //this will be automated from file in local storage!!
            //TODO
            var api = new ConsoleApi();

            var newWord = api.GetRandomWordFromJsonDb();

            string word = newWord.Name;
            string wordDescription = newWord.Description;

            List<char> wordResult = FillResultList(word);

            Console.WriteLine(DrowBegginingScreen());
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(wordDescription);
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

        private static bool WinScreen()
        {
            Console.WriteLine();
            Console.WriteLine("Congratilations, you made it.");
            Console.WriteLine();
            Console.WriteLine("Do you wanna play again? Type Y for \"Yes\" or N for \"No\"");
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
            Console.WriteLine("Do you wanna play again? Type Y for \"Yes\" or N for \"No\"");
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


            result.AppendLine(wordDescription);
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

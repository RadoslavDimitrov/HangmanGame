using System;
using System.Collections.Generic;
using System.Text;

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

            //this will be automated from file in local storage!!
            //TODO
            string word = "Hello";
            string wordDescription = "Something you say to every person you know and meet";

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
                char answer = char.Parse(Console.ReadLine().Substring(0,1));

                //IndexOf not found = -1;
                //Contains("string", startIndex)


                if (word.ToLower().Contains(answer))
                {
                    int index = word.ToLower().IndexOf(answer);
                    if(wordResult[index] != answer)
                    {
                        wordResult[index] = answer;
                    }

                    //TODO case double letter
                    //TODO alreagy have this letter
                    //TODO update word showing

                    Console.WriteLine(UpdateScreen(wrongAnswers, word, wordDescription, wordResult));


                }
                else
                {
                    //we update screen with one more wrong answers
                    wrongAnswers++;
                    Console.WriteLine(UpdateScreen(wrongAnswers, word, wordDescription, wordResult));
                }

                if (wrongAnswers == maxWrongAnswers)
                {
                    break;
                }
            }

            bool newGame = LoseScreen();
            if (newGame)
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

            if(answer[0] == 'y')
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private static string UpdateScreen(int wrongAnswers, string word, string wordDescription, List<char> wordResult)
        {
            StringBuilder result = new StringBuilder();

            result.AppendLine(DrowBegginingScreen());
            result.AppendLine();
            result.AppendLine();
            result.AppendLine(wordDescription);
            result.AppendLine();
            result.AppendLine(UpdateWord(word.Length, word, wordResult));

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
            else if(wrongAnswers == 2)
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
            else if(wrongAnswers == 0)
            {
                result.AppendLine(".|........");
                result.AppendLine(".|........");
                result.AppendLine(".|........");
            }

            return result.ToString();
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

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < wordLength; i++)
            {
                if(i == randIndex)
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

using System;
using System.Collections.Generic;
using System.Text;

namespace Hangman
{
    class Program
    {
        static void Main(string[] args)
        {
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
                char answer = char.Parse(Console.ReadLine());

                if (word.Contains(answer))
                {
                    //we have a match
                }
                else
                {
                    //we update screen with one more wrong answers
                    wrongAnswers++;
                    Console.WriteLine(UpdateWorngAnswerScreen(wrongAnswers));
                }

                if(wrongAnswers == maxWrongAnswers)
                {
                    break;
                }
            }
  
        }

        private static string UpdateWorngAnswerScreen(int wrongAnswers)
        {
            StringBuilder result = new StringBuilder();
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

            return result.ToString();
        }

        private static List<char> FillResultList(string word)
        {
            var result = new List<char>();

            foreach (var ch in word)
            {
                result.Add('0');
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

using System;
using System.Collections.Generic;
using WordUnscrambler.Workers;
using WordUnscrambler.Data;
using System.Linq;
using static System.Console;

namespace WordUnscrambler
{
    class Program
    {
        private static readonly WordMatcher _wordMatcher = new WordMatcher();
        private static readonly FileReader _fileReader = new FileReader();
        private const string wordListFileName = "wordlist.txt";
        static void Main(string[] args)
        {
            bool continueWordUnscrambler = true;
            do
            {
                WriteLine("Please enter the option - M for Manual and F for File");
                var option = ReadLine() ?? string.Empty;

                switch (option.ToUpper())
                {
                    case "F":
                        Write("Enter scrambled word file name: ");
                        ExecuteScrambledWordsInFileScenario();
                        break;
                    case "M":
                        Write("Enter scrambled words manualy: ");
                        ExecuteScrambledWordsManualEntryScenario();
                        break;
                    default:
                        Write("Option was not recognised.");
                        break;
                }

                var continueDecision = string.Empty;
                do
                {
                    WriteLine("Do you want to continue? Y/N");
                    continueDecision = ReadLine() ?? string.Empty;

                } while (!continueDecision.Equals("Y", StringComparison.OrdinalIgnoreCase) && !continueDecision.Equals("N", StringComparison.OrdinalIgnoreCase));

                continueWordUnscrambler = continueDecision.Equals("Y", StringComparison.OrdinalIgnoreCase);

            } while (continueWordUnscrambler);
        }

        private static void ExecuteScrambledWordsManualEntryScenario()
        {
            var manualInput = ReadLine() ?? string.Empty;
            string[] scrambledWords = manualInput.Split(',');
            DisplayMatchedUnscrambledWords(scrambledWords);
        }        

        private static void ExecuteScrambledWordsInFileScenario()
        {
            var fileName = ReadLine() ?? string.Empty;
            string[] scrambledWords = _fileReader.Read(fileName);
            DisplayMatchedUnscrambledWords(scrambledWords);
        }

        private static void DisplayMatchedUnscrambledWords(string[] scrambledWords)
        {
            string[] wordList = _fileReader.Read(wordListFileName);

            List<MatchedWord> matchedWords = _wordMatcher.Match(scrambledWords, wordList);

            if (matchedWords.Any())
            {
                foreach (var matchedWord in matchedWords)
                {
                    WriteLine("Match found for {0}: {1}", matchedWord.ScrambledWord, matchedWord.Word);
                }                
            }
            else
            {
                WriteLine("No matches have been found.");
            }
        }
    }
}

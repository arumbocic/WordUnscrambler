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
        static void Main(string[] args)
        {
            try
            {
                bool continueWordUnscrambler = true;
                do
                {
                    WriteLine(Constants.OptionsOnHowToEnterScrambledWords);
                    var option = ReadLine() ?? string.Empty;

                    switch (option.ToUpper())
                    {
                        case Constants.File:
                            Write(Constants.EnterScrambledWordsViaFile);
                            ExecuteScrambledWordsInFileScenario();
                            break;
                        case Constants.Manual:
                            Write(Constants.EnterScrambledWordsManually);
                            ExecuteScrambledWordsManualEntryScenario();
                            break;
                        default:
                            Write(Constants.EnterScrambledWordsOptionNotRecognized);
                            break;
                    }

                    var continueDecision = string.Empty;
                    do
                    {
                        WriteLine(Constants.OptionsOnContinuingTheProgram);
                        continueDecision = ReadLine() ?? string.Empty;

                    } while (!continueDecision.Equals(Constants.Yes, StringComparison.OrdinalIgnoreCase) && !continueDecision.Equals(Constants.No, StringComparison.OrdinalIgnoreCase));

                    continueWordUnscrambler = continueDecision.Equals(Constants.Yes, StringComparison.OrdinalIgnoreCase);

                } while (continueWordUnscrambler);
            }
            catch (Exception ex)
            {
                WriteLine(Constants.ErrorProgramWillBeTerminated + ex.Message);
            }
        }

        private static void ExecuteScrambledWordsManualEntryScenario()
        {
            var manualInput = ReadLine() ?? string.Empty;
            string[] scrambledWords = manualInput.Split(',');
            DisplayMatchedUnscrambledWords(scrambledWords);
        }        

        private static void ExecuteScrambledWordsInFileScenario()
        {
            try
            {
                var fileName = ReadLine() ?? string.Empty;
                string[] scrambledWords = _fileReader.Read(fileName);
                DisplayMatchedUnscrambledWords(scrambledWords);
            }
            catch (Exception ex)
            {
                WriteLine(Constants.ErrorScrambledWordsCannotBeLoaded + ex.Message);
            }
        }

        private static void DisplayMatchedUnscrambledWords(string[] scrambledWords)
        {
            string[] wordList = _fileReader.Read(Constants.wordListFileName);

            List<MatchedWord> matchedWords = _wordMatcher.Match(scrambledWords, wordList);

            if (matchedWords.Any())
            {
                foreach (var matchedWord in matchedWords)
                {
                    WriteLine(Constants.MatchFound, matchedWord.ScrambledWord, matchedWord.Word);
                }                
            }
            else
            {
                WriteLine(Constants.MatchNotFound);
            }
        }
    }
}

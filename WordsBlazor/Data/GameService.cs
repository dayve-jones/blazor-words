using System;
using System.Collections.Generic;
using System.Linq;
using WordsBlazor.Models;

namespace WordsBlazor.Data
{
    public class GameService
    {
        private const int WordLimit = 8;

        public Game GetRandomGameData()
        {
            var rnd = new Random();

            var randomWord = WordsStorage.Words[rnd.Next(WordsStorage.Words.Count)];
            var linkedWords = new List<string> { };

            var questSymbols = randomWord.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());

            foreach (var word in WordsStorage.Words)
            {
                var symbols = word.ToArray().GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
                var isResultSuccess = symbols.All(x => CheckSymbol(questSymbols, x.Key, x.Value));

                if (isResultSuccess)
                {
                    linkedWords.Add((word));
                }

                if (linkedWords.Count >= WordLimit)
                    break;
            }

            return new Game
            {
                Characters = Shuffle(randomWord),
                Words = linkedWords.Select(x => new Word { Text = x}).ToList()
            };
        }

        public List<string> GetHints(string text)
        {
            var results = new List<(string, bool)> { };

            var questSymbols = text.ToLower().GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());

            foreach (var word in WordsStorage.Words)
            {
                var symbols = word.ToArray().GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());

                var result = symbols.All(x => CheckSymbol(questSymbols, x.Key, x.Value));

                results.Add((word, result));
            }

            var answers = results.Where(x => x.Item2);

            return answers.Select(x => x.Item1).ToList();
        }

        private bool CheckSymbol(Dictionary<char, int> questSymbols, char symbol, int minCount)
        {
            var isExist = questSymbols.TryGetValue(symbol, out int value);

            return isExist && (value >= minCount);
        }

        public string Shuffle(string str)
        {
            Random num = new Random();

            return new string(str.ToCharArray().
                            OrderBy(s => (num.Next(2) % 2) == 0).ToArray());
        }

    }
}

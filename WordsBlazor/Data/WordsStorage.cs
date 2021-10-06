using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace WordsBlazor.Data
{
    public static class WordsStorage
    {
        public static List<string> Words;

        public static void Init()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"singular");

            Words = System.IO.File.ReadAllLines(path).Where( x=> x.Length >= 3  && x.Length <= 6).Distinct().ToList();
        }        
    }
}

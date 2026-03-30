using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dzshka2
{
    public class Journal<T> where T : IJournalEntry
    {
        private readonly List<T> Jour = new List<T>();
        public void Add(T item)
        {
            Jour.Add(item);
        }
        public List<T> GetAll()
        {
            return Jour;
        }
        public void SaveToFile(string path)
        {
            List<string> lines = new List<string>();
            foreach (T item in Jour)
            {
                lines.Add(item.ToLogLine());
            }
            File.WriteAllLines(path, lines);
        }
    }
}

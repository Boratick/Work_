using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TerminologyApp.Models
{
    public class TermRepository
    {
        private Dictionary<string, Term> terms = new();

        public void AddTerm(Term term)
        {
            if (!terms.ContainsKey(term.Name))
                terms[term.Name] = term;
        }

        public Term GetTerm(string name)
        {
            terms.TryGetValue(name, out var term);
            return term;
        }

        public List<Term> GetAllTerms() => terms.Values.ToList();

        public void SaveToFile(string path)
        {
            var json = JsonSerializer.Serialize(terms.Values.ToList(), new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }

        public void LoadFromFile(string path)
        {
            if (!File.Exists(path)) return;

            var json = File.ReadAllText(path);
            var loadedTerms = JsonSerializer.Deserialize<List<Term>>(json);
            terms = loadedTerms.ToDictionary(t => t.Name, t => t);
        }
    }
}
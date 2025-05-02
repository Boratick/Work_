using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminologyApp.Models
{
    public class TermNavigator
    {
        private readonly TermRepository _repo;

        public TermNavigator(TermRepository repo)
        {
            _repo = repo;
        }

        public List<Term> GetChain(string startTerm)
        {
            var result = new List<Term>();
            var visited = new HashSet<string>();
            Traverse(startTerm, result, visited);
            return result;
        }

        private void Traverse(string name, List<Term> result, HashSet<string> visited)
        {
            if (visited.Contains(name)) return;
            visited.Add(name);

            var term = _repo.GetTerm(name);
            if (term != null)
            {
                result.Add(term);
                foreach (var refName in term.References)
                    Traverse(refName, result, visited);
            }
        }
    }
}

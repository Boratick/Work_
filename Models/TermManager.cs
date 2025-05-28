using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TerminologyApp.Models;

namespace TerminologyApp.Models
{
    public static class TermManager
    {
        private static List<Term> termList = new List<Term>();

        public static List<Term> Terms => termList;

        public static void GetTerms()
        {
            termList.Clear();
            if (Directory.Exists("Terms"))
            {
                foreach (string file in Directory.GetFiles("Terms", "*.json"))
                {
                    try
                    {
                        string json = File.ReadAllText(file);
                        Term term = JsonSerializer.Deserialize<Term>(json);
                        if (term != null)
                        {
                            termList.Add(term);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        public static void SaveTerm(Term term)
        {
            termList.Add(term);
            string path = Path.Combine("Terms", $"{term.Name}.json");
            Directory.CreateDirectory("Terms");
            File.WriteAllText(path, JsonSerializer.Serialize(term, new JsonSerializerOptions { WriteIndented = true }));
        }

        public static (bool Success, string Message) RemoveTerm(string termName)
        {
            Term term = termList.Find(t => t.Name == termName);
            if (term == null)
            {
                return (false, $"Термін '{termName}' не знайдено.");
            }

            string path = Path.Combine("Terms", $"{termName}.json");
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                    termList.Remove(term);
                    return (true, $"Термін '{termName}' видалено.");
                }
                else
                {
                    termList.Remove(term);
                    return (true, $"Термін '{termName}' видалено з пам'яті, файл не знайдено.");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Помилка видалення терміна '{termName}': {ex.Message}");
            }
        }

        public static (bool Success, string Message) UpdateTerm(string originalName, Term updatedTerm)
        {
            Term existingTerm = termList.Find(t => t.Name == originalName);
            if (existingTerm == null)
            {
                return (false, $"Термін '{originalName}' не знайдено.");
            }

            try
            {
                
                string oldPath = Path.Combine("Terms", $"{originalName}.json");
                if (File.Exists(oldPath) && originalName != updatedTerm.Name)
                {
                    File.Delete(oldPath);
                }

               
                termList.Remove(existingTerm);
                termList.Add(updatedTerm);

                string newPath = Path.Combine("Terms", $"{updatedTerm.Name}.json");
                Directory.CreateDirectory("Terms");
                File.WriteAllText(newPath, JsonSerializer.Serialize(updatedTerm, new JsonSerializerOptions { WriteIndented = true }));

                return (true, $"Термін '{updatedTerm.Name}' оновлено.");
            }
            catch (Exception ex)
            {
                return (false, $"Помилка оновлення терміна '{updatedTerm.Name}': {ex.Message}");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace BookHub.DataAccess.Models
{
   public static class DataStorage
   {
        public static void SaveData<T>(string filePath, List<T> data)
        {
            try
            {
                string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Storage Error]: Failed to save data to {filePath}. {ex.Message}");
                Console.ResetColor();
            }
        }

        public static List<T> LoadData<T>(string filePath)
        {
            try
            {
                if (!File.Exists(filePath)) return new List<T>();

                string json = File.ReadAllText(filePath);
                if (string.IsNullOrWhiteSpace(json)) return new List<T>();

                return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            }
            catch (JsonException)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"[Storage Warning]: File {filePath} was corrupted. Initializing fresh dataset.");
                Console.ResetColor();
                return new List<T>();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Storage Error]: Could not read {filePath}. {ex.Message}");
                Console.ResetColor();
                return new List<T>();
            }
        }
    }
}

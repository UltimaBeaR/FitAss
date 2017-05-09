using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace FitAssWPF
{
   public class ModelRoot
   {
      public ModelRoot()
      {
         Products = new List<Product>();
         Days = new List<Day>();
      }

      public static ModelRoot Instance
         => _instance ?? (_instance = Load() ?? new ModelRoot());

      public static string SaveFolderPath
         => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FitAss");

      [JsonProperty(ItemIsReference = true)]
      public List<Product> Products { get; set; }

      public List<Day> Days { get; set; }

      public void Save()
      {
         var save = JsonConvert.SerializeObject(this, Formatting.Indented);

         Directory.CreateDirectory(SaveFolderPath);

         File.WriteAllText(Path.Combine(SaveFolderPath, saveFileName), save);
      }

      public static ModelRoot Load()
      {
         string json;
         try
         {
            json = File.ReadAllText(Path.Combine(SaveFolderPath, saveFileName));
         }
         catch
         {
            return null;
         }

         return JsonConvert.DeserializeObject<ModelRoot>(json);
      }

      private static ModelRoot _instance;

      private const string saveFileName = "save.json";
   }
}

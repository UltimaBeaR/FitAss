using System.Collections.Generic;

namespace FitAssWPF
{
   public class Day
   {
      public Day()
      {
         FoodIntakes = new List<FoodIntake>();
      }

      public string Name { get; set; }
      public List<FoodIntake> FoodIntakes { get; set; }
   }
}

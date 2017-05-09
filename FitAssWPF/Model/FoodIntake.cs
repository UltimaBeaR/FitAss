using System;
using System.Collections.Generic;

namespace FitAssWPF
{
   public class FoodIntake
   {
      public FoodIntake()
      {
         Products = new List<ProductInFoodIntake>();
      }

      public string Name { get; set; }

      public TimeSpan TimeOfDay { get; set; }

      public List<ProductInFoodIntake> Products { get; set; }
   }
}

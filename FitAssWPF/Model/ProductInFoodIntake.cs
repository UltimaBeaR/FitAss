using Newtonsoft.Json;

namespace FitAssWPF
{
   public class ProductInFoodIntake
   {
      [JsonProperty(IsReference = true)]
      public Product Product { get; set; }

      public float CookedMass { get; set; }
   }
}

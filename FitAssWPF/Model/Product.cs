namespace FitAssWPF
{
   public class Product
   {
      public Product()
      {
         NutritionFacts = new NutritionFacts();
      }

      /// <summary>
      /// Полное название продукта
      /// </summary>
      public string FullProductName { get; set; }

      /// <summary>
      /// Название блюда (То есть название уже приготовленного продукта)
      /// </summary>
      public string DishName { get; set; }

      /// <summary>
      /// Дополнительная информация, такая как способ приготовления, примечания и т.д.
      /// </summary>
      public string Info { get; set; }

      /// <summary>
      /// Пищевая ценность продукта на 100г (сухих)
      /// </summary>
      public NutritionFacts NutritionFacts { get; set; }

      /// <summary>
      /// Масса до готовки
      /// </summary>
      public float MassBeforeCooking { get; set; } = 1f;

      /// <summary>
      /// Масса после готовки
      /// </summary>
      public float MassAfterCooking { get; set; } = 1f;

      /// <summary>
      /// Размер порции (в готовой массе)
      /// </summary>
      public float CookedMassPerServing { get; set; }

      /// <summary>
      /// Имя порции (штука (шт.) например)
      /// </summary>
      public string ServingName { get; set; }
   }
}

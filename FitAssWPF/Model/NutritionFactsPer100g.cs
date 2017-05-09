namespace FitAssWPF
{
   /// <summary>
   /// Пищевая ценность на 100г продукта
   /// </summary>
   public class NutritionFacts
   {
      /// <summary>
      /// На сколько грамм идет измерение (обычно на 100г, но могут быть исключения, если замер идет на порцию например)
      /// Еще бывает когда замер идет на мл. но это пока не рассматриваю.
      /// </summary>
      public float MeasuringMass { get; set; } = 100f;

      /// <summary>
      /// Белки в г
      /// </summary>
      public float Protein { get; set; }

      /// <summary>
      /// Жиры в г
      /// </summary>
      public float Fat { get; set; }

      /// <summary>
      /// Углеводы в г
      /// </summary>
      public float Hydrocarbonate { get; set; }

      /// <summary>
      /// Энергия в ккал
      /// </summary>
      public float Energy { get; set; }
   }
}

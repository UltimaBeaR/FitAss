using System;
using ReactiveUI;

namespace FitAssWPF
{
   public class ProductVM : ViewModel, IModelHolder<Product>
   {
      private ProductVM(Product model)
      {
         Model = model;

         _holderImpl = new ModelHolderImplementation<ProductVM, Product>(this);

         this.WhenAnyValue(_ => _.FullProductName, _ => _.DishName)
            .Subscribe(delegate { this.RaisePropertyChanged(nameof(DisplayName)); });
      }

      static ProductVM() {
         ModelHolderImplementation<ProductVM, Product>.DefaultCreateNewModelHolderDelegate =
            model => new ProductVM(model);
      }

      public Product Model { get; }

      public string DisplayName
         => Model.DishName ?? Model.FullProductName ?? "Продукт";

      public string FullProductName {
         get => Model.FullProductName;
         set {
            Model.FullProductName = value;
            if (Model.FullProductName == "")
               Model.FullProductName = null;

            this.RaisePropertyChanged();
         }
      }

      public string DishName {
         get => Model.DishName;
         set {
            Model.DishName = value;
            if (Model.DishName == "")
               Model.DishName = null;

            this.RaisePropertyChanged();
         }
      }

      public string Info {
         get => Model.Info;
         set { Model.Info = value; this.RaisePropertyChanged(); }
      }

      public float MeasuringMass {
         get => Model.NutritionFacts.MeasuringMass;
         set { Model.NutritionFacts.MeasuringMass = value; this.RaisePropertyChanged(); }
      }

      public float Energy {
         get => Model.NutritionFacts.Energy;
         set { Model.NutritionFacts.Energy = value; this.RaisePropertyChanged(); }
      }

      public float Protein {
         get => Model.NutritionFacts.Protein;
         set { Model.NutritionFacts.Protein = value; this.RaisePropertyChanged(); }
      }

      public float Fat {
         get => Model.NutritionFacts.Fat;
         set { Model.NutritionFacts.Fat = value; this.RaisePropertyChanged(); }
      }

      public float Hydrocarbonate {
         get => Model.NutritionFacts.Hydrocarbonate;
         set { Model.NutritionFacts.Hydrocarbonate = value; this.RaisePropertyChanged(); }
      }

      public float MassBeforeCooking {
         get => Model.MassBeforeCooking;
         set { Model.MassBeforeCooking = value; this.RaisePropertyChanged(); }
      }

      public float MassAfterCooking {
         get => Model.MassAfterCooking;
         set { Model.MassAfterCooking = value; this.RaisePropertyChanged(); }
      }

      public float CookedMassPerServing {
         get => Model.CookedMassPerServing;
         set { Model.CookedMassPerServing = value; this.RaisePropertyChanged(); }
      }

      public string ServingName {
         get => Model.ServingName;
         set { Model.ServingName = value; this.RaisePropertyChanged(); }
      }

      public bool MeasuresAsServingCount
         => !string.IsNullOrWhiteSpace(Model.ServingName);

      public float GetEnergyCalculatedFromPFH()
         => Protein * 4f + Fat * 9f + Hydrocarbonate * 4f;

      private ModelHolderImplementation<ProductVM, Product> _holderImpl;
   }
}

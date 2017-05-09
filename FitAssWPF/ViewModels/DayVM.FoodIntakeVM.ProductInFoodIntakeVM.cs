using System.Collections.Generic;
using System.Linq;
using ReactiveUI;
using System;

namespace FitAssWPF
{
   public partial class DayVM
   {
      public partial class FoodIntakeVM : ViewModel, IModelHolder<FoodIntake>
      {
         public class ProductInFoodIntakeVM : ViewModel, IModelHolder<ProductInFoodIntake>
         {
            private ProductInFoodIntakeVM(ProductInFoodIntake model)
            {
               Model = model;

               _holderImpl = new ModelHolderImplementation<ProductInFoodIntakeVM, ProductInFoodIntake>(this);

               this.WhenAnyValue(vm => vm.Product).Subscribe(delegate {
                  _updateServingsSubscriber?.Dispose();

                  if (Product != null)
                  {
                     _updateServingsSubscriber = Product.WhenAnyValue(vm => vm.CookedMassPerServing, vm => vm.MeasuresAsServingCount)
                        .Subscribe(delegate
                        {
                           this.RaisePropertyChanged(nameof(MeasuresAsServingCount));
                           this.RaisePropertyChanged(nameof(CookedMassOrServingCountDispayUnit));
                           this.RaisePropertyChanged(nameof(CookedMassOrServingCount));
                        });
                  }
               });
            }

            static ProductInFoodIntakeVM()
            {
               ModelHolderImplementation<ProductInFoodIntakeVM, ProductInFoodIntake>.DefaultCreateNewModelHolderDelegate =
                  model => new ProductInFoodIntakeVM(model);
            }

            public ProductInFoodIntake Model { get; }

            public ProductVM Product {
               get => Model.Product != null ? GetOrCreateByModel<ProductVM, Product>(Model.Product) : null;
               set { Model.Product = value?.Model; this.RaisePropertyChanged(); }
            }

            public IEnumerable<ProductVM> AvailableProducts
               => ModelRoot.Instance.Products.Select(product => GetOrCreateByModel<ProductVM, Product>(product));

            public bool MeasuresAsServingCount
               => Product?.MeasuresAsServingCount ?? false;

            public string CookedMassOrServingCountDispayUnit
               => MeasuresAsServingCount ? Product.ServingName : "г";

            /// <summary>
            /// Готовая масса либо количество порций (В зависимости от того, измеряется ли продукт в массе или в порциях)
            /// </summary>
            public float CookedMassOrServingCount {
               get => MeasuresAsServingCount ? Model.CookedMass / Product.CookedMassPerServing :  Model.CookedMass;
               set { Model.CookedMass = MeasuresAsServingCount ? value * Product.CookedMassPerServing : value; this.RaisePropertyChanged(); }
            }

            private IDisposable _updateServingsSubscriber;

            private ModelHolderImplementation<ProductInFoodIntakeVM, ProductInFoodIntake> _holderImpl;
         }
      }
   }
}

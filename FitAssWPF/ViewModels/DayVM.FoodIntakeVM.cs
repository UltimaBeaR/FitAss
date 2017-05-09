using System;
using System.Collections.Generic;
using System.Linq;
using ReactiveUI;

namespace FitAssWPF
{
   public partial class DayVM
   {
      public partial class FoodIntakeVM : ViewModel, IModelHolder<FoodIntake>
      {
         private FoodIntakeVM(FoodIntake model)
         {
            Model = model;

            _holderImpl = new ModelHolderImplementation<FoodIntakeVM, FoodIntake>(this);
         }

         static FoodIntakeVM()
         {
            ModelHolderImplementation<FoodIntakeVM, FoodIntake>.DefaultCreateNewModelHolderDelegate =
               model => new FoodIntakeVM(model);
         }

         public FoodIntake Model { get; }

         public string Name {
            get => Model.Name;
            set { Model.Name = value; this.RaisePropertyChanged(); }
         }

         public TimeSpan TimeOfDay {
            get => Model.TimeOfDay;
            set { Model.TimeOfDay = value; this.RaisePropertyChanged(); }
         }

         public IReadOnlyList<ProductInFoodIntakeVM> Products
            => Model.Products.Select(product => GetOrCreateByModel<ProductInFoodIntakeVM, ProductInFoodIntake>(product)).ToArray();

         public ProductInFoodIntakeVM AddNewProduct()
         {
            var product = new ProductInFoodIntake();

            Model.Products.Add(product);
            this.RaisePropertyChanged(nameof(Products));

            return GetOrCreateByModel<ProductInFoodIntakeVM, ProductInFoodIntake>(product);
         }

         public void RemoveProduct(ProductInFoodIntakeVM productVM)
         {
            Model.Products.Remove(productVM.Model);
            this.RaisePropertyChanged(nameof(Products));
         }

         private ModelHolderImplementation<FoodIntakeVM, FoodIntake> _holderImpl;
      }
   }
}

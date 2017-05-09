using System.Collections.Generic;
using System.Linq;
using ReactiveUI;

namespace FitAssWPF
{
   public class ProductsVM : ViewModel
   {
      public ProductsVM()
      {
         _model = ModelRoot.Instance;
      }

      public ProductVM SelectedProduct {
         get => _selectedProduct;
         set { _selectedProduct = value; this.RaisePropertyChanged(); }
      }

      public IReadOnlyList<ProductVM> Products
         => _model.Products.Select(product => GetOrCreateByModel<ProductVM, Product>(product)).ToArray();

      public ProductVM AddNewProduct()
      {
         var product = new Product();

         _model.Products.Add(product);
         this.RaisePropertyChanged(nameof(Products));

         return GetOrCreateByModel<ProductVM, Product>(product);
      }

      public void RemoveProduct(ProductVM productVM)
      {
         _model.Products.Remove(productVM.Model);
         this.RaisePropertyChanged(nameof(Products));

         var productsToClear = _model.Days.SelectMany(day => day.FoodIntakes).SelectMany(foodIntake => foodIntake.Products).Where(product => product.Product == productVM.Model);

         foreach (var productToClear in productsToClear)
         {
            GetOrCreateByModel<DayVM.FoodIntakeVM.ProductInFoodIntakeVM, ProductInFoodIntake>(productToClear).Product = null;
         }
      }

      private ProductVM _selectedProduct;

      private ModelRoot _model;
   }
}

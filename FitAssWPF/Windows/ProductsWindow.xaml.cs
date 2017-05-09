using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace FitAssWPF
{
   public partial class ProductsWindow : Window
   {
      public ProductsWindow()
      {
         InitializeComponent();

         VM = new ProductsVM();
      }

      private void OnProductInListMouseDown(object sender, MouseButtonEventArgs e)
      {
         if (Keyboard.FocusedElement is TextBox elem)
         {
            var binding = BindingOperations.GetBindingExpression(elem, TextBox.TextProperty);
            binding.UpdateSource();
         }

         var productVM = (e.OriginalSource as FrameworkElement).DataContext as ProductVM;

         VM.SelectedProduct = productVM;
      }

      private void OnNewProductClicked(object sender, RoutedEventArgs e)
      {
         VM.SelectedProduct = VM.AddNewProduct();
      }

      private void OnRemoveProductClicked(object sender, RoutedEventArgs e)
      {
         var productVM = (e.OriginalSource as FrameworkElement).DataContext as ProductVM;

         VM.RemoveProduct(productVM);

         if (VM.SelectedProduct?.Model == productVM.Model)
            VM.SelectedProduct = null;
      }

      private object IsProductSelectedToBrush(ProductVM product, ProductVM selectedProduct)
      {
         if (product == null || selectedProduct == null)
            return DependencyProperty.UnsetValue;

         return product.Model == VM.SelectedProduct.Model ? Brushes.LightGreen : DependencyProperty.UnsetValue;
      }

      private Visibility CollapseNullObject(object obj)
         => obj == null ? Visibility.Collapsed : Visibility.Visible;

      private ProductsVM VM {
         get => DataContext as ProductsVM;
         set { DataContext = value; }
      }

      private void CalculateEnergyBasedOnPFH(object sender, RoutedEventArgs e)
      {
         var productVM = e.GetDataContext<ProductVM>();
         productVM.Energy = productVM.GetEnergyCalculatedFromPFH();
      }
   }
}

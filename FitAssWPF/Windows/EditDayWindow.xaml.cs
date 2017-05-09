using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FitAssWPF
{
   public partial class EditDayWindow : Window
   {
      public EditDayWindow(DayVM vm)
      {
         InitializeComponent();

         VM = vm;
      }

      private void OnNewFoodIntakeClicked(object sender, RoutedEventArgs e)
      {
         VM.AddNewFoodIntake();
      }

      private void OnRemoveFoodIntakeClicked(object sender, RoutedEventArgs e)
      {
         VM.RemoveFoodIntake(e.GetDataContext<DayVM.FoodIntakeVM>());
      }

      private void OnNewProductClicked(object sender, RoutedEventArgs e)
      {
         e.GetDataContext<DayVM.FoodIntakeVM>().AddNewProduct();
      }

      private void OnRemoveProductClicked(object sender, RoutedEventArgs e)
      {
         var foodIntakeVM = e.GetParentItemsControlDataContext<DayVM.FoodIntakeVM>();
         var productVM = e.GetDataContext<DayVM.FoodIntakeVM.ProductInFoodIntakeVM>();

         foodIntakeVM.RemoveProduct(productVM);
      }

      private DayVM VM {
         get => DataContext as DayVM;
         set { DataContext = value; }
      }
   }
}

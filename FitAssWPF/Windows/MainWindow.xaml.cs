using System.Diagnostics;
using System.Windows;

namespace FitAssWPF
{
   public partial class MainWindow : Window
   {
      public MainWindow()
      {
         InitializeComponent();

         VM = new MainVM();
      }

      private void OnSaveAllClicked(object sender, RoutedEventArgs e)
      {
         ModelRoot.Instance.Save();
      }

      private void OnOpenSaveFolderClicked(object sender, RoutedEventArgs e)
      {
         Process.Start(ModelRoot.SaveFolderPath);
      }

      private void OnOpenProductsClicked(object sender, RoutedEventArgs e)
      {
         var productsWindow = new ProductsWindow();
         productsWindow.ShowDialog();
      }

      private void OnOpenDayClicked(object sender, RoutedEventArgs e)
      {
         var dayVM = (e.OriginalSource as FrameworkElement).DataContext as DayVM;

         var editDayWindow = new EditDayWindow(dayVM);
         editDayWindow.ShowDialog();
      }

      private void OnNewDayClicked(object sender, RoutedEventArgs e)
      {
         VM.AddNewDay();
      }

      private void OnRemoveDayClicked(object sender, RoutedEventArgs e)
      {
         var dayVM = (e.OriginalSource as FrameworkElement).DataContext as DayVM;

         VM.RemoveDay(dayVM);
      }

      private MainVM VM {
         get => DataContext as MainVM;
         set { DataContext = value; }
      }
   }
}

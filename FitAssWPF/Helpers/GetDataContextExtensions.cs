using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FitAssWPF
{
   public static class GetDataContextExtensions
   {
      public static TDataContext GetDataContext<TDataContext>(this RoutedEventArgs args)
         => args != null ? (args.OriginalSource as FrameworkElement).GetDataContext<TDataContext>() : default(TDataContext);

      public static TDataContext GetParentItemsControlDataContext<TDataContext>(this RoutedEventArgs args)
         => args != null ? (args.OriginalSource as DependencyObject).GetParentItemsControlDataContext<TDataContext>() : default(TDataContext);

      public static TDataContext GetParentItemsControlDataContext<TDataContext>(this DependencyObject dependencyObject)
      {
         if (dependencyObject == null)
            return default(TDataContext);

         var parentDependencyObject = VisualTreeHelper.GetParent(dependencyObject);

         if (parentDependencyObject == null)
            return default(TDataContext);

         if (parentDependencyObject is ItemsControl itemsControl)
            return itemsControl.GetDataContext<TDataContext>();

         return parentDependencyObject.GetParentItemsControlDataContext<TDataContext>();
      }

      public static TDataContext GetDataContext<TDataContext>(this FrameworkElement frameworkElement)
         => frameworkElement != null && frameworkElement.DataContext is TDataContext ? (TDataContext)frameworkElement.DataContext : default(TDataContext);
   }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace FitAssWPF
{
   public class ContentDimensionsBehaviour : Behavior<FrameworkElement>
   {
      #region ContentActualWidth associated object attached property

      public double ContentActualWidth {
         get { return GetContentActualWidth(AssociatedObject); }
         private set { AssociatedObject.SetValue(ContentActualWidthPropertyKey, value); }
      }

      public static double GetContentActualWidth(FrameworkElement element)
         => (double)element.GetValue(ContentActualWidthProperty);

      public static readonly DependencyProperty ContentActualWidthProperty = null; 
      static readonly DependencyPropertyKey ContentActualWidthPropertyKey = null;

      #endregion

      static ContentDimensionsBehaviour() {
         ContentActualWidthPropertyKey = DependencyProperty.RegisterAttachedReadOnly(nameof(ContentActualWidth), typeof(double), typeof(ContentDimensionsBehaviour), new FrameworkPropertyMetadata(0.0));
         ContentActualWidthProperty = ContentActualWidthPropertyKey.DependencyProperty;
      }

      protected override void OnAttached()
      {
         _frameworkElementActualWidthDescriptor.AddValueChanged(AssociatedObject, OnAssociatedObjectActualWidthChanged);
      }

      protected override void OnDetaching()
      {
         _frameworkElementActualWidthDescriptor.RemoveValueChanged(AssociatedObject, OnAssociatedObjectActualWidthChanged);
      }

      private void OnAssociatedObjectActualWidthChanged(object sender, EventArgs args)
      {
         double actualWidth = AssociatedObject.ActualWidth;

         switch (AssociatedObject)
         {
            case GroupBox groupBox: actualWidth -= 22; break;
         }

         ContentActualWidth = actualWidth;
      }

      private static DependencyPropertyDescriptor _frameworkElementActualWidthDescriptor = DependencyPropertyDescriptor.FromProperty(FrameworkElement.ActualWidthProperty, typeof(FrameworkElement));
   }
}

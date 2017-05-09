using System.Windows;
using System.Windows.Controls;

namespace FitAssWPF.Controls
{
   public class WatermarkTextBox : TextBox
   {
      public WatermarkTextBox()
      {
         DefaultStyleKey = typeof(WatermarkTextBox);
      }

      public string Watermark {
         get => (string)GetValue(WatermarkProperty);
         set => SetValue(WatermarkProperty, value);
      }

      public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register(
         nameof(Watermark), typeof(string), typeof(WatermarkTextBox), new PropertyMetadata());
   }
}

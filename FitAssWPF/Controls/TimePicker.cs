using System;
using System.Windows;
using System.Windows.Controls;

namespace FitAssWPF.Controls
{
   [TemplatePart(Name = partHours, Type = typeof(TextBox))]
   [TemplatePart(Name = partMinutes, Type = typeof(TextBox))]
   public class TimePicker : Control
   {
      public TimePicker()
      {
         DefaultStyleKey = typeof(TimePicker);
      }

      public TimeSpan Value {
         get => (TimeSpan)GetValue(ValueProperty);
         set => SetValue(ValueProperty, value);
      }

      public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
         nameof(Value), typeof(TimeSpan), typeof(TimePicker), new PropertyMetadata(new TimeSpan(0), (d, e) => (d as TimePicker).OnValueChanged(e)));

      public override void OnApplyTemplate()
      {
         if (_hours != null)
         {
            _hours.LostKeyboardFocus -= OnHoursLostKeyboardFocus;
            _hours.KeyUp -= OnHoursKeyUp;
         }

         if (_minutes != null)
         {
            _minutes.LostKeyboardFocus -= OnMinutesLostKeyboardFocus;
            _minutes.KeyUp -= OnMinutesKeyUp;
         }

         _hours = GetTemplateChild(partHours) as TextBox;
         _minutes = GetTemplateChild(partMinutes) as TextBox;

         if (_hours != null)
         {
            _hours.LostKeyboardFocus += OnHoursLostKeyboardFocus;
            _hours.KeyUp += OnHoursKeyUp;
         }

         if (_minutes != null)
         {
            _minutes.LostKeyboardFocus += OnMinutesLostKeyboardFocus;
            _minutes.KeyUp += OnMinutesKeyUp;
         }

         SetHoursVisual();
         SetMinutesVisual();

         _appliedtemplate = Template;
      }

      private void OnHoursKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
      {
         if (e.Key == System.Windows.Input.Key.Enter)
            ApplyHours();
      }

      private void OnHoursLostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
      {
         ApplyHours();
      }

      private void OnMinutesKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
      {
         if (e.Key == System.Windows.Input.Key.Enter)
            ApplyMinutes();
      }

      private void OnMinutesLostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
      {
         ApplyMinutes();
      }

      private void ApplyHours()
      {
         var isParsed = int.TryParse(_hours.Text, out int parsedHours);

         if (isParsed && parsedHours >= 0 && parsedHours <= 23)
            Value = new TimeSpan(parsedHours, Value.Minutes, Value.Seconds);

         SetHoursVisual();
      }

      private void ApplyMinutes()
      {
         var isParsed = int.TryParse(_minutes.Text, out int parsedMinutes);

         if (isParsed && parsedMinutes >= 0 && parsedMinutes <= 59)
            Value = new TimeSpan(Value.Hours, parsedMinutes, Value.Seconds);

         SetMinutesVisual();
      }

      private void SetHoursVisual()
      {
         _hours.Text = $"{Value.Hours:00}";
      }

      private void SetMinutesVisual()
      {
         _minutes.Text = $"{Value.Minutes:00}";
      }

      private void OnValueChanged(DependencyPropertyChangedEventArgs e)
      {
         if (_appliedtemplate == null)
            return;

         _hours.Text = Value.Hours.ToString();
         _minutes.Text = Value.Minutes.ToString();
      }

      private ControlTemplate _appliedtemplate;

      private TextBox _hours;
      private TextBox _minutes;

      private const string partHours = "PART_Hours";
      private const string partMinutes = "PART_Minutes";
   }
}

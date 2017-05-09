using System.Collections.Generic;
using System.Linq;
using ReactiveUI;

namespace FitAssWPF
{
   public class MainVM : ViewModel
   {
      public MainVM()
      {
         _model = ModelRoot.Instance;
      }

      public IReadOnlyList<DayVM> Days
         => _model.Days.Select(day => GetOrCreateByModel<DayVM, Day>(day)).ToArray();

      public DayVM AddNewDay()
      {
         var day = new Day();

         _model.Days.Add(day);
         this.RaisePropertyChanged(nameof(Days));

         return GetOrCreateByModel<DayVM, Day>(day);
      }

      public void RemoveDay(DayVM dayVM)
      {
         _model.Days.Remove(dayVM.Model);
         this.RaisePropertyChanged(nameof(Days));
      }

      private ModelRoot _model;
   }
}

using System.Collections.Generic;
using System.Linq;
using ReactiveUI;
using System;

namespace FitAssWPF
{
   public partial class DayVM : ViewModel, IModelHolder<Day>
   {
      private DayVM(Day model)
      {
         Model = model;

         _holderImpl = new ModelHolderImplementation<DayVM, Day>(this);

         _onTotalsChangedSubscribers = new List<IDisposable>();

         OnTotalsChanged();
      }

      static DayVM()
      {
         ModelHolderImplementation<DayVM, Day>.DefaultCreateNewModelHolderDelegate =
            model => new DayVM(model);
      }

      public Day Model { get; }

      public string Name {
         get => Model.Name;
         set {  Model.Name = value; this.RaisePropertyChanged(); }
      }

      #region FoodIntakes

      public IReadOnlyList<FoodIntakeVM> FoodIntakes
         => Model.FoodIntakes.Select(foodIntake => GetOrCreateByModel<FoodIntakeVM, FoodIntake>(foodIntake)).ToArray();

      public FoodIntakeVM AddNewFoodIntake()
      {
         var foodIntake = new FoodIntake();

         Model.FoodIntakes.Add(foodIntake);
         this.RaisePropertyChanged(nameof(FoodIntakes));

         return GetOrCreateByModel<FoodIntakeVM, FoodIntake>(foodIntake);
      }

      public void RemoveFoodIntake(FoodIntakeVM foodIntakeVM)
      {
         Model.FoodIntakes.Remove(foodIntakeVM.Model);
         this.RaisePropertyChanged(nameof(FoodIntakes));
      }

      #endregion

      #region Total part

      public float EnergyTotal
         => _energyTotal;

      public float ProteinTotal
         => _proteinTotal;

      public float FatTotal
         => _fatTotal;

      public float HydrocarbonateTotal
         => _hydrocarbonateTotal;

      public bool TotalsCalculatedCompletely
         => _totalsCalculatedCompletely;

      #endregion

      private void OnTotalsChanged()
      {
         if (_onTotalChangedInProgress)
            return;

         _onTotalChangedInProgress = true;

         // Освобождаем старых подписчиков

         foreach (var subscriber in _onTotalsChangedSubscribers)
            subscriber.Dispose();
         _onTotalsChangedSubscribers.Clear();

         // Добавляем новых подписчиков

         _onTotalsChangedSubscribers.Add(
            this.WhenAnyValue(vm => vm.FoodIntakes).
            Subscribe(vm => OnTotalsChanged()));

         foreach (var foodIntake in FoodIntakes)
         {
            _onTotalsChangedSubscribers.Add(
               foodIntake.WhenAnyValue(vm => vm.Products)
               .Subscribe(vm => OnTotalsChanged()));

            foreach (var product in foodIntake.Products)
            {
               _onTotalsChangedSubscribers.Add(
                  product.WhenAnyValue(vm => vm.Product, vm => vm.CookedMassOrServingCount)
                  .Subscribe(vm => OnTotalsChanged()));

               if (product.Product != null)
                  _onTotalsChangedSubscribers.Add(product.Product.Changed.Subscribe(vm => OnTotalsChanged()));
            }
         }

         // Пересчитываем totals

         RecalculateTotals();

         _onTotalChangedInProgress = false;
      }

      private void RecalculateTotals()
      {
         _energyTotal = 0;
         _proteinTotal = 0;
         _fatTotal = 0;
         _hydrocarbonateTotal = 0;
         _totalsCalculatedCompletely = true;

         foreach (var foodIntake in FoodIntakes)
         {
            foreach (var product in foodIntake.Products)
            {
               if (product.Product != null && product.CookedMassOrServingCount >= 0)
               {
                  // Получаем коэффициент для преобразования из готовой массы продукта в сухую
                  var cookedToRawMassCoeff = product.Product.MassBeforeCooking == product.Product.MassAfterCooking ?
                     1f :
                     product.Product.MassBeforeCooking / product.Product.MassAfterCooking;

                  // Получаем сухую массу продукта

                  var rawMass = product.Model.CookedMass * cookedToRawMassCoeff;

                  // Получаем пищевую ценность (каждый ее компонент) в "процентах" (Диапазон 0-1) и умножаем на сухую массу,
                  // получая содержание комопнентов пищевой ценности в этой сухой массе

                  var energy = (product.Product.Energy / product.Product.MeasuringMass) * rawMass;
                  var protein = (product.Product.Protein / product.Product.MeasuringMass) * rawMass;
                  var fat = (product.Product.Fat / product.Product.MeasuringMass) * rawMass;
                  var hydrocarbonate = (product.Product.Hydrocarbonate / product.Product.MeasuringMass) * rawMass;

                  // Суммируем с total-ом

                  _energyTotal += energy;
                  _proteinTotal += protein;
                  _fatTotal += fat;
                  _hydrocarbonateTotal += hydrocarbonate;
               }
               else
                  _totalsCalculatedCompletely = false;
            }
         }

         this.RaisePropertyChanged(nameof(EnergyTotal));
         this.RaisePropertyChanged(nameof(ProteinTotal));
         this.RaisePropertyChanged(nameof(FatTotal));
         this.RaisePropertyChanged(nameof(HydrocarbonateTotal));
         this.RaisePropertyChanged(nameof(TotalsCalculatedCompletely));
      }

      /// <summary>
      /// Подписчики на изменения, которые влияют на пересчет Totals
      /// </summary>
      private List<IDisposable> _onTotalsChangedSubscribers;
      private bool _onTotalChangedInProgress;

      private float _energyTotal;
      private float _proteinTotal;
      private float _fatTotal;
      private float _hydrocarbonateTotal;
      private bool _totalsCalculatedCompletely;

      private ModelHolderImplementation<DayVM, Day> _holderImpl;
   }
}

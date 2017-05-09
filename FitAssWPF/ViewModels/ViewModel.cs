using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitAssWPF
{
   /// <summary>
   /// Любая view model должна наследоваться от этого класса
   /// </summary>
   public abstract class ViewModel : ReactiveObject
   {
      /// <summary>
      /// Получить VM нужного типа на основании модели. Если такой модели еще нет, она будет создана автоматически
      /// (либо через дефолтный делегат создания, если он задан для ModelHolderImplementation данного холдера, либо через переданный делегат, если он не null)
      /// </summary>
      public static TViewModel GetOrCreateByModel<TViewModel, TModel>(TModel model, Func<TModel, TViewModel> createNewModelHolderDelegate = null)
         where TViewModel : ViewModel, IModelHolder<TModel>
         => ModelHolderImplementation<TViewModel, TModel>.GetOrCreate(model, createNewModelHolderDelegate);

      /// <summary>
      /// Получить коллекцию VM на основании модели. Результирующая колллекция может быть пустой, если нет ни одной VM с такой моделью
      /// </summary>
      public static IEnumerable<TViewModel> GetAllByModel<TViewModel, TModel>(TModel model)
         where TViewModel : ViewModel, IModelHolder<TModel>
         => ModelHolderImplementation<TViewModel, TModel>.GetAll(model);
   }

   /// <summary>
   /// ViewModel (Либо другой класс), содержащая модель определенного типа должна (желательно) реализовать этот интерфейс, плюс внутренне должен быть создан
   /// экземпляр ModelHolderImplementation через агрегацию
   /// </summary>
   public interface IModelHolder<TModel>
   {
      TModel Model { get; }
   }

   /// <summary>
   /// Вспомогательный класс, позволяющий хранить связи между ModelHolder-ами и их моделями и делать поиск по модели.
   /// В классе, реализующем IModelHolder, нужно через агрегацию (Если идет цепочка наследований холдеров, то такой объект должен создаваться на каждом уровне наследования, где модель становится более конкретным классом)
   /// Например ItemB_VM : ItemA_VM, в этом случае этот объект должен быть создан в ItemA для модели A, и в ItemB для модели B, хотя модель B и наследуется от модели A
   /// Также, в каждом холдере нужно в статическом конструкторе в DefaultCreateNewModelHolderDelegate присваивать дефолтный конструктор для создания этого холдера, в случае если его можно создать без каких-либо дополнительных параметров, кроме модели
   /// (Иначе, если он не задан, то при получении холдера по модели придется его каждый раз передавть явно (но зато там можно учитывать дополнительные параметры, кроме модели)
   /// </summary>
   public class ModelHolderImplementation<TModelHolder, TModel> : IDisposable
      where TModelHolder : class, IModelHolder<TModel>
   {
      public ModelHolderImplementation(TModelHolder modelHolder)
      {
         _modelHolder = modelHolder;

         // Добавляем текущий экземпляр в список живых
         _aliveModelHolders.Add(_modelHolder);
      }

      ~ModelHolderImplementation()
      {
         Dispose();
      }

      public static Func<TModel, TModelHolder> DefaultCreateNewModelHolderDelegate { get; set; }

      public void Dispose()
      {
         if (!_isDisposed)
         {
            _isDisposed = true;

            // Удаляем текущий экземпляр из списка живых

            if (object.ReferenceEquals(_lastRequestedModelHolder, _modelHolder))
               _lastRequestedModelHolder = null;

            _aliveModelHolders.Remove(_modelHolder);
         }
      }

      /// <summary>
      /// Получение первой попавшейся ViewModel по заданной модели
      /// Опционально можно задать делегат создания новой ViewModel, который будет использован в случае, если ничего не будет найдено
      /// (Таким образом реализуется поведение "взять текущую или создать новую")
      /// </summary>
      public static TModelHolder GetOrCreate(TModel model, Func<TModel, TModelHolder> createNewModelHolderDelegate = null)
      {
         if (_lastRequestedModelHolder != null && object.ReferenceEquals(_lastRequestedModelHolder.Model, model))
            return _lastRequestedModelHolder;

         foreach (var modelHolder in _aliveModelHolders)
         {
            if (object.ReferenceEquals(modelHolder.Model, model))
            {
               _lastRequestedModelHolder = modelHolder;
               return modelHolder;
            }
         }

         {
            if (createNewModelHolderDelegate == null)
            {
               // Если делегат создания холдера не задан явно - берем его из статических данных, предварительно вызвав статический конструктор холдера, так как
               // этот статический делегат в основном задается от туда

               InitModelHolderStatic();
               createNewModelHolderDelegate = DefaultCreateNewModelHolderDelegate;
            }

            TModelHolder modelHolder = createNewModelHolderDelegate?.Invoke(model);
            if (modelHolder != null)
               _lastRequestedModelHolder = modelHolder;

            return modelHolder;
         }
      }

      /// <summary>
      /// Получение всех ViewModel по заданной модели
      /// </summary>
      public static IEnumerable<TModelHolder> GetAll(TModel model)
      {
         foreach (var modelHolder in _aliveModelHolders)
         {
            if (object.ReferenceEquals(modelHolder.Model, model))
               yield return modelHolder;
         }
      }

      /// <summary>
      /// Вызов статического конструктора для типа ModelHolder-а. Если этого не сделать, может быть ситуация что при поиске ModelHolder-а
      /// не сможет создаться ModelHolder через DefaultCreateNewModelHolderDelegate, так как он задается (обычно) в статическом конструкторе холдера, а этот конструктор может быть не вызван на момент поиска
      /// </summary>
      private static void InitModelHolderStatic()
      {
         if (_modelHolderStaticConstructorInvoked)
            return;

         System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(TModelHolder).TypeHandle);
         _modelHolderStaticConstructorInvoked = true;
      }

      private bool _isDisposed;

      private TModelHolder _modelHolder;

      private static bool _modelHolderStaticConstructorInvoked;

      /// <summary>
      /// Небольшая оптимизация (кэш), чтобы GetByModel() на одну и ту же модель подряд не искал ее каждый раз в списке
      /// </summary>
      private static TModelHolder _lastRequestedModelHolder;

      /// <summary>
      /// Все живые на данный момент держатели этого типа модели
      /// </summary>
      private static List<TModelHolder> _aliveModelHolders
         = new List<TModelHolder>();
   }
}

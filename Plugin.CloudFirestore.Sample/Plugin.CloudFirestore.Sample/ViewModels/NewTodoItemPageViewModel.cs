using System;
using Reactive.Bindings;
using System.Reactive.Linq;
using Reactive.Bindings.Extensions;
using Plugin.CloudFirestore.Sample.Models;
using Prism.Services;
using Prism.Navigation;
using System.Threading;
using System.Threading.Tasks;

namespace Plugin.CloudFirestore.Sample.ViewModels
{
    public class NewTodoItemPageViewModel : ViewModelBase
    {
        public ReactivePropertySlim<string?> Name { get; set; } = new ReactivePropertySlim<string?>();
        public ReactivePropertySlim<string?> Notes { get; set; } = new ReactivePropertySlim<string?>();

        public AsyncReactiveCommand CreateCommand { get; }
        public ReactiveCommand CancelCommand { get; }

        private readonly IPageDialogService _pageDialogService;

        public NewTodoItemPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService)
        {
            _pageDialogService = pageDialogService;

            Title = "New Todo Item";

            CreateCommand = new[] {
                Name.Select(x => string.IsNullOrEmpty(x))
            }.CombineLatestValuesAreAllFalse()
             .ObserveOn(SynchronizationContext.Current)
             .ToAsyncReactiveCommand();

            _ = CreateCommand.Subscribe(async () =>
              {
                  var item = new TodoItem
                  {
                      Name = Name.Value,
                      Notes = Notes.Value,
                  };

                  _ = CrossCloudFirestore.Current
                                     .Instance
                                     .Collection(TodoItem.CollectionPath)
                                     .AddAsync(item)
                                     .ContinueWith(t =>
                                     {
                                         System.Diagnostics.Debug.WriteLine(t.Exception);
                                     }, TaskContinuationOptions.OnlyOnFaulted);

                  await navigationService.GoBackAsync();
              });

            CancelCommand = new ReactiveCommand();
            CancelCommand.Subscribe(() => NavigationService.GoBackAsync());
        }
    }
}

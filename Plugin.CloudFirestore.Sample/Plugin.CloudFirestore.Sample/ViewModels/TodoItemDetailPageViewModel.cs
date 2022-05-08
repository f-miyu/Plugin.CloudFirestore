using System;
using System.Reactive.Linq;
using Plugin.CloudFirestore.Sample.Models;
using Prism.Navigation;
using Prism.Services;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Plugin.CloudFirestore.Sample.ViewModels
{
    public class TodoItemDetailPageViewModel : ViewModelBase<string?>
    {
        private ReactivePropertySlim<string?> _id = new ReactivePropertySlim<string?>(mode: ReactivePropertyMode.DistinctUntilChanged);
        private ReactivePropertySlim<TodoItem> _todoItem = new ReactivePropertySlim<TodoItem>(mode: ReactivePropertyMode.DistinctUntilChanged);

        public ReactivePropertySlim<string?> Name { get; set; } = new ReactivePropertySlim<string?>();
        public ReactivePropertySlim<string?> Notes { get; set; } = new ReactivePropertySlim<string?>();

        public AsyncReactiveCommand UpdateCommand { get; }
        public AsyncReactiveCommand DeleteCommand { get; }

        private readonly IPageDialogService _pageDialogService;

        public TodoItemDetailPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService)
        {
            _pageDialogService = pageDialogService;

            Title = "Todo Item";

            _id.Where(id => id != null)
               .SelectMany(id => CrossCloudFirestore.Current.Instance.Document($"{TodoItem.CollectionPath}/{id}").GetAsync())
               .Where(document => document != null)
               .Select(document => document.ToObject<TodoItem>())
               .Subscribe(todoItem =>
               {
                   if (_todoItem != null)
                   {
                       _todoItem.Value = todoItem!;
                       Name.Value = todoItem!.Name;
                       Notes.Value = todoItem!.Notes;
                   }
               }, ex => System.Diagnostics.Debug.WriteLine(ex));

            UpdateCommand = new[] {
                _todoItem.Select(x => x == null),
                Name.Select(x => string.IsNullOrEmpty(x))
            }.CombineLatestValuesAreAllFalse()
             .ObserveOn(SynchronizationContext.Current)
             .ToAsyncReactiveCommand();

            UpdateCommand.Subscribe(async () =>
            {
                var todoItem = _todoItem.Value;
                todoItem.Name = Name.Value;
                todoItem.Notes = Notes.Value;

                _ = CrossCloudFirestore.Current.Instance.Document($"{TodoItem.CollectionPath}/{todoItem.Id}")
                    .UpdateAsync(todoItem)
                    .ContinueWith(t =>
                    {
                        System.Diagnostics.Debug.WriteLine(t.Exception);
                    }, TaskContinuationOptions.OnlyOnFaulted);

                await navigationService.GoBackAsync();
            });

            DeleteCommand = new[] {
                _todoItem.Select(x => x == null)
            }.CombineLatestValuesAreAllFalse()
             .ObserveOn(SynchronizationContext.Current)
             .ToAsyncReactiveCommand();

            DeleteCommand.Subscribe(async () =>
            {
                var ok = await _pageDialogService.DisplayAlertAsync("Are you sure you want to delete this?", _todoItem.Value.Name, "Ok", "Cancel");

                if (ok)
                {
                    _ = CrossCloudFirestore.Current
                        .Instance
                        .Document($"{TodoItem.CollectionPath}/{_todoItem.Value.Id}")
                        .DeleteAsync()
                        .ContinueWith(t =>
                        {
                            System.Diagnostics.Debug.WriteLine(t.Exception);
                        }, TaskContinuationOptions.OnlyOnFaulted);

                    await NavigationService.GoBackAsync();
                }
            });
        }

        public override void Prepare(string? parameer)
        {
            _id.Value = parameer;
        }
    }
}
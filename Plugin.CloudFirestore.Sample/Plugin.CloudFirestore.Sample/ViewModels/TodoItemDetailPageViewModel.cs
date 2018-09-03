using System;
using System.Reactive.Linq;
using Plugin.CloudFirestore.Sample.Models;
using Prism.Navigation;
using Prism.Services;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System.Net.Http.Headers;
using System.Threading;

namespace Plugin.CloudFirestore.Sample.ViewModels
{
    public class TodoItemDetailPageViewModel : ViewModelBase<string>
    {
        private ReactivePropertySlim<string> _id = new ReactivePropertySlim<string>(mode: ReactivePropertyMode.DistinctUntilChanged);
        private ReactivePropertySlim<TodoItem> _todoItem = new ReactivePropertySlim<TodoItem>(mode: ReactivePropertyMode.DistinctUntilChanged);

        public ReactivePropertySlim<string> Name { get; set; } = new ReactivePropertySlim<string>();
        public ReactivePropertySlim<string> Notes { get; set; } = new ReactivePropertySlim<string>();

        public AsyncReactiveCommand UpdateCommand { get; }
        public AsyncReactiveCommand DeleteCommand { get; }

        private readonly IPageDialogService _pageDialogService;

        public TodoItemDetailPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService)
        {
            _pageDialogService = pageDialogService;

            Title = "Todo Item";

            _id.Where(id => id != null)
               .SelectMany(id => CrossCloudFirestore.Current.GetDocument($"{TodoItem.CollectionPath}/{id}").GetDocumentAsync())
               .Where(document => document != null)
               .Select(document => document.ToObject<TodoItem>())
               .Where(todoItem => _todoItem != null)
               .Subscribe(todoItem =>
               {
                   _todoItem.Value = todoItem;
                   Name.Value = todoItem.Name;
                   Notes.Value = todoItem.Notes;
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

                CrossCloudFirestore.Current.GetDocument($"{TodoItem.CollectionPath}/{todoItem.Id}")
                                   .UpdateData(todoItem, (error) =>
                                   {
                                       if (error != null)
                                       {
                                           System.Diagnostics.Debug.WriteLine(error);
                                       }
                                   });

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
                    CrossCloudFirestore.Current
                                       .GetDocument($"{TodoItem.CollectionPath}/{_todoItem.Value.Id}")
                                       .DeleteDocument((error) =>
                                       {
                                           if (error != null)
                                           {
                                               System.Diagnostics.Debug.WriteLine(error);
                                           }
                                       });

                    await NavigationService.GoBackAsync();
                }
            });
        }

        public override void Prepare(string parameer)
        {
            _id.Value = parameer;
        }
    }
}
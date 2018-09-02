using System;
using System.Reactive.Linq;
using Plugin.CloudFirestore.Sample.Models;
using Prism.Navigation;
using Prism.Services;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System.Net.Http.Headers;

namespace Plugin.CloudFirestore.Sample.ViewModels
{
    public class TodoItemDetailPageViewModel : ViewModelBase
    {
        public static string NavigationParameterId = "id";

        private ReactivePropertySlim<TodoItem> _todoItem = new ReactivePropertySlim<TodoItem>();

        public ReactivePropertySlim<string> Name { get; set; } = new ReactivePropertySlim<string>();
        public ReactivePropertySlim<string> Notes { get; set; } = new ReactivePropertySlim<string>();

        public AsyncReactiveCommand UpdateCommand { get; }
        public AsyncReactiveCommand DeleteCommand { get; }

        private readonly IPageDialogService _pageDialogService;

        public TodoItemDetailPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService)
        {
            _pageDialogService = pageDialogService;

            Title = "Todo Item";

            UpdateCommand = new[] {
                _todoItem.Select(x => x == null),
                Name.Select(x => string.IsNullOrEmpty(x)),
                Notes.Select(x => string.IsNullOrEmpty(x))
            }.CombineLatestValuesAreAllFalse()
             .ToAsyncReactiveCommand();

            UpdateCommand.Subscribe(async () =>
            {
                var todoItem = _todoItem.Value;
                todoItem.Name = Name.Value;
                todoItem.Notes = Notes.Value;

                CrossCloudFirestore.Current.GetDocument($"{TodoItem.CollectionPath}/{todoItem.Id}")
                                   .UpdateData(todoItem, (error) =>
                                   {
                                       System.Diagnostics.Debug.WriteLine(error);
                                   });

                await navigationService.GoBackAsync();
            });

            DeleteCommand = new[] {
                _todoItem.Select(x => x == null)
            }.CombineLatestValuesAreAllFalse()
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
                                           System.Diagnostics.Debug.WriteLine(error);
                                       });

                    await NavigationService.GoBackAsync();
                }
            });
        }

        public async override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.GetNavigationMode() == NavigationMode.New)
            {
                try
                {
                    var id = (string)parameters[NavigationParameterId];

                    var document = await CrossCloudFirestore.Current
                                                            .GetDocument($"{TodoItem.CollectionPath}/{id}")
                                                            .GetDocumentAsync();

                    var todoItem = document.ToObject<TodoItem>();
                    _todoItem.Value = todoItem;
                    Name.Value = todoItem.Name;
                    Notes.Value = todoItem.Notes;
                }
                catch (CloudFirestoreException e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            }
        }
    }
}
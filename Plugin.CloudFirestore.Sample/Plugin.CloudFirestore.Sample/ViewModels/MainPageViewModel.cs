using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reactive.Bindings;
using Plugin.CloudFirestore.Sample.Models;
using Plugin.CloudFirestore.Extensions;
using Reactive.Bindings.Extensions;
using System.Reactive.Disposables;
using Plugin.CloudFirestore.Sample.Extensions;
using System.Reactive.Linq;

namespace Plugin.CloudFirestore.Sample.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private const int Limit = 50;

        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public ReactiveCollection<TodoItemViewModel> TodoItems { get; } = new ReactiveCollection<TodoItemViewModel>();
        public ReactiveCommand AddCommand { get; }
        public ReactiveCommand<TodoItemViewModel> SelectItemCommand { get; }

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Todo List";

            var query = CrossCloudFirestore.Current
                                           .GetCollection(TodoItem.CollectionPath)
                                           .OrderBy(nameof(TodoItem.CreatedAt), false)
                                           .LimitTo(Limit);

            query.ObserveAdded()
                 .Subscribe(document =>
                 {
                     var item = document.ToObject<TodoItem>();
                     TodoItems.InsertOnScheduler(0, new TodoItemViewModel(item));
                 }).AddTo(_disposables);

            query.ObserveModified()
                 .Subscribe(document =>
                 {
                     var item = document.ToObject<TodoItem>();
                     var viewModel = TodoItems.FirstOrDefault(x => x.Id == item.Id);
                     viewModel?.Update(item.Name, item.Notes);
                 }).AddTo(_disposables);

            query.ObserveRemoved()
                 .Subscribe(document =>
                 {
                     var item = document.ToObject<TodoItem>();
                     var viewModel = TodoItems.FirstOrDefault(x => x.Id == item.Id);
                     if (viewModel != null)
                     {
                         TodoItems.RemoveOnScheduler(viewModel);
                     }
                 }).AddTo(_disposables);

            AddCommand = new ReactiveCommand();
            AddCommand.Subscribe(() =>
            {
                navigationService.NavigateAsync<NewTodoItemPageViewModel>(useModalNavigation: true, wrapInNavigationPage: true);
            });

            SelectItemCommand = new ReactiveCommand<TodoItemViewModel>();
            SelectItemCommand.Subscribe(item =>
            {
                var parameters = new NavigationParameters();
                parameters.Add(TodoItemDetailPageViewModel.NavigationParameterId, item.Id);

                navigationService.NavigateAsync<TodoItemDetailPageViewModel>(parameters);
            });
        }

        public override void Destroy()
        {
            base.Destroy();

            _disposables.Dispose();
        }
    }
}

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
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public ReactiveCollection<TodoItemViewModel> TodoItems { get; } = new ReactiveCollection<TodoItemViewModel>();
        public ReactiveCommand AddCommand { get; }
        public ReactiveCommand<TodoItemViewModel> SelectItemCommand { get; }

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Todo List";

            var query = CrossCloudFirestore.Current
                                           .Instance
                                           .GetCollection(TodoItem.CollectionPath)
                                           .OrderBy(nameof(TodoItem.CreatedAt), true);

            query.ObserveAdded()
                 .Select(change => (Object: change.Document.ToObject<TodoItem>(ServerTimestampBehavior.Estimate), Index: change.NewIndex))
                 .Select(t => (ViewModel: new TodoItemViewModel(t.Object), Index: t.Index))
                 .Subscribe(t => TodoItems.InsertOnScheduler(t.Index, t.ViewModel))
                 .AddTo(_disposables);

            query.ObserveModified()
                 .Select(change => change.Document.ToObject<TodoItem>(ServerTimestampBehavior.Estimate))
                 .Select(todoItem => (TodoItem: todoItem, ViewModel: TodoItems.FirstOrDefault(x => x.Id == todoItem.Id)))
                 .Where(t => t.ViewModel != null)
                 .Subscribe(t => t.ViewModel.Update(t.TodoItem.Name, t.TodoItem.Notes))
                 .AddTo(_disposables);

            query.ObserveRemoved()
                 .Select(change => TodoItems.FirstOrDefault(x => x.Id == change.Document.Id))
                 .Where(viewModel => viewModel != null)
                 .Subscribe(viewModel => TodoItems.RemoveOnScheduler(viewModel))
                 .AddTo(_disposables);

            AddCommand = new ReactiveCommand();
            AddCommand.Subscribe(() =>
            {
                navigationService.NavigateAsync<NewTodoItemPageViewModel>(useModalNavigation: true, wrapInNavigationPage: true);
            });

            SelectItemCommand = new ReactiveCommand<TodoItemViewModel>();
            SelectItemCommand.Subscribe(item =>
            {
                navigationService.NavigateAsync<TodoItemDetailPageViewModel, string>(item.Id);
            });
        }

        public override void Destroy()
        {
            base.Destroy();

            _disposables.Dispose();
        }
    }
}

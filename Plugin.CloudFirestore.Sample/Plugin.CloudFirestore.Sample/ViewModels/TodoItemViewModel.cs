using System;
using Prism.Mvvm;
using Reactive.Bindings;
using System.Globalization;
using Plugin.CloudFirestore.Sample.Models;
namespace Plugin.CloudFirestore.Sample.ViewModels
{
    public class TodoItemViewModel : BindableBase
    {
        public string? Id { get; }

        public ReactivePropertySlim<string?> Name { get; set; } = new ReactivePropertySlim<string?>();
        public ReactivePropertySlim<string?> Notes { get; set; } = new ReactivePropertySlim<string?>();

        public TodoItemViewModel(TodoItem item)
        {
            Id = item.Id;
            Name.Value = item.Name;
            Notes.Value = item.Notes;
        }

        public void Update(string? name, string? notes)
        {
            Name.Value = name;
            Notes.Value = notes;
        }
    }
}

using System;
using Plugin.CloudFirestore.Attributes;
namespace Plugin.CloudFirestore.Sample.Models
{
    public class TodoItem
    {
        public static string CollectionPath = "todoItems";

        [Id]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

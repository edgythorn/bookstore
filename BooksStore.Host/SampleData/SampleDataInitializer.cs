using BooksStore.Interfaces;
using BooksStore.Models;
using Newtonsoft.Json;
using System;
using System.IO;

namespace BooksStore.Host.SampleData
{
    public static class SampleDataInitializer
    {
        public static void LoadDataFromJson(IBooksRepository repository)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SampleData", "books.json");
            var json = File.ReadAllText(path);
            var books = JsonConvert.DeserializeObject<Book[]>(json);

            foreach (var item in books)
            {
                repository.CreateBookAsync(item);
            }
        }
    }
}

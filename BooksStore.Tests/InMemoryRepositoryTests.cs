using BooksStore.Interfaces;
using BooksStore.Models;
using BooksStore.Services;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BooksStore.Tests
{
    [TestClass]
    public class InMemoryRepositoryTests
    {
        private static ILoggerFactory _loggerFactory = new LoggerFactory().AddConsole();
        private static IBooksRepository GetRepository() => new InMemoryBooksRepository(_loggerFactory.CreateLogger<InMemoryBooksRepository>());

        [TestMethod]
        public void InMemoryRepository_CreateTest()
        {
            CreateTestAcync().Wait();
        }

        private async Task CreateTestAcync()
        {
            var repo = GetRepository();
            var id = Guid.NewGuid();
            var createdId = await repo.CreateBookAsync(new Book { Id = id });
            Assert.AreEqual(id, createdId);
            var saved = await repo.GetBookAsync(id);
            Assert.AreEqual(id, saved.Id);
        }

        [TestMethod]
        public void InMemoryRepository_UpdateTest()
        {
            UpdateTestAcync().Wait();
        }

        private async Task UpdateTestAcync()
        {
            var repo = GetRepository();
            var id = Guid.NewGuid();
            var createdId = await repo.CreateBookAsync(new Book { Id = id });
            Assert.AreEqual(id, createdId);
            await repo.UpdateBookAsync(new Book { Id = id, Title = id.ToString() });
            var saved = await repo.GetBookAsync(id);
            Assert.AreEqual(id, saved.Id);
            Assert.AreEqual(id.ToString(), saved.Title);
        }

        [TestMethod]
        public void InMemoryRepository_DeleteTest()
        {
            DeleteTestAcync().Wait();
        }

        [ExpectedException(typeof(KeyNotFoundException))]
        private async Task DeleteTestAcync()
        {
            var repo = GetRepository();
            var id = Guid.NewGuid();
            var createdId = await repo.CreateBookAsync(new Book { Id = id });
            Assert.AreEqual(id, createdId);
            var saved = await repo.GetBookAsync(id);
        }

        [TestMethod]
        public void InMemoryRepository_PaginationTest()
        {
            PaginationTestAsync().Wait();
        }

        private async Task PaginationTestAsync()
        {
            var repo = GetRepository();

            int pageSize = 10;
            int pages = 25;
            int additional = 3;
            int total = pageSize * pages + additional;

            for (int i = 0; i < total; i++)
            {
                var id = Guid.NewGuid();
                var createdId = await repo.CreateBookAsync(new Book { Id = id });
                Assert.AreEqual(id, createdId);
            }

            Pagination pagination;
            for (int i = 0; i < pages; i++)
            {
                pagination = new Pagination(i, pageSize);

                var result = await repo.GetBooksAsync(pagination);

                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Books);
                Assert.AreEqual(pageSize, result.Books.Length);
                Assert.AreEqual(total, result.Total);
            }

            pagination = new Pagination(pages, pageSize);

            var lastPage = await repo.GetBooksAsync(pagination);

            Assert.IsNotNull(lastPage);
            Assert.IsNotNull(lastPage.Books);
            Assert.AreEqual(additional, lastPage.Books.Length);
            Assert.AreEqual(total, lastPage.Total);
        }
    }
}

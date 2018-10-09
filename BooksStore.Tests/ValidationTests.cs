using BooksStore.Tests.TestData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BooksStore.Tests
{
    [TestClass]
    public class ValidationTests
    {
        //TODO More tests

        [TestMethod]
        public void ValidModelsShouldBePassed()
        {
            foreach (var model in ValidModels.Get())
            {
                Assert.IsNull(model.Validate());
            }
        }

        [TestMethod]
        public void InvalidModelsShouldBeCatched()
        {
            foreach (var model in InvalidModels.Get())
            {
                var results = model.Validate();
                Assert.IsNotNull(results);
                Assert.AreNotEqual(0, results.Count);
                foreach (var item in results)
                {
                    Assert.IsNotNull(item);
                    Assert.IsNotNull(item.ErrorMessage);
                }
            }
        }
    }
}

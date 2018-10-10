using BooksStore.Tests.TestData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BooksStore.Tests
{
    [TestClass]
    public class ValidationTests
    {
        [TestMethod]
        public void Validation_ValidModelsShouldBePassed()
        {
            var index = 0;
            foreach (var model in ValidModels.Get())
            {
                Assert.IsNull(model.Validate(), $"Model with index {index} ({(model as TestBook)?.Description}) has not been validated");
                index++;
            }
        }

        [TestMethod]
        public void Validation_InvalidModelsShouldBeCatched()
        {
            var index = 0;
            foreach (var model in InvalidModels.Get())
            {
                var results = model.Validate();

                Assert.IsNotNull(results, $"Model with index {index} ({(model as TestBook)?.Description}) has been validated");
                Assert.AreNotEqual(0, results.Count);
                
                foreach (var item in results)
                {
                    Assert.IsNotNull(item);
                    Assert.IsNotNull(item.ErrorMessage);
                }

                index++;
            }
        }
    }
}

using BooksStore.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BooksStore.Tests
{
    [TestClass]
    public class ISBNValidationTests
    {
        private static string[] _valid = new string[]
        {
            "5-93286-045-6",
            "5-93286-043-X",
            "5-93286-043-x",
            "978-5-9908910-4-3",
            "978-5-6040723-1-8",
            "978-5-496-00433-6",
            "978-5-6041394-0-0",
            "978-5-97060-415-1"
        };

        private static string[] _invalid = new string[]
        {
            "5-93286-043-0",
            "X-93286-045-6",
            "978-X-9908910-4-3",
            "979-5-6040723-1-8",
            "97-8-5-496-00433-6",
            "978-5496-00433-6",
            "978-5-6041394-O-0"
        };


        [TestMethod]
        public void ISBN_ValidTest()
        {
            foreach (var item in _valid)
            {
                Assert.IsTrue(ISBNValidator.IsValid(item), $"Value '{item}' has not passed validation");
            }
        }

        [TestMethod]
        public void ISBN_InvalidTest()
        {
            foreach (var item in _invalid)
            {
                Assert.IsFalse(ISBNValidator.IsValid(item), $"Value '{item}' has passed validation");
            }
        }
    }
}

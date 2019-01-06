using System.Collections.Generic;

namespace AutoSFaP.Tests
{
    internal static class TestHelper
    {
        internal static IEnumerable<TestModel> GetTestData()
        {
            return new List<TestModel>
            {
                new TestModel{Name = "Martin", Job = "Dev"},
                new TestModel{Name = "Bob", Job = "Dev"},
                new TestModel{Name = "John", Job = "Tester"},
                new TestModel{Name = "Steve", Job = "Manager"}
            };
        }

        internal static IEnumerable<TestModel> GetFilteredByDevJobData()
        {
            return new List<TestModel>
            {
                new TestModel{Name = "Martin", Job = "Dev"},
                new TestModel{Name = "Bob", Job = "Dev"},
            };
        }

        internal static IEnumerable<TestModel> GetSortedByNameData()
        {
            return new List<TestModel>
            {
                new TestModel{Name = "Bob", Job = "Dev"},
                new TestModel{Name = "John", Job = "Tester"},
                new TestModel{Name = "Martin", Job = "Dev"},
                new TestModel{Name = "Steve", Job = "Manager"}
            };
        }

        internal static IEnumerable<TestModel> GetFilteredAndSortedData()
        {
            return new List<TestModel>
            {
                new TestModel{Name = "Bob", Job = "Dev"},
                new TestModel{Name = "Martin", Job = "Dev"},
            };
        }
    }

    public class TestModel
    {
        public string Name { get; set; }
        public string Job { get; set; }
    }
}

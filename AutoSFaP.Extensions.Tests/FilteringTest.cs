using System.Linq;
using AutoSFaP.Models;
using AutoSFaP.Extensions;
using FluentAssertions;
using Xunit;

namespace AutoSFaP.Extension.Test
{
    public class FilteringTest
    {
        private readonly IQueryable<TestModel> testData;

        public FilteringTest()
        {
            testData = ModelHelper.GetTestData().AsQueryable();
        }

        [Fact]
        public void ShouldReturnOriginalDataWhenFilteringFieldEmpty()
        {
            // arrange
            var filterFields = new FilterField<TestModel>[0];
            // act
            var result = filterFields.Filter(testData);
            // assert
            result.Should().BeEquivalentTo(testData);
        }

        [Fact]
        public void ShouldReturnFilteredDataWhenStringFullNameFilteringFieldProvided()
        {
            // arrange
            const string propertyName = "Name";
            const string filterValue = "TestName_6";
            var filterField = new FilterField<TestModel>(propertyName, filterValue);
            var expectedData = testData.Where(t => t.Name == filterValue);
            // act
            var result = filterField.Filter(testData);
            // assert
            result.Should().BeEquivalentTo(expectedData);
        }

        [Fact]
        public void ShouldReturnEveryElement()
        {
            // arrange
            const string propertyName = "Name";
            const string filterValue = "TestName";
            var filterField = new FilterField<TestModel>(propertyName, filterValue);
            var expectedData = testData.Where(t => t.Name.Contains(filterValue));
            // act
            var result = filterField.Filter(testData);
            // assert
            result.Should().BeEquivalentTo(expectedData);
        }

        [Fact]
        public void ShouldReturnFilteredDataWhenNonStringFilteringFieldProvided()
        {
            // arrange
            const string propertyName = "IsEven";
            const bool filterValue = true;
            var filterField = new FilterField<TestModel>(propertyName, filterValue);
            var expectedData = testData.Where(t => t.IsEven == (filterValue));
            // act
            var result = filterField.Filter(testData);
            // assert
            result.Should().BeEquivalentTo(expectedData);
        }

        [Fact]
        public void ShouldSupportMultipleFiltering()
        {
            // arrange
            const bool isEvenFilterValue = true;
            const string nameFilterValue = "TestName_6";
            var filterFields = new FilterField<TestModel>[2];
            filterFields[0] = new FilterField<TestModel>
            {
                PropertyName = "IsEven",
                FilterValue = isEvenFilterValue
            };
            filterFields[1] = new FilterField<TestModel>
            {
                PropertyName = "Name",
                FilterValue = nameFilterValue
            };
            var expectedData = testData.Where(t => t.IsEven == (isEvenFilterValue))
                .Where(t => t.Name.Contains(nameFilterValue));
            // act
            var result = filterFields.Filter(testData);
            // assert
            result.Should().BeEquivalentTo(expectedData);
        }
    }
}

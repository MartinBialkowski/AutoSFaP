using System;
using System.Linq;
using AutoSFaP.Models;
using AutoSFaP.Extensions;
using FluentAssertions;
using Xunit;

namespace AutoSFaP.Extension.Test
{
    public class SortingTest
    {
        private readonly IQueryable<TestModel> testData;

        public SortingTest()
        {
            testData = ModelHelper.GetTestData().AsQueryable();
        }

        [Fact]
        public void ShouldReturnAscendingSortedData()
        {
            // arrange
            var sortField = new SortField<TestModel>
            {
                PropertyName = "Name",
                SortOrder = SortOrder.Ascending
            };
            // act
            var result = sortField.SortBy(testData);
            // assert
            result.Should().BeInAscendingOrder(x => x.Name);
        }

        [Fact]
        public void ShouldReturnDescendingSortedData()
        {
            // arrange
            var sortField = new SortField<TestModel>
            {
                PropertyName = "Name",
                SortOrder = SortOrder.Descending
            };
            // act
            var result = sortField.SortBy(testData);
            // assert
            result.Should().BeInDescendingOrder(x => x.Name);
        }

        [Fact]
        public void ShouldReturnSortedDataWhenArraySortFieldsProvided()
        {
            // arrange
            var exptectedData = testData.OrderBy(x => x.IsEven).ThenByDescending(x => x.Name);
            var sortFields = new SortField<TestModel>[2];
            sortFields[0] = new SortField<TestModel>
            {
                PropertyName = "IsEven",
                SortOrder = SortOrder.Ascending
            };
            sortFields[1] = new SortField<TestModel>
            {
                PropertyName = "Name",
                SortOrder = SortOrder.Descending
            };
            // act
            var result = sortFields.Sort(testData);
            // assert
            result.Should().BeEquivalentTo(exptectedData, options => options.WithStrictOrdering());
        }

        [Fact]
        public void ShouldThrowExceptionWhenSortFieldNull()
        {
            // act
            Action act = () => (null as SortField<TestModel>[]).Sort(testData);
            // assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ShouldThrowExceptionWhenSortFieldEmpty()
        {
            // arrange
            var sortFields = new SortField<TestModel>[0];
            // act
            Action act = () => sortFields.Sort(testData);
            // assert
            act.Should().Throw<ArgumentException>();
        }
    }
}

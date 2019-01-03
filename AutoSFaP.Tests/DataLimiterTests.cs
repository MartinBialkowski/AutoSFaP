using AutoSFaP.Models;
using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AutoSFaP.Tests
{
    public class DataLimiterTests
    {
        private readonly DataLimiter<TestModel> sut;
        private readonly IQueryable<TestModel> input;

        public DataLimiterTests()
        {
            sut = new DataLimiter<TestModel>();
            input = TestHelper.GetTestData().AsQueryable();
        }

        [Fact]
        public void ShouldReturnUnchangedDataWhenFilterAndSortModelsNotSpecyfied()
        {
            // arrange
            var expected = TestHelper.GetTestData().ToList();

            // act
            var result = sut.LimitData(input, null, null).ToList();

            // assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ShouldReturnSortedData()
        {
            // arrange
            var expected = TestHelper.GetSortedByNameData().ToList();
            var sortFields = GetSortFields();

            // act
            var result = sut.LimitData(input, sortFields, null);

            // assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ShouldReturnFilteredData()
        {
            // arrange
            var expected = TestHelper.GetFilteredByDevJobData().ToList();
            var filterFields = GetFilterFields();

            // act
            var result = sut.LimitData(input, null, filterFields);

            // assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ShouldReturnFilteredAndSortedData()
        {
            // arrange
            var expected = TestHelper.GetFilteredAndSortedData().ToList();
            var sortFields = GetSortFields();
            var filterFields = GetFilterFields();

            // act
            var result = sut.LimitData(input, sortFields, filterFields);

            // assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ShouldReturnPagedResult()
        {
            // arrange
            var paging = new Paging(pageNumber: 1, pageLimit: 2);
            var totalItems = input.Count();
            var expected = new PagedResult<TestModel>
            {
                Results = input.Skip(paging.Offset).Take(paging.PageLimit).ToList(),
                PageNumber = paging.PageNumber,
                PageSize = paging.PageLimit,
                TotalNumberOfRecords = totalItems,
                TotalNumberOfPages = totalItems / paging.PageLimit
            };
            // act
            var result = await sut.LimitDataAsync(input, null, null, paging);

            // assert
            result.Should().BeEquivalentTo(expected);
        }

        private FilterField<TestModel>[] GetFilterFields()
        {
            return new FilterField<TestModel>[]
            {
                new FilterField<TestModel>(propertyName: "Job", filterValue: "Dev")
            };
        }

        private SortField<TestModel>[] GetSortFields()
        {
            return new SortField<TestModel>[]
            {
                new SortField<TestModel>(propertyName: "Name", sortOrder: SortOrder.Ascending)
            };
        }
    }
}

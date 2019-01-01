using System.Linq;
using System.Threading.Tasks;
using AutoSFaP.Models;
using AutoSFaP.Extensions;
using FluentAssertions;
using Xunit;

namespace AutoSFaP.Extension.Test
{
    public class PagingTest
    {
        private int pageNumber;
        private int pageSize;
        private readonly TestModel[] testData;

        public PagingTest()
        {
            testData = ModelHelper.GetTestData();
        }

        [Fact]
        public async Task ShouldReturnWholeDataWhenPageSizeGreaterThanDataLength()
        {
            // arrange
            pageNumber = 1;
            pageSize = 20;
            var paging = new Paging(pageNumber, pageSize);
            // act
            var result = await paging.Page(testData.AsQueryable()).ToList();
            // assert
            result.Should().HaveSameCount(testData);
        }

        [Fact]
        public async Task ShouldReturnNothingWhenOffsetGreaterThanDataLength()
        {
            // arrange
            pageNumber = 20;
            pageSize = 1;
            var paging = new Paging(pageNumber, pageSize);
            // act
            var result = await paging.Page(testData.AsQueryable()).ToList();
            // assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task ShouldReturnDemandedPage()
        {
            // arrange
            pageNumber = 2;
            pageSize = 5;
            var paging = new Paging(pageNumber, pageSize);
            var expected = testData.Skip(pageSize);
            // act
            var result = await paging.Page(testData.AsQueryable()).ToList();
            // assert
            result.Should().HaveSameCount(expected);
        }
    }
}

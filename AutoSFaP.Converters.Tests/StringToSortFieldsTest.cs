using AutoSFaP.Models;
using FluentAssertions;
using System;
using Xunit;

namespace AutoSFaP.Converters.Test
{
    public class StringToSortFieldsTest
    {
        private readonly SortFieldsConverter<TestModel> converter;
        private readonly int expectedLength = 2;
        private readonly SortField<TestModel>[] expected;
        private SortField<TestModel>[] result;
        
        public StringToSortFieldsTest()
        {
            converter = new SortFieldsConverter<TestModel>();
            result = new SortField<TestModel>[0];
            expected = new SortField<TestModel>[expectedLength];
        }

        [Fact]
        public void ShouldReturnSortFieldsAscending()
        {
            // assign
            const string source = "Id,FieldName";
            expected[0] = new SortField<TestModel>
            {
                PropertyName = "Id",
                SortOrder = SortOrder.Ascending
            };
            expected[1] = new SortField<TestModel>
            {
                PropertyName = "FieldName",
                SortOrder = SortOrder.Ascending
            };
            // act
            result = converter.Convert(source, result, null);
            // assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ShouldReturnSortFieldsDescending()
        {
            // assign
            const string source = "Id-,FieldName-";
            expected[0] = new SortField<TestModel>
            {
                PropertyName = "Id",
                SortOrder = SortOrder.Descending
            };
            expected[1] = new SortField<TestModel>
            {
                PropertyName = "FieldName",
                SortOrder = SortOrder.Descending
            };
            // act
            result = converter.Convert(source, result, null);
            // assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ShouldReturnSortFieldsCaseInsensitive()
        {
            // assign
            const string source = "ID,FiEldNaMe-";
            expected[0] = new SortField<TestModel>
            {
                PropertyName = "Id",
                SortOrder = SortOrder.Ascending
            };
            expected[1] = new SortField<TestModel>
            {
                PropertyName = "FieldName",
                SortOrder = SortOrder.Descending
            };
            // act
            result = converter.Convert(source, result, null);
            // assert
            result.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("")]
        [InlineData("NotAField")]
        public void ShouldFailWhenNotValidDataProvided(string source)
        {
            // act
            Action act = () => converter.Convert(source, result, null);
            // assert
            act.Should().Throw<ArgumentException>();
        }
    }
}

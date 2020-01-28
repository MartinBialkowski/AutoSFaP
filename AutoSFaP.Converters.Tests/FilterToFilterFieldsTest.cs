using AutoSFaP.Models;
using FluentAssertions;
using Xunit;

namespace AutoSFaP.Converters.Test
{
    public class FilterToFilterFieldsTest
    {
        [Fact]
        public void ShouldConvertOnlyNotNullProperties()
        {
            // arrange
            const int expectedLength = 1;
            var source = new TestModelDto
            {
                FieldName = null,
                Id = 1
            };
            var expected = new FilterField<TestModel>[expectedLength];
            expected[0] = new FilterField<TestModel>
            {
                PropertyName = "Id",
                FilterValue = 1
            };
            // act
            var result = FilterFieldsConverter<TestModelDto, TestModel>.Convert(source);
            // assert
            result[0].Should().BeEquivalentTo(expected[0]);
        }
    }
}

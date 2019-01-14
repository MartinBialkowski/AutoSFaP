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
            var converter = new FilterFieldsConverter<TestModelDto, TestModel>();

            // assign
            const int expectedLength = 1;
            var source = new TestModelDto
            {
                FieldName = null,
                Id = 1
            };
            var result = new FilterField<TestModel>[0];
            var expected = new FilterField<TestModel>[expectedLength];
            expected[0] = new FilterField<TestModel>
            {
                PropertyName = "Id",
                FilterValue = 1
            };
            // act
            result = converter.Convert(source, result, null);
            // assert
            result[0].Should().BeEquivalentTo(expected[0]);
        }
    }
}

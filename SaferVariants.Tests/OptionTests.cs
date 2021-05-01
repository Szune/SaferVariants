using Xunit;

namespace SaferVariants.Tests
{
    public class OptionTests
    {
        [Fact]
        public void NoneIfNull_ShouldReturnNone_WhenArgumentIsNull()
        {
            var sut = Option.NoneIfNull<string>(null);
            Assert.IsType<None<string>>(sut);
        }
        
        [Fact]
        public void NoneIfNull_ShouldReturnSome_WhenArgumentIsNotNull()
        {
            var sut = Option.NoneIfNull("hello test");
            Assert.IsType<Some<string>>(sut);
            Assert.Equal("hello test", sut.ValueOr(""));
        }
    }
}
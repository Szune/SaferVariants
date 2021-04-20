using Xunit;

namespace SaferVariants.Tests
{
    public class SomeTests
    {
        [Fact]
        public void PatternMatchingOnSome()
        {
            IOption<string> sut = Option.Some("hello");
            if (sut is not Some<string>)
            {
                Assert.True(false, "We have a problem");
            }
        }
        
        [Fact]
        public void Map_ShouldBeNewValue_ForSomeVariant()
        {
            IOption<string> sut = Option.Some("hello_world");
            var result = sut.Map(s => Option.Some(s.Length));
            Assert.Equal("hello_world".Length, result.ValueOr(0));
        }
        
        [Fact]
        public void IsSome_ShouldBeTrue_ForSomeVariant()
        {
            IOption<string> sut = Option.Some("hello");
            Assert.True(sut.IsSome());
        }
        
        [Fact]
        public void ValueOr_ShouldBeValueFromSome_ForSomeVariant()
        {
            IOption<string> sut = Option.Some("hello");
            Assert.Equal("hello", sut.ValueOr("something else"));
        }
        
    }
}
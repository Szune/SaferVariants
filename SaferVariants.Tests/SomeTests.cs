using Xunit;

namespace SaferVariants.Tests
{
    public class SomeTests
    {
        [Fact]
        public void PatternMatchingOnSome()
        {
            var sut = Option.Some("hello");
            if (sut is not Some<string>)
            {
                Assert.True(false, "We have a problem");
            }
        }
        
        [Fact]
        public void Map_ShouldBeNewValue_ForSomeVariant()
        {
            var sut = Option.Some("hello_world");
            var result = sut.Map(s => Option.Some(s.Length));
            Assert.Equal("hello_world".Length, result.ValueOr(0));
        }
        
        [Fact]
        public void MapOr_ShouldBeNewValue_ForSomeVariant()
        {
            var sut = Option.Some("hello_world");
            var result = sut.MapOr(0, s => s.Length);
            Assert.Equal("hello_world".Length, result);
        }
        
        [Fact]
        public void Then_ShouldPerformAction_ForSomeVariant()
        {
            var i = 0;
            Option.Some("maybe").Then(_ => i += 1);
            Assert.Equal(1, i);
        }
        
        [Fact]
        public void IsSomeWithOutBinding_ShouldBeTrueAndBindValue_ForSomeVariant()
        {
            var sut = Option.Some("hello");
            if (sut.IsSome(out var val))
            {
                Assert.Equal("hello", val);
            }
            Assert.True(sut.IsSome(out _));
        }
        
        [Fact]
        public void IsSome_ShouldBeTrue_ForSomeVariant()
        {
            var sut = Option.Some("hello");
            Assert.True(sut.IsSome());
        }
        
        [Fact]
        public void ValueOr_ShouldBeValueFromSome_ForSomeVariant()
        {
            var sut = Option.Some("hello");
            Assert.Equal("hello", sut.ValueOr("something else"));
        }

        [Fact]
        public void HandleNone_ShouldNotPerformAction_ForSomeVariant()
        {
            var i = 0;
            var sut = Option.Some("hello");
            sut.HandleNone(() => i++);
            Assert.Equal(0, i);
        }
        
    }
}
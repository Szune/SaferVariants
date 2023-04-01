using Xunit;

namespace SaferVariants.Tests
{
    public class SomeTests
    {
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
            Option.Some("maybe").IfSome(_ => i += 1);
            Assert.Equal(1, i);
        }

        [Fact]
        public void IsSomeWithOutBinding_ShouldBeTrueAndBindValue_ForSomeVariant()
        {
            var sut = Option.Some("hello");
            if (sut.TryGetValue(out var val))
            {
                Assert.Equal("hello", val);
            }

            Assert.True(sut.TryGetValue(out _));
        }

        [Fact]
        public void IsSome_ShouldBeTrue_ForSomeVariant()
        {
            var sut = Option.Some("hello");
            Assert.True(sut.HasValue);
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
            sut.IfNone(() => i++);
            Assert.Equal(0, i);
        }
    }
}
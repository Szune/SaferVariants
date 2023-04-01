using Xunit;

namespace SaferVariants.Tests
{
    public class NoneTests
    {
        [Fact]
        public void Map_ShouldRemainNone_ForNoneVariant()
        {
            var sut = Option<string>.None;
            var result = sut.Map(s => Option.Some(s.Length));
            Assert.Equal(-1, result.ValueOr(-1));
        }

        [Fact]
        public void MapOr_ShouldReturnElseValue_ForNoneVariant()
        {
            var sut = Option<string>.None;
            var result = sut.MapOr(-10, s => s.Length);
            Assert.Equal(-10, result);
        }

        [Fact]
        public void Then_ShouldBeNoOp_ForNoneVariant()
        {
            Option<string>.None.IfSome(_ => Assert.True(false));
        }

        [Fact]
        public void IsSomeWithOutBinding_ShouldBeFalseAndNotBindValue_ForNoneVariant()
        {
            var sut = Option<string>.None;
            if (sut.TryGetValue(out var val))
            {
                Assert.True(false);
            }

            Assert.False(sut.TryGetValue(out _));
        }

        [Fact]
        public void IsSome_ShouldBeFalse_ForNoneVariant()
        {
            var sut = Option<object>.None;
            Assert.False(sut.HasValue);
        }

        [Fact]
        public void ValueOr_ShouldBeElseValue_ForNoneVariant()
        {
            var sut = Option<string>.None;
            Assert.Equal("something else", sut.ValueOr("something else"));
        }

        [Fact]
        public void HandleNone_ShouldPerformAction_ForNoneVariant()
        {
            var i = 0;
            var sut = Option<string>.None;
            sut.IfNone(() => i++);
            Assert.Equal(1, i);
        }
    }
}
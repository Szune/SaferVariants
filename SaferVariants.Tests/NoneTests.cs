using Xunit;

namespace SaferVariants.Tests
{
    public class NoneTests
    {
        [Fact]
        public void PatternMatchingOnNone()
        {
            IOption<string> sut = Option.None<string>();
            if (sut is not None<string>)
            {
                Assert.True(false, "We have a problem");
            }
        }
        
        [Fact]
        public void Map_ShouldRemainNone_ForNoneVariant()
        {
            IOption<string> sut = Option.None<string>();
            var result = sut.Map(s => Option.Some(s.Length));
            Assert.Equal(-1, result.ValueOr(-1));
        }
        
        [Fact]
        public void MapOr_ShouldReturnElseValue_ForNoneVariant()
        {
            var sut = Option.None<string>();
            var result = sut.MapOr(-10, s => s.Length);
            Assert.Equal(-10, result);
        }
        
        [Fact]
        public void Then_ShouldBeNoOp_ForNoneVariant()
        {
            Option.None<string>().Then(_ => Assert.True(false));
        }
        
        [Fact]
        public void IsSomeWithOutBinding_ShouldBeFalseAndNotBindValue_ForNoneVariant()
        {
            var sut = Option.None<string>();
            if (sut.IsSome(out var val))
            {
                Assert.True(false);
            }
            Assert.False(sut.IsSome(out _));
        }
        
        [Fact]
        public void IsSome_ShouldBeFalse_ForNoneVariant()
        {
            IOption<object> sut = Option.None<object>();
            Assert.False(sut.IsSome());
        }
        
        [Fact]
        public void ValueOr_ShouldBeElseValue_ForNoneVariant()
        {
            IOption<string> sut = Option.None<string>();
            Assert.Equal("something else", sut.ValueOr("something else"));
        }
        
        [Fact]
        public void HandleNone_ShouldPerformAction_ForNoneVariant()
        {
            var i = 0;
            var sut = Option.None<string>();
            sut.HandleNone(() => i++);
            Assert.Equal(1, i);
        }
    }
}
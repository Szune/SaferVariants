using System;
using SaferVariants.Extensions;
using Xunit;

namespace SaferVariants.Tests
{
    public class OptionTests
    {
        [Fact]
        public void MethodsTakingDelegates_ShouldThrow_IfDelegateArgumentIsNull()
        {
            var sut = Option.Some(new object());
            Assert.Throws<ArgumentNullException>(() => sut.Map<object>(null!));
            Assert.Throws<ArgumentNullException>(() => sut.MapOr(new object(), null!));
            Assert.Throws<ArgumentNullException>(() => sut.IfSome(null!));
            Assert.Throws<ArgumentNullException>(() => sut.IfNone(null!));
        }

        [Fact]
        public void NoneIfNull_ShouldReturnNone_WhenArgumentIsNull()
        {
            var sut = Option.NoneIfNull<string>(null!);
            Assert.False(sut.HasValue);
        }

        [Fact]
        public void NoneIfNull_ShouldReturnSome_WhenArgumentIsNotNull()
        {
            var sut = Option.NoneIfNull("hello test");
            Assert.True(sut.HasValue);
            Assert.Equal("hello test", sut.ValueOr(""));
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
        public void IfSome_ShouldPerformAction_ForSomeVariant()
        {
            var i = 0;
            Option.Some("maybe").IfSome(_ => i += 1);
            Assert.Equal(1, i);
        }

        [Fact]
        public void TryGetValueWithOutBinding_ShouldBeTrueAndBindValue_ForSomeVariant()
        {
            var sut = Option.Some("hello");
            if (sut.TryGetValue(out var val))
            {
                Assert.Equal("hello", val);
            }
            Assert.True(sut.TryGetValue(out _));
        }

        [Fact]
        public void HasValue_ShouldBeTrue_ForSomeVariant()
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

﻿using Xunit;

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
    }
}
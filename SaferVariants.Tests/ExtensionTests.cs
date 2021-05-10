using System;
using SaferVariants.Extensions;
using Xunit;

namespace SaferVariants.Tests
{
    public class ExtensionTests
    {
        [Fact]
        public void ToNoneIfNull_ShouldBeSome_IfNotNull()
        {
            Assert.True("definitely some".ToNoneIfNull().IsSome());
        }
        
        [Fact]
        public void ToNoneIfNull_ShouldBeNone_IfNull()
        {
            Assert.False((null as string).ToNoneIfNull().IsSome());
        }

        [Fact]
        public void ToSome_ShouldBeSome_IfNotNull()
        {
            Assert.True("some string".ToSome().IsSome());
        }
        
        [Fact]
        public void ToSome_ShouldThrow_IfNull()
        {
            void Act() => (null as string).ToSome();
            Assert.Throws<ArgumentNullException>(Act);
        }

        [Fact]
        public void ToNone_ShouldBeNone_Unconditionally()
        {
            Assert.False(1.ToNone().IsSome());
        }

        [Fact]
        public void ToOk_ShouldBeOk_IfNotNull()
        {
            Assert.True("Should be ok".ToOk<string,int>().IsOk());
        }
        
        [Fact]
        public void ToOk_ShouldThrow_IfNull()
        {
            void Act() => (null as string).ToOk<string,int>();
            Assert.Throws<ArgumentNullException>(Act);
        }

        [Fact]
        public void ToErr_ShouldBeErr_IfNotNull()
        {
            Assert.True(((int?) 1).ToErr<string, int?>().IsErr());
        }
        
        [Fact]
        public void ToErr_ShouldThrow_IfNull()
        {
            void Act() => (null as int?).ToErr<string,int?>();
            Assert.Throws<ArgumentNullException>(Act);
        }
    }
}
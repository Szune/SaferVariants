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
        
        [Fact]
        public void Flatten_Option_ShouldBeInner_IfOuterIsSome()
        {
            var option = Option.Some("hello world");
            var optionOuter = Option.Some(option);
            var flattened = optionOuter.Flatten();
            Assert.Equal("hello world", flattened.ValueOr(""));
        }
        
        [Fact]
        public void Flatten_Option_ShouldBeNone_IfOuterIsNone()
        {
            var optionOuter = Option.None<IOption<string>>();
            var flattened = optionOuter.Flatten();
            Assert.Equal("", flattened.ValueOr(""));
        }
        
        [Fact]
        public void Flatten_Result_ShouldBeInnerOk_IfOuterIsOk()
        {
            var resultOuter = Result.Ok<IResult<string,int>,int>(Result.Ok<string,int>("hello to all worlds"));
            var flattened = resultOuter.Flatten();
            Assert.Equal("hello to all worlds", flattened.ValueOr(""));
        }
        
        [Fact]
        public void Flatten_Result_ShouldBeOuterErr_IfOuterIsErr()
        {
            var resultOuter = Result.Err<IResult<string,int>,int>(3);
            var flattened = resultOuter.Flatten();
            Assert.Equal(3, flattened.IsErr(out var err) ? err : -1);
        }
        
        [Fact]
        public void Flatten_Result_ShouldBeInnerErr_IfOuterIsOkAndInnerIsErr()
        {
            var resultOuter = Result.Ok<IResult<string,int>,int>(Result.Err<string,int>(2));
            var flattened = resultOuter.Flatten();
            Assert.Equal(2, flattened.IsErr(out var err) ? err : -1);
        }
    }
}
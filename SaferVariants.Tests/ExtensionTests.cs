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
            Assert.True("definitely some".ToOption().HasValue);
        }

        [Fact]
        public void ToNoneIfNull_ShouldBeNone_IfNull()
        {
            Assert.False((null as string).ToOption().HasValue);
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
            var optionOuter = Option<Option<string>>.None;
            var flattened = optionOuter.Flatten();
            Assert.Equal("", flattened.ValueOr(""));
        }

        [Fact]
        public void Flatten_Result_ShouldBeInnerOk_IfOuterIsOk()
        {
            var resultOuter = Result.Ok<Result<string, int>, int>(Result.Ok<string, int>("hello to all worlds"));
            var flattened = resultOuter.Flatten();
            Assert.Equal("hello to all worlds", flattened.ValueOr(""));
        }

        [Fact]
        public void Flatten_Result_ShouldBeOuterErr_IfOuterIsErr()
        {
            var resultOuter = Result.Error<Result<string, int>, int>(3);
            var flattened = resultOuter.Flatten();
            Assert.Equal(3, flattened.TryGetError(out var err) ? err : -1);
        }

        [Fact]
        public void Flatten_Result_ShouldBeInnerErr_IfOuterIsOkAndInnerIsErr()
        {
            var resultOuter = Result.Ok<Result<string, int>, int>(Result.Error<string, int>(2));
            var flattened = resultOuter.Flatten();
            Assert.Equal(2, flattened.TryGetError(out var err) ? err : -1);
        }

        [Fact]
        public void ToOk_ShouldBeOk_IfNotNull()
        {
            Assert.True("Should be ok".ToOk<string, int>().IsOk);
        }

        [Fact]
        public void ToErr_ShouldBeErr_IfNotNull()
        {
            Assert.True(((int?)1).ToError<string, int?>().IsError);
        }
    }
}
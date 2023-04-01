using System;
using System.Text;
using SaferVariants.Extensions;
using Xunit;

namespace SaferVariants.Tests
{
    public class ResultTests
    {
        [Fact]
        public void MethodsTakingDelegates_ShouldThrow_IfDelegateArgumentIsNull()
        {
            var sut = Result.Ok<object, object>(new object());
            Assert.Throws<ArgumentNullException>(() => sut.Map<object, string>(null!, null!));
            Assert.Throws<ArgumentNullException>(() => sut.Map<object>(null!));
            Assert.Throws<ArgumentNullException>(() => sut.MapOr(new object(), null!));
            Assert.Throws<ArgumentNullException>(() => sut.HandleError(null!));
            Assert.Throws<ArgumentNullException>(() => sut.IfOk(null!));
            Assert.Throws<ArgumentNullException>(() => sut.IfError(null!));
        }

        [Fact]
        public void ResultWithNothingType()
        {
            // Arrange
            var value = Result.EmptyOk<int>();
            var error = Result.EmptyError<string>();
            // Assert
            Assert.True(value.IsOk);
            Assert.False(error.IsOk);
        }

        [Fact]
        public void MapResult()
        {
            // Arrange
            var sut = "hello".ToOk<string, int>();
            // Act
            var result = sut.Map(
                ok => new StringBuilder(ok).Append(" world"),
                err => (StringSplitOptions)err);
            // Assert
            Assert.True(result.IsOk);
            Assert.Equal(new StringBuilder("hello").Append(" world").ToString(), result.ValueOrThrow().ToString());
        }
    }
}

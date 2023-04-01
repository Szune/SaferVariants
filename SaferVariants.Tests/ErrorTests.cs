using Xunit;

namespace SaferVariants.Tests
{
    public class ErrTests
    {
        [Fact]
        public void ErrorOrThrow_ShouldThrow_IfErrorIsNull()
        {
            var sut = default(Result<int, string>);
            Assert.Throws<ValueNullException>(() => sut.ErrorOrThrow());
        }

        [Fact]
        public void TryGetError_ShouldThrow_IfErrorIsNull()
        {
            var sut = default(Result<int, string>);
            Assert.Throws<ValueNullException>(() => sut.TryGetError(out var _));
        }

        [Fact]
        public void Map_ShouldRemainErr_ForErrVariant()
        {
            var sut = Result.Error<int, string>("biggest error owest");
            var result = sut.Map(s => Result.Ok<int, string>(s * s));
            Assert.Equal(-101, result.ValueOr(-101));
        }

        [Fact]
        public void MapErr_ShouldBeNewErr_ForErrVariant()
        {
            var sut = Result.Error<int, string>("interesting error");
            var result = sut
                .MapErr(err => err.Length)
                .Map(num => Result.Ok<int, int>(num * num));

            if (!result.TryGetError(out var error))
            {
                Assert.True(false);
            }

            Assert.Equal(17, error);
        }

        [Fact]
        public void MapOr_ShouldReturnElseValue_ForErrVariant()
        {
            var sut = Result.Error<int, string>("no");
            var result = sut.MapOr(1, value => value * value);
            Assert.Equal(1, result);
        }

        [Fact]
        public void HandleError_ShouldCallErrorHandler_ForErrVariant()
        {
            var i = 0;
            Result.Error<int, string>("LBTM").HandleError(_ => i += 1);
            Assert.Equal(1, i);
        }

        [Fact]
        public void HandleError_ShouldReturnNone_ForErrVariant()
        {
            var sut = Result.Error<string, int>(33).HandleError(_ => { });
            Assert.False(sut.HasValue);
        }

        [Fact]
        public void Then_ShouldNotPerformAction_ForErrVariant()
        {
            Result.Error<string, int>(1).IfOk(_ => Assert.True(false));
        }

        [Fact]
        public void IsOkWithOutBinding_ShouldBeFalseAndNotBindValue_ForErrVariant()
        {
            var sut = Result.Error<string, int>(-1);
            if (sut.TryGetValue(out var ok))
            {
                Assert.True(false);
            }

            Assert.False(sut.TryGetValue(out _));
        }

        [Fact]
        public void IsOk_ShouldBeFalse_ForErrVariant()
        {
            var sut = Result.Error<int, int>(-1);
            Assert.False(sut.IsOk);
        }

        [Fact]
        public void IsErrWithOutBinding_ShouldBeTrueAndBindValue_ForErrVariant()
        {
            var sut = Result.Error<int, int>(3);
            if (sut.TryGetError(out var err))
            {
                Assert.Equal(3, err);
            }

            Assert.True(sut.TryGetError(out _));
        }

        [Fact]
        public void IsErr_ShouldBeTrue_ForErrVariant()
        {
            var sut = Result.Error<int, int>(3);
            Assert.True(!sut.IsOk);
        }


        [Fact]
        public void ValueOr_ShouldBeElseValue_ForErrVariant()
        {
            var sut = Result.Error<int, int>(-1);
            Assert.Equal(1, sut.ValueOr(1));
        }
    }
}

using Xunit;

namespace SaferVariants.Tests
{
    public class OkTests
    {
        [Fact]
        public void Map_ShouldBeNewResult_ForOkVariant()
        {
            var sut = Result.Ok<int, int>(1);
            var result = sut.Bind(s => Result.Ok<byte, int>((byte)s));
            Assert.Equal(1, result.ValueOr(0));
        }

        [Fact]
        public void MapOr_ShouldReturnTransformedValue_ForOkVariant()
        {
            var sut = Result.Ok<string, string>("yes");
            var result = sut.MapOr(0, value => value.Length);
            Assert.Equal(3, result);
        }

        [Fact]
        public void Bind_ShouldBeNewErr_ForOkVariantMapReturningErr()
        {
            var sut = Result.Ok<int, int>(1);
            var result = sut.Bind(_ => Result.Error<byte, int>(-100));
            if (result.TryGetError(out var err))
            {
                Assert.Equal(-100, err);
            }
            else
            {
                Assert.True(false, "We have a problem");
            }
        }

        [Fact]
        public void HandleError_ShouldNotCallErrorHandler_ForOkVariant()
        {
            var i = 0;
            Result.Ok<string, int>("LGTM").HandleError(_ => i += 1);
            Assert.Equal(0, i);
        }

        [Fact]
        public void HandleError_ShouldReturnSome_ForOkVariant()
        {
            var sut = Result.Ok<string, int>("LGTM").HandleError(_ => { });
            Assert.Equal("LGTM", sut.ValueOr(""));
        }

        [Fact]
        public void Then_ShouldPerformAction_ForOkVariant()
        {
            var i = 0;
            Result.Ok<string, int>("LGTM").IfOk(_ => i += 1);
            Assert.Equal(1, i);
        }

        [Fact]
        public void IsOkWithOutBinding_ShouldBeTrueAndBindValue_ForOkVariant()
        {
            var sut = Result.Ok<string, int>("good day, buddy");
            if (sut.TryGetValue(out var ok))
            {
                Assert.Equal("good day, buddy", ok);
            }

            Assert.True(sut.TryGetValue(out _));
        }

        [Fact]
        public void IsOk_ShouldBeTrue_ForOkVariant()
        {
            var sut = Result.Ok<int, int>(3);
            Assert.True(sut.IsOk);
        }

        [Fact]
        public void IsErrorWithOutBinding_ShouldBeFalseAndNotBindValue_ForOkVariant()
        {
            var sut = Result.Ok<int, int>(3);
            if (sut.TryGetError(out var err))
            {
                Assert.True(false);
            }

            Assert.False(sut.TryGetError(out _));
        }

        [Fact]
        public void IsError_ShouldBeFalse_ForOkVariant()
        {
            var sut = Result.Ok<int, int>(3);
            Assert.False(!sut.IsOk);
        }

        [Fact]
        public void ValueOr_ShouldBeValueFromOk_ForOkVariant()
        {
            var sut = Result.Ok<int, int>(3);
            Assert.Equal(3, sut.ValueOr(5));
        }
    }
}

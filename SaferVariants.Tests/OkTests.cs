using Xunit;

namespace SaferVariants.Tests
{
    public class OkTests
    {
        [Fact]
        public void PatternMatchingOnOk()
        {
            IResult<int,int> sut = Result.Ok<int,int>(1);
            switch (sut)
            {
                case Ok<int,int> ok:
                    Assert.Equal(1, ok.Value);
                    break;
                default:
                    Assert.True(false, "We have a problem");
                    break;
            }

            if (sut is not Ok<int, int>)
            {
                Assert.True(false, "We have a problem");
            }
        }

        [Fact]
        public void Map_ShouldBeNewResult_ForOkVariant()
        {
            var sut = Result.Ok<int, int>(1);
            var result = sut.Map(s => Result.Ok<byte, int>((byte) s));
            Assert.Equal(1, result.ValueOr(0));
        }

        [Fact]
        public void Map_ShouldBeNewErr_ForOkVariantMapReturningErr()
        {
            var sut = Result.Ok<int, int>(1);
            var result = sut.Map(_ => Result.Err<byte, int>(-100));
            if (result is Err<byte, int> err)
            {
                Assert.Equal(-100, err.Error);
            }
            else
            {
                Assert.True(false, "We have a problem");
            }
        }

        [Fact]
        public void Then_ShouldPerformAction_ForOkVariant()
        {
            var i = 0;
            Result.Ok<string, int>("LGTM").Then(_ => i += 1);
            Assert.Equal(1, i);
        }

        [Fact]
        public void IsOkWithOutBinding_ShouldBeTrueAndBindValue_ForOkVariant()
        {
            var sut = Result.Ok<string, int>("good day, buddy");
            if (sut.IsOk(out var ok))
            {
                Assert.Equal("good day, buddy", ok);
            }

            Assert.True(sut.IsOk(out _));
        }

        [Fact]
        public void IsOk_ShouldBeTrue_ForOkVariant()
        {
            var sut = Result.Ok<int, int>(3);
            Assert.True(sut.IsOk());
        }

        [Fact]
        public void IsErrorWithOutBinding_ShouldBeFalseAndNotBindValue_ForOkVariant()
        {
            var sut = Result.Ok<int, int>(3);
            if (sut.IsErr(out var err))
            {
                Assert.True(false);
            }

            Assert.False(sut.IsErr(out _));
        }

        [Fact]
        public void IsError_ShouldBeFalse_ForOkVariant()
        {
            var sut = Result.Ok<int, int>(3);
            Assert.False(sut.IsErr());
        }

        [Fact]
        public void ValueOr_ShouldBeValueFromOk_ForOkVariant()
        {
            var sut = Result.Ok<int, int>(3);
            Assert.Equal(3, sut.ValueOr(5));
        }
    }
}
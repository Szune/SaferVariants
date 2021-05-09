using Xunit;

namespace SaferVariants.Tests
{
    public class ErrTests
    {
        [Fact]
        public void PatternMatchingOnErr()
        {
            IResult<int, string> sut = Result.Err<int, string>("big error ow");
            if (sut is not Err<int,string>)
            {
                Assert.True(false, "We have a problem");
            }
        }

        [Fact]
        public void Map_ShouldRemainErr_ForErrVariant()
        {
            IResult<int,string> sut = Result.Err<int,string>("biggest error owest");
            var result = sut.Map(s => Result.Ok<int,string>(s * s));
            Assert.Equal(-101, result.ValueOr(-101));
        }

        [Fact]
        public void Then_ShouldNotPerformAction_ForErrVariant()
        {
            Result.Err<string, int>(1).Then(_ => Assert.True(false));
        }

        [Fact]
        public void IsOkWithOutBinding_ShouldBeFalseAndNotBindValue_ForErrVariant()
        {
            var sut = Result.Err<string, int>(-1);
            if (sut.IsOk(out var ok))
            {
                Assert.True(false);
            }

            Assert.False(sut.IsOk(out _));
        }

        [Fact]
        public void IsOk_ShouldBeFalse_ForErrVariant()
        {
            IResult<int, int> sut = Result.Err<int, int>(-1);
            Assert.False(sut.IsOk());
        }

        [Fact]
        public void IsErrWithOutBinding_ShouldBeTrueAndBindValue_ForErrVariant()
        {
            var sut = Result.Err<int, int>(3);
            if (sut.IsErr(out var err))
            {
                Assert.Equal(3, err);
            }

            Assert.True(sut.IsErr(out _));
        }

        [Fact]
        public void IsErr_ShouldBeTrue_ForErrVariant()
        {
            var sut = Result.Err<int, int>(3);
            Assert.True(sut.IsErr());
        }


        [Fact]
        public void ValueOr_ShouldBeElseValue_ForErrVariant()
        {
            IResult<int, int> sut = Result.Err<int, int>(-1);
            Assert.Equal(1, sut.ValueOr(1));
        }
    }
}
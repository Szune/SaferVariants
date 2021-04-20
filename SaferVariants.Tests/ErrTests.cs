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
        public void IsOk_ShouldBeFalse_ForErrVariant()
        {
            IResult<int, int> sut = Result.Err<int, int>(-1);
            Assert.False(sut.IsOk());
        }
        
        [Fact]
        public void ValueOr_ShouldBeElseValue_ForErrVariant()
        {
            IResult<int, int> sut = Result.Err<int, int>(-1);
            Assert.Equal(1, sut.ValueOr(1));
        }
        
    }
}
using System;
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
                    Assert.Equal(1,ok.Value);
                    break;
                default:
                    Assert.True(false, "We have a problem");
                    break;
            }
            if (sut is not Ok<int,int>)
            {
                Assert.True(false, "We have a problem");
            }
        }
        
        [Fact]
        public void Map_ShouldBeNewResult_ForOkVariant()
        {
            var sut = Result.Ok<int,int>(1);
            var result = sut.Map(s => Result.Ok<byte,int>((byte)s));
            Assert.Equal(1, result.ValueOr(0));
        }
        
        [Fact]
        public void Map_ShouldBeNewErr_ForOkVariantMapReturningErr()
        {
            var sut = Result.Ok<int,int>(1);
            var result = sut.Map(_ => Result.Err<byte,int>(-100));
            if (result is Err<byte,int> err)
            {
                Assert.Equal(-100, err.Error);
            }
            else
            {
                Assert.True(false, "We have a problem");
            }
        }
        
        [Fact]
        public void IsOk_ShouldBeTrue_ForOkVariant()
        {
            var sut = Result.Ok<int, int>(3);
            Assert.True(sut.IsOk());
        }
        
        [Fact]
        public void ValueOr_ShouldBeValueFromOk_ForOkVariant()
        {
            var sut = Result.Ok<int, int>(3);
            Assert.Equal(3, sut.ValueOr(5));
        }
        
    }
}
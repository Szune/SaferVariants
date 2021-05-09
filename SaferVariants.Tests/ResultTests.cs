using System;
using SaferVariants.Extensions;
using Xunit;

namespace SaferVariants.Tests
{
    public class ResultTests
    {
        private class InvalidVariant : IResult<string, int>
        {
            public IResult<TResult, int> Map<TResult>(Func<string, IResult<TResult, int>> transform) => throw new NotImplementedException();
            public void Then(Action<string> action)
            {
                throw new NotImplementedException();
            }

            public string ValueOr(string elseValue) => throw new NotImplementedException();
            public bool IsOk(out string value)
            {
                throw new NotImplementedException();
            }

            public bool IsOk() => throw new NotImplementedException();
            public bool IsErr(out int error)
            {
                throw new NotImplementedException();
            }

            public bool IsErr()
            {
                throw new NotImplementedException();
            }
        }
        
        [Fact]
        public void EnsureValid_ShouldNotThrow_IfValidVariant()
        {
            var sut = Result.Ok<string, int>("hello").EnsureValid();
            sut = Result.Err<string, int>(1).EnsureValid();
            Result.EnsureValid(sut);
            Assert.Equal("", sut.ValueOr(""));
        }
        
        [Fact]
        public void EnsureValid_ShouldThrow_IfInvalidVariant()
        {
            IResult<string, int> sut = new InvalidVariant();
            void Act() => sut.EnsureValid();
            Assert.Throws<InvalidResultVariantException>(Act);
            void Act2() => Result.EnsureValid(sut);
            Assert.Throws<InvalidResultVariantException>(Act2);
        }
        
        [Fact]
        public void EnsureValid_ShouldThrow_IfNull()
        {
            IResult<string, int> sut = null;
            void Act() => sut.EnsureValid();
            Assert.Throws<InvalidResultVariantException>(Act);
            void Act2() => Result.EnsureValid(sut);
            Assert.Throws<InvalidResultVariantException>(Act2);
        }
        
        [Fact]
        public void Invalid_ShouldReturn_Exception()
        {
            // more of a reminder than a test
            IResult<string, int> sut = null;
            switch (sut)
            {
                case Ok<string,int> ok:
                    break;
                case Err<string,int> err:
                    break;
                default:
                    Action act = () => throw Result.Invalid();
                    Assert.Throws<InvalidResultVariantException>(act);
                    break;
            }


            Action throwAgain = () =>
            {
                var test = sut switch
                {
                    Ok<string, int> x => "hello",
                    Err<string, int> y => "world",
                    _ => throw Result.Invalid(),
                };
            };
            Assert.Throws<InvalidResultVariantException>(throwAgain);
        }
    }
}
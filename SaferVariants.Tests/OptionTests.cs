using System;
using SaferVariants.Extensions;
using Xunit;

namespace SaferVariants.Tests
{
    public class OptionTests
    {
        
        private class InvalidVariant : IOption<string>
        {
            public IOption<TResult> Map<TResult>(Func<string, IOption<TResult>> transform) => throw new NotImplementedException();
            public TResult MapOr<TResult>(TResult elseValue, Func<string, TResult> transform)
            {
                throw new NotImplementedException();
            }

            public bool IsSome(out string value)
            {
                throw new NotImplementedException();
            }

            public void Then(Action<string> action)
            {
                throw new NotImplementedException();
            }

            public string ValueOr(string elseValue) => throw new NotImplementedException();

            public bool IsSome() => throw new NotImplementedException();
        }
        
        [Fact]
        public void EnsureValid_ShouldNotThrow_IfValidVariant()
        {
            var sut = Option.Some("hej").EnsureValid();
            sut = Option.None<string>().EnsureValid();
            Option.EnsureValid(sut);
            Assert.Equal("", sut.ValueOr(""));
        }
        
        [Fact]
        public void EnsureValid_ShouldThrow_IfInvalidVariant()
        {
            IOption<string> sut = new InvalidVariant();
            void Act() => sut.EnsureValid();
            Assert.Throws<InvalidOptionVariantException>(Act);
            void Act2() => Option.EnsureValid(sut);
            Assert.Throws<InvalidOptionVariantException>(Act2);
        }
        
        [Fact]
        public void EnsureValid_ShouldThrow_IfNull()
        {
            IOption<string> sut = null;
            void Act() => sut.EnsureValid();
            Assert.Throws<InvalidOptionVariantException>(Act);
            void Act2() => Option.EnsureValid(sut);
            Assert.Throws<InvalidOptionVariantException>(Act2);
        }
        
        [Fact]
        public void SomeVariant_ShouldThrow_IfDelegateArgumentIsNull()
        {
            var sut = Option.Some(new object());
            Assert.Throws<ArgumentNullException>(() => sut.Map<object>(null));
            Assert.Throws<ArgumentNullException>(() => sut.MapOr(new object(), null));
            Assert.Throws<ArgumentNullException>(() => sut.Then(null));
        }
        
        [Fact]
        public void NoneVariant_ShouldThrow_IfDelegateArgumentIsNull()
        {
            var sut = Option.None<object>();
            Assert.Throws<ArgumentNullException>(() => sut.Map<object>(null));
            Assert.Throws<ArgumentNullException>(() => sut.MapOr(new object(), null));
            Assert.Throws<ArgumentNullException>(() => sut.Then(null));
        }
        
        [Fact]
        public void NoneIfNull_ShouldReturnNone_WhenArgumentIsNull()
        {
            var sut = Option.NoneIfNull<string>(null);
            Assert.IsType<None<string>>(sut);
        }
        
        [Fact]
        public void NoneIfNull_ShouldReturnSome_WhenArgumentIsNotNull()
        {
            var sut = Option.NoneIfNull("hello test");
            Assert.IsType<Some<string>>(sut);
            Assert.Equal("hello test", sut.ValueOr(""));
        }
        
        [Fact]
        public void Invalid_ShouldReturn_Exception()
        {
            // more of a reminder than a test
            IOption<string> sut = null;
            switch (sut)
            {
                case Some<string> s:
                    break;
                case None<string>:
                    break;
                default:
                    Action act = () => throw Option.Invalid();
                    Assert.Throws<InvalidOptionVariantException>(act);
                    break;
            }

            Action throwAgain = () =>
            {
                var test = sut switch
                {
                    Some<string> => "hello",
                    None<string> => "world",
                    _ => throw Option.Invalid(),
                };
            };
            Assert.Throws<InvalidOptionVariantException>(throwAgain);
        }
    }
}
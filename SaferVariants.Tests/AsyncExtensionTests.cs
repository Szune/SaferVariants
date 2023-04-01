using System.Threading.Tasks;
using SaferVariants.AsyncExtensions;
using Xunit;

namespace SaferVariants.Tests
{
    public class AsyncExtensionTests
    {
        public class OptionTests
        {
            [Fact]
            public async Task IfSomeAsync_ShouldPerformAction_ForSomeVariant()
            {
                var option = Option.Some("something");
                var str = "more than";
                await option.IfSomeAsync(async s =>
                {
                    await Task.Delay(0);
                    str += " " + s;
                });

                Assert.Equal("more than something", str);
            }

            [Fact]
            public async Task IfSomeAsync_ShouldNotPerformAction_ForNoneVariant()
            {
                Option<string> option = null;
                var str = "less than";
                await option.IfSomeAsync(async s =>
                {
                    await Task.Delay(0);
                    str += " " + s;
                });

                Assert.Equal("less than", str);
            }
        }

        public class ResultTests
        {
            [Fact]
            public async Task HandleErrorAsync_ShouldPerformAction_ForErrVariant()
            {
                var result = Result.Error<string, int>(3);
                var str = "less than";
                await result.HandleErrorAsync(async s =>
                {
                    await Task.Delay(0);
                    str += " " + s;
                });

                Assert.Equal("less than 3", str);
            }

            [Fact]
            public async Task HandleErrorAsync_ShouldNotPerformAction_ForOkVariant()
            {
                var result = Result.Ok<string, int>("nothing");
                var str = "less than";
                await result.HandleErrorAsync(async s =>
                {
                    await Task.Delay(0);
                    str += " " + s;
                });

                Assert.Equal("less than", str);
            }

            [Fact]
            public async Task IfOkAsync_ShouldPerformAction_ForOkVariant()
            {
                var result = Result.Ok<string, int>("this");
                var str = "more than";
                await result.IfOkAsync(async s =>
                {
                    await Task.Delay(0);
                    str += " " + s;
                });

                Assert.Equal("more than this", str);
            }

            [Fact]
            public async Task IfOkAsync_ShouldNotPerformAction_ForErrVariant()
            {
                var result = Result.Error<string, int>(3);
                var str = "less than";
                await result.IfOkAsync(async s =>
                {
                    await Task.Delay(0);
                    str += " " + s;
                });

                Assert.Equal("less than", str);
            }

            [Fact]
            public async Task IfOkAsync_ShouldPerformAction_ForOkVariantAfterHandleErrorAsync()
            {
                var i = 0;
                var result = Result.Ok<string, int>("this");
                var str = "more than";
                await result
                    .HandleErrorAsync(async _ =>
                    {
                        await Task.Delay(0);
                        i++;
                    })
                    .IfSomeAsync(async s =>
                    {
                        await Task.Delay(0);
                        str += " " + s;
                    });

                Assert.Equal("more than this", str);
                Assert.Equal(0, i);
            }
        }
    }
}

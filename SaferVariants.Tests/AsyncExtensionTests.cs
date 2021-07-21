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
            public async Task ThenAsync_ShouldPerformAction_ForSomeVariant()
            {
                var option = Option.Some("something");
                var str = "more than";
                await option.ThenAsync(async s =>
                {
                    await Task.Delay(0);
                    str += " " + s;
                });

                Assert.Equal("more than something", str);
            }

            [Fact]
            public async Task ThenAsync_ShouldNotPerformAction_ForNoneVariant()
            {
                var option = Option.None<string>();
                var str = "less than";
                await option.ThenAsync(async s =>
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
                var result = Result.Err<string, int>(3);
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
            public async Task ThenAsync_ShouldPerformAction_ForOkVariant()
            {
                var result = Result.Ok<string, int>("this");
                var str = "more than";
                await result.ThenAsync(async s =>
                {
                    await Task.Delay(0);
                    str += " " + s;
                });

                Assert.Equal("more than this", str);
            }

            [Fact]
            public async Task ThenAsync_ShouldNotPerformAction_ForErrVariant()
            {
                var result = Result.Err<string, int>(3);
                var str = "less than";
                await result.ThenAsync(async s =>
                {
                    await Task.Delay(0);
                    str += " " + s;
                });

                Assert.Equal("less than", str);
            }

            [Fact]
            public async Task ThenAsync_ShouldPerformAction_ForOkVariantAfterHandleErrorAsync()
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
                    .ThenAsync(async s =>
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
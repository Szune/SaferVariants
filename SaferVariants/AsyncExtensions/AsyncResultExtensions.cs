using System;
using System.Threading.Tasks;

namespace SaferVariants.AsyncExtensions
{
    public static class AsyncResultExtensions
    {
        public static async Task<IOption<T>> HandleErrorAsync<T, E>(this IResult<T, E> self, Func<E, Task> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            switch (self)
            {
                case Err<T, E> err:
                    await action(err.Error).ConfigureAwait(false);
                    return Option.None<T>();
                case Ok<T, E> ok:
                    return Option.Some(ok.Value);
                default:
                    throw Result.Invalid();
            }
        }

        public static async Task ThenAsync<T, E>(this IResult<T, E> self, Func<T, Task> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if (self.IsOk(out var value))
            {
                await action(value).ConfigureAwait(false);
            }
        }

        public static async Task ThenAsync<T>(this Task<IOption<T>> self, Func<T, Task> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if ((await self.ConfigureAwait(false))
                .IsSome(out var value))
            {
                await action(value).ConfigureAwait(false);
            }
        }
    }
}
using System;
using System.Threading.Tasks;

namespace SaferVariants.AsyncExtensions
{
    public static class AsyncResultExtensions
    {
        public static async Task<Option<T>> HandleErrorAsync<T, E>(this Result<T, E> self, Func<E, Task> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if (self.IsOk)
            {
                return new Option<T>(self._value);
            }
            else
            {
                await action(self._error).ConfigureAwait(false);
                return Option<T>.None;
            }
        }

        public static async Task<Option<E>> HandleOkAsync<T, E>(this Result<T, E> self, Func<T, Task> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if (self.IsError)
            {
                return new Option<E>(self._error);
            }
            else
            {
                await action(self._value).ConfigureAwait(false);
                return Option<E>.None;
            }
        }

        public static async Task<Result<T, E>> IfOkAsync<T, E>(this Result<T, E> self, Func<T, Task> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if (self.IsOk)
            {
                await action(self._value).ConfigureAwait(false);
            }

            return self;
        }

        public static async Task<Result<T, E>> IfErrorAsync<T, E>(this Result<T, E> self, Func<E, Task> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if (self.IsError)
            {
                await action(self._error).ConfigureAwait(false);
            }

            return self;
        }

        public static async Task<Option<T>> HandleErrorAsync<T, E>(this Task<Result<T, E>> self, Func<E, Task> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var result = await self.ConfigureAwait(false);
            if (result.IsOk)
            {
                return new Option<T>(result._value);
            }
            else
            {
                await action(result._error).ConfigureAwait(false);
                return Option<T>.None;
            }
        }

        public static async Task<Option<E>> HandleOkAsync<T, E>(this Task<Result<T, E>> self, Func<T, Task> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var result = await self.ConfigureAwait(false);
            if (result.IsError)
            {
                return new Option<E>(result._error);
            }
            else
            {
                await action(result._value).ConfigureAwait(false);
                return Option<E>.None;
            }
        }

        public static async Task<Result<T, E>> IfOkAsync<T, E>(this Task<Result<T, E>> self, Func<T, Task> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var result = await self.ConfigureAwait(false);
            if (result.IsOk)
            {
                await action(result._value).ConfigureAwait(false);
            }

            return result;
        }

        public static async Task<Result<T, E>> IfErrorAsync<T, E>(this Task<Result<T, E>> self, Func<E, Task> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var result = await self.ConfigureAwait(false);
            if (result.IsError)
            {
                await action(result._error).ConfigureAwait(false);
            }

            return result;
        }
    }
}

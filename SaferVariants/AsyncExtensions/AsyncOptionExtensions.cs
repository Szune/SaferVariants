using System;
using System.Threading.Tasks;

namespace SaferVariants.AsyncExtensions
{
    public static class AsyncOptionExtensions
    {
        public static async Task IfSomeAsync<T>(this Option<T> self, Func<T, Task> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if (self.HasValue)
            {
                await action(self._value).ConfigureAwait(false);
            }
        }

        public static async Task IfSomeAsync<T>(this Task<Option<T>> self, Func<T, Task> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if ((await self.ConfigureAwait(false)).TryGetValue(out var value))
            {
                await action(value).ConfigureAwait(false);
            }
        }
    }
}

using System;
using System.Threading.Tasks;

namespace SaferVariants.AsyncExtensions
{
    public static class AsyncOptionExtensions
    {
        public static async Task ThenAsync<T>(this IOption<T> self, Func<T, Task> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            
            if (self.IsSome(out var value))
            {
                await action(value).ConfigureAwait(false);
            }
        }
    }
}
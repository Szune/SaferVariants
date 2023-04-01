namespace SaferVariants.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="Option{T}"/> and <see cref="Result{TValue,TError}"/> instances.
    /// </summary>
    public static class Extensions
    {
        public static Option<TValue> ToOption<TValue>(this TValue value)
        {
            return value;
        }

        public static Result<TValue, TError> ToOk<TValue, TError>(this TValue value)
        {
            return value;
        }

        public static Result<TValue, TError> ToError<TValue, TError>(this TError error)
        {
            return error;
        }

        /// <summary>
        /// Returns the inner <see cref="Option{T}"/>.
        /// </summary>
        public static Option<T> Flatten<T>(this Option<Option<T>> option)
        {
            return option.ValueOr(Option<T>.None);
        }

        /// <summary>
        /// Returns the inner <see cref="Result{TValue,TError}"/>.
        /// </summary>
        public static Result<TValue, TError> Flatten<TValue, TError>(this Result<Result<TValue, TError>, TError> result)
        {
            return result.Bind(_ => _);
        }
    }
}

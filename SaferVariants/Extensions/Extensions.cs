using System.Runtime.CompilerServices;

namespace SaferVariants.Extensions
{
    public static class Extensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IOption<T> EnsureValid<T>(this IOption<T> value, [CallerMemberName] string method = "",
            [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            // Option.Invalid
            switch (value)
            {
                case Some<T> _:
                case None<T> _:
                    break;
                default:
                    throw Option.Invalid(method, filePath, lineNumber);
            }
            return value;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IResult<TValue,TError> EnsureValid<TValue,TError>(this IResult<TValue,TError> value, [CallerMemberName] string method = "",
            [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            // Result.Invalid
            switch (value)
            {
                case Ok<TValue,TError> _:
                case Err<TValue,TError> _:
                    break;
                default:
                    throw Result.Invalid(method, filePath, lineNumber);
            }
            return value;
        }
    }
}
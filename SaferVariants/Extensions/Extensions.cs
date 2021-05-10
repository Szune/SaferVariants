using System;
using System.Runtime.CompilerServices;

namespace SaferVariants.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// Returns <see cref="SaferVariants.None{T}"/> if the value is null, returns the value wrapped in <see cref="SaferVariants.Some{T}"/> otherwise.
        /// </summary>
        public static IOption<T> ToNoneIfNull<T>(this T value)
        {
            if (value == null)
            {
                return Option.None<T>();
            }

            return Option.Some(value);
        }

        /// <summary>
        /// Returns the value wrapped in <see cref="Some{T}"/>, throws an exception if the value is null.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static IOption<T> ToSome<T>(this T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return Option.Some(value);
        }

        /// <summary>
        /// Returns <see cref="None{T}"/> unconditionally.
        /// </summary>
        public static IOption<T> ToNone<T>(this T _)
        {
            return Option.None<T>();
        }

        /// <summary>
        /// Returns the value wrapped in <see cref="Ok{TValue,TError}"/>, throws an exception if the value is null.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static IResult<T, E> ToOk<T, E>(this T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            return Result.Ok<T, E>(value);
        }

        /// <summary>
        /// Returns the value wrapped in <see cref="Err{TValue,TError}"/>, throws an exception if the value is null.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static IResult<T, E> ToErr<T, E>(this E value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            return Result.Err<T, E>(value);
        }

        /// <summary>
        /// Ensures that the <see cref="IOption{T}"/> is a valid option variant, throws an exception otherwise.
        /// </summary>
        /// <exception cref="InvalidOptionVariantException"></exception>
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

        /// <summary>
        /// Ensures that the <see cref="IResult{TValue,TError}"/> is a valid result variant, throws an exception otherwise.
        /// </summary>
        /// <exception cref="InvalidResultVariantException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IResult<TValue, TError> EnsureValid<TValue, TError>(this IResult<TValue, TError> value,
            [CallerMemberName] string method = "",
            [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            // Result.Invalid
            switch (value)
            {
                case Ok<TValue, TError> _:
                case Err<TValue, TError> _:
                    break;
                default:
                    throw Result.Invalid(method, filePath, lineNumber);
            }

            return value;
        }
    }
}
using System;
using System.Runtime.CompilerServices;

namespace SaferVariants.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="IOption{T}"/> and <see cref="IResult{TValue,TError}"/> interfaces.
    /// </summary>
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
        /// Returns the inner <see cref="IOption{T}"/>. An invalid variant throws an exception.
        /// </summary>
        /// <exception cref="InvalidOptionVariantException"></exception>
        public static IOption<T> Flatten<T>(this IOption<IOption<T>> option)
        {
            return option switch
            {
                Some<IOption<T>> s => s.Value,
                None<IOption<T>> _ => Option.None<T>(),
                _ => throw Option.Invalid(),
            };
        }
        
        /// <summary>
        /// Returns the inner <see cref="IResult{TValue,TError}"/>. An invalid variant throws an exception.
        /// </summary>
        /// <exception cref="InvalidResultVariantException"></exception>
        public static IResult<T,E> Flatten<T,E>(this IResult<IResult<T,E>,E> result)
        {
            return result switch
            {
                Ok<IResult<T, E>,E> ok => ok.Value,
                Err<IResult<T, E>,E> err => Result.Err<T,E>(err.Error),
                _ => throw Result.Invalid(),
            };
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
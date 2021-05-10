using System;
using System.Runtime.CompilerServices;

namespace SaferVariants
{
    /// <summary>
    /// Intended as a discriminated union of either <see cref="Some{T}"/> or <see cref="None{T}"/>.
    /// </summary>
    public interface IOption<T>
    {
        /// <summary>
        /// If the IOption is <see cref="Some{T}"/>, the transform is applied to the inner value and returned as a new IOption.
        /// </summary>
        /// <param name="transform">The transformation to apply to the value.</param>
        IOption<TResult> Map<TResult>(Func<T, IOption<TResult>> transform);
        /// <summary>
        /// If the IOption is <see cref="Some{T}"/>, the continuation is applied to the inner value.
        /// </summary>
        void Then(Action<T> action);
        /// <summary>
        /// Returns the inner value if the IOption is <see cref="Some{T}"/>, returns the specified <paramref name="elseValue"/> otherwise.
        /// </summary>
        T ValueOr(T elseValue);
        /// <summary>
        /// Returns true and binds the inner value to the out variable <paramref name="value"/> if the IOption is <see cref="Some{T}"/>, returns false otherwise.
        /// </summary>
        bool IsSome(out T value);
        /// <summary>
        /// Returns true if the IOption is <see cref="Some{T}"/>, returns false otherwise.
        /// </summary>
        bool IsSome();
    }

    internal static class Option<T>
    {
        internal static readonly IOption<T> None = new None<T>();
    }

    public static class Option
    {
        public static IOption<T> Some<T>(T value)
        {
            return new Some<T>(value);
        }

        public static IOption<T> None<T>()
        {
            return Option<T>.None;
        }

        /// <summary>
        /// Returns <see cref="SaferVariants.None{T}"/> if the value is null, returns the value wrapped in <see cref="SaferVariants.Some{T}"/> otherwise.
        /// </summary>
        public static IOption<T> NoneIfNull<T>(T value)
        {
            return value != null
                ? new Some<T>(value)
                : Option<T>.None;
        }

        public static InvalidOptionVariantException Invalid([CallerMemberName] string method = "",
            [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0) =>
            new InvalidOptionVariantException(method, filePath, lineNumber);

        /// <summary>
        /// Ensures that the <see cref="IOption{T}"/> is a valid option variant, throws an exception otherwise.
        /// </summary>
        /// <exception cref="InvalidOptionVariantException"></exception>
        public static void EnsureValid<T>(IOption<T> value, [CallerMemberName] string method = "",
            [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            switch (value)
            {
                case Some<T> _:
                case None<T> _:
                    break;
                default:
                    throw Invalid(method, filePath, lineNumber);
            }
        }
    }

    public readonly struct Some<T> : IOption<T>
    {
        public Some(T value)
        {
            Value = value;
        }

        public T Value { get; }

        public IOption<TResult> Map<TResult>(Func<T, IOption<TResult>> transform)
        {
            return transform(Value);
        }

        public bool IsSome(out T value)
        {
            value = Value;
            return true;
        }

        public void Then(Action<T> action)
        {
            action(Value);
        }

        public T ValueOr(T elseValue)
        {
            return Value;
        }

        public bool IsSome()
        {
            return true;
        }
    }

    public struct None<T> : IOption<T>
    {
        public IOption<TResult> Map<TResult>(Func<T, IOption<TResult>> transform)
        {
            return Option.None<TResult>();
        }
        
        public bool IsSome(out T value)
        {
            value = default;
            return false;
        }
        
        public void Then(Action<T> action)
        {
        }

        public T ValueOr(T elseValue)
        {
            return elseValue;
        }

        public bool IsSome()
        {
            return false;
        }
    }
}
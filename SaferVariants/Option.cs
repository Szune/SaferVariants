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
        /// If the IOption is <see cref="Some{T}"/>, the transform is applied to the inner value and the new value is returned, returns the specified <paramref name="elseValue"/> otherwise.
        /// </summary>
        /// <param name="elseValue">The value to return if the IOption is of type <see cref="None{T}"/>.</param>
        /// <param name="transform">The transformation to apply to the value.</param>
        TResult MapOr<TResult>(TResult elseValue, Func<T, TResult> transform);

        /// <summary>
        /// Performs an action if the IOption is <see cref="None{T}"/>.
        /// </summary>
        /// <param name="noneHandler">The action to perform.</param>
        /// <exception cref="ArgumentNullException"></exception>
        IOption<T> HandleNone(Action noneHandler);
        
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

    /// <summary>
    /// A helper class to instantiate <see cref="IOption{T}"/> variants.
    /// </summary>
    public static class Option
    {
        /// <summary>
        /// Returns the specified value wrapped in the <see cref="Some{T}"/> variant.
        /// </summary>
        public static IOption<T> Some<T>(T value)
        {
            return new Some<T>(value);
        }

        /// <summary>
        /// Returns the <see cref="None{T}"/> variant for the specified type.
        /// </summary>
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

        /// <summary>
        /// Returns an <see cref="InvalidOptionVariantException"/> with information about the call site.
        /// </summary>
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

    ///<inheritdoc cref="IOption{T}"/>
    public readonly struct Some<T> : IOption<T>
    {
        public Some(T value)
        {
            Value = value;
        }

        public T Value { get; }

        public IOption<TResult> Map<TResult>(Func<T, IOption<TResult>> transform)
        {
            if (transform == null)
                throw new ArgumentNullException(nameof(transform));
            return transform(Value);
        }

        public TResult MapOr<TResult>(TResult elseValue, Func<T, TResult> transform)
        {
            if (transform == null)
                throw new ArgumentNullException(nameof(transform));
            return transform(Value);
        }

        public IOption<T> HandleNone(Action noneHandler)
        {
            if (noneHandler == null)
                throw new ArgumentNullException(nameof(noneHandler));
            return this;
        }

        public bool IsSome(out T value)
        {
            value = Value;
            return true;
        }

        public void Then(Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
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

    ///<inheritdoc cref="IOption{T}"/>
    public struct None<T> : IOption<T>
    {
        public IOption<TResult> Map<TResult>(Func<T, IOption<TResult>> transform)
        {
            if (transform == null)
                throw new ArgumentNullException(nameof(transform));
            return Option.None<TResult>();
        }

        public TResult MapOr<TResult>(TResult elseValue, Func<T, TResult> transform)
        {
            if (transform == null)
                throw new ArgumentNullException(nameof(transform));
            return elseValue;
        }
        
        public IOption<T> HandleNone(Action noneHandler)
        {
            if (noneHandler == null)
                throw new ArgumentNullException(nameof(noneHandler));
            noneHandler();
            return this;
        }

        public bool IsSome(out T value)
        {
            value = default;
            return false;
        }
        
        public void Then(Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
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
using System;
using System.Runtime.CompilerServices;

namespace SaferVariants
{
    public interface IOption<T>
    {
        IOption<TResult> Map<TResult>(Func<T, IOption<TResult>> transform);
        void Then(Action<T> action);
        T ValueOr(T elseValue);
        bool IsSome(out T value);
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
        /// Returns <see cref="SaferVariants.None{T}"/> if the value is null, returns <see cref="SaferVariants.Some{T}"/> otherwise 
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
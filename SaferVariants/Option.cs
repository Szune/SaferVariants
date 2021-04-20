using System;

namespace SaferVariants
{
    public interface IOption<T>
    {
        IOption<TResult> Map<TResult>(Func<T, IOption<TResult>> transform);
        T ValueOr(T elseValue);
        bool IsSome();
    }
    
    public static class Option<T>
    {
        internal static readonly None<T> None = new None<T>();
    }

    public static class Option
    {
        public static Some<T> Some<T>(T value)
        {
            return new Some<T>(value);
        }

        public static None<T> None<T>()
        {
            return Option<T>.None;
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
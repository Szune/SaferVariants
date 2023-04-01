using System.Diagnostics.CodeAnalysis;
using System;

namespace SaferVariants
{
    public readonly struct Option
    {
        public static Option None { get; } = new Option();
        public static Option<T> Some<T>(T value) => Option<T>.Some(value);
        public static Option<T> NoneIfNull<T>(T value) => value != null ? new Option<T>(value) : Option<T>.None;
    }

    public readonly struct Option<T>
    {
        internal readonly T _value;
        public bool HasValue { get; }

        public static readonly Option<T> None = new Option<T>();
        public static Option<T> Some(T value) => new Option<T>(value);

        internal Option(T value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
            HasValue = true;
        }

        public static implicit operator Option<T>(Option _) => None;

        public static implicit operator Option<T>(T value)
        {
            return value == null ? None : Some(value);
        }


        public T ValueOr(T elseValue = default(T))
        {
            return HasValue ? _value : elseValue;
        }

        public T ValueOr(Func<T> elseValue)
        {
            if (elseValue == null)
                throw new ArgumentNullException(nameof(elseValue));
            return HasValue ? _value : elseValue();
        }

        public T ValueOrThrow()
        {
            return HasValue
                ? _value
                : throw new ValueNotPresentException();
        }

        public bool TryGetValue([NotNullWhen(true)] out T value)
        {
            value = _value;
            return HasValue;
        }

        public Option<TResult> Bind<TResult>(Func<T, Option<TResult>> transform)
        {
            if (transform == null)
                throw new ArgumentNullException(nameof(transform));
            return HasValue ? transform(_value) : Option<TResult>.None;
        }

        public Option<TResult> Map<TResult>(Func<T, TResult> transform)
        {
            if (transform == null)
                throw new ArgumentNullException(nameof(transform));
            return HasValue
                ? new Option<TResult>(transform(_value))
                : Option<TResult>.None;
        }

        public TResult MapOr<TResult>(TResult elseValue, Func<T, TResult> transform)
        {
            if (transform == null)
                throw new ArgumentNullException(nameof(transform));
            return HasValue ? transform(_value) : elseValue;
        }

        public TResult MapOr<TResult>(Func<TResult> elseValue, Func<T, TResult> transform)
        {
            if (transform == null)
                throw new ArgumentNullException(nameof(transform));
            if (elseValue == null)
                throw new ArgumentNullException(nameof(elseValue));
            return HasValue
                ? transform(_value)
                : elseValue();
        }

        public void Match(Action<T> ifSome, Action ifNone)
        {
            if (ifSome == null)
                throw new ArgumentNullException(nameof(ifSome));
            if (ifNone == null)
                throw new ArgumentNullException(nameof(ifNone));

            if (HasValue)
            {
                ifSome(_value);
            }
            else
            {
                ifNone();
            }
        }

        public TResult Match<TResult>(Func<T, TResult> ifSome, Func<TResult> ifNone)
        {
            if (ifSome == null)
                throw new ArgumentNullException(nameof(ifSome));
            if (ifNone == null)
                throw new ArgumentNullException(nameof(ifNone));

            return HasValue
                ? ifSome(_value)
                : ifNone();
        }

        public Option<T> IfNone(Action noneHandler)
        {
            if (noneHandler == null)
                throw new ArgumentNullException(nameof(noneHandler));
            if (!HasValue)
                noneHandler();

            return this;
        }

        public Option<T> IfSome(Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (HasValue)
                action(_value);
            return this;
        }
    }
}

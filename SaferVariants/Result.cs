using System.Diagnostics.CodeAnalysis;
using System;

namespace SaferVariants
{
    public readonly struct Result
    {
        public static Result<Unit, TError> EmptyOk<TError>() => new Result<Unit, TError>(Unit.It);
        public static Result<TValue, Unit> EmptyError<TValue>() => new Result<TValue, Unit>(Unit.It);
        public static Result<TValue, TError> Ok<TValue, TError>(TValue value) => Result<TValue, TError>.Ok(value);
        public static Result<TValue, TError> Error<TValue, TError>(TError error) => Result<TValue, TError>.Error(error);
    }

    public readonly struct Result<TValue, TError>
    {
        internal readonly TValue _value;
        internal readonly TError _error;
        private readonly bool _isOk;
        public bool IsOk => _isOk;
        public bool IsError => !_isOk;

        public static Result<TValue, TError> Ok(TValue value) => new Result<TValue, TError>(value);
        public static Result<TValue, TError> Error(TError error) => new Result<TValue, TError>(error);

        internal Result(TValue value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
            _error = default!;
            _isOk = true;
        }

        internal Result(TError error)
        {
            _value = default!;
            _error = error ?? throw new ArgumentNullException(nameof(error));
            _isOk = false;
        }

        public static implicit operator Result<TValue, TError>(TValue value)
        {
            return new Result<TValue, TError>(value);
        }

        public static implicit operator Result<TValue, TError>(TError error)
        {
            return new Result<TValue, TError>(error);
        }

        public TValue ValueOrThrow()
        {
            return IsOk ? _value : throw new SuccessValueNotPresentException();
        }

        public TValue ValueOr(TValue elseValue = default(TValue))
        {
            return IsOk ? _value : elseValue;
        }

        public TValue ValueOr(Func<TValue> elseValue)
        {
            if (elseValue == null)
                throw new ArgumentNullException(nameof(elseValue));
            return IsOk ? _value : elseValue();
        }

        public bool TryGetValue([NotNullWhen(true)] out TValue value)
        {
            value = _value;
            return IsOk;
        }


        /// <exception cref="ValueNullException">The error value was null. This can happen if creating a <see cref="Result"/> with the "default" keyword.</exception>
        /// <exception cref="ErrorValueNotPresentException">The <see cref="Result"/> was an Ok result.</exception>
        public TError ErrorOrThrow()
        {
            return !IsOk
                ? _error ?? throw new ValueNullException()
                : throw new ErrorValueNotPresentException();
        }

        /// <exception cref="ValueNullException">The error value was null. This can happen if creating a <see cref="Result"/> with the "default" keyword.</exception>
        public bool TryGetError([NotNullWhen(true)] out TError error)
        {
            error = _error;
            if (IsOk)
            {
                return false;
            }

            if (error == null) throw new ValueNullException();
            return true;
        }

        public void Match(Action<TValue> ifOk, Action<TError> ifError)
        {
            if (ifOk == null)
                throw new ArgumentNullException(nameof(ifOk));
            if (ifError == null)
                throw new ArgumentNullException(nameof(ifError));

            if (IsOk)
            {
                ifOk(_value);
            }
            else
            {
                ifError(_error);
            }
        }

        public TResult Match<TResult>(Func<TValue, TResult> ifOk, Func<TError, TResult> ifError)
        {
            if (ifOk == null)
                throw new ArgumentNullException(nameof(ifOk));
            if (ifError == null)
                throw new ArgumentNullException(nameof(ifError));

            return IsOk
                ? ifOk(_value)
                : ifError(_error);
        }

        public Result<TResultValue, TError> Bind<TResultValue>(
            Func<TValue, Result<TResultValue, TError>> bind)
        {
            if (bind == null)
                throw new ArgumentNullException(nameof(bind));

            return IsOk
                ? bind(_value)
                : _error;
        }

        public Result<TResultValue, TResultError> Map<TResultValue, TResultError>(
            Func<TValue, TResultValue> mapOk, Func<TError, TResultError> mapError)
        {
            if (mapOk == null)
                throw new ArgumentNullException(nameof(mapOk));
            if (mapError == null)
                throw new ArgumentNullException(nameof(mapError));

            return IsOk
                ? new Result<TResultValue, TResultError>(mapOk(_value))
                : new Result<TResultValue, TResultError>(mapError(_error));
        }

        public Result<TResult, TError> Map<TResult>(Func<TValue, TResult> transform)
        {
            if (transform == null)
                throw new ArgumentNullException(nameof(transform));

            return IsOk
                ? new Result<TResult, TError>(transform(_value))
                : new Result<TResult, TError>(_error);
        }

        public Result<TValue, TResult> MapErr<TResult>(Func<TError, TResult> transform)
        {
            if (transform == null)
                throw new ArgumentNullException(nameof(transform));

            return IsOk
                ? Result<TValue, TResult>.Ok(_value)
                : Result<TValue, TResult>.Error(transform(_error));
        }

        public TResult MapOr<TResult>(TResult elseValue, Func<TValue, TResult> transform)
        {
            if (transform == null)
                throw new ArgumentNullException(nameof(transform));

            return IsOk
                ? transform(_value)
                : elseValue;
        }

        public TResult MapOr<TResult>(Func<TResult> elseValue, Func<TValue, TResult> transform)
        {
            if (transform == null)
                throw new ArgumentNullException(nameof(transform));
            if (elseValue == null)
                throw new ArgumentNullException(nameof(elseValue));

            return IsOk
                ? transform(_value)
                : elseValue();
        }

        public Result<TValue, TError> IfOk(Action<TValue> okHandler)
        {
            if (okHandler == null) throw new ArgumentNullException(nameof(okHandler));
            if (IsOk) okHandler(_value);
            return this;
        }

        public Result<TValue, TError> IfError(Action<TError> errorHandler)
        {
            if (errorHandler == null) throw new ArgumentNullException(nameof(errorHandler));
            if (!IsOk) errorHandler(_error);
            return this;
        }

        public Option<TValue> HandleError(Action<TError> errorHandler)
        {
            if (errorHandler == null) throw new ArgumentNullException(nameof(errorHandler));
            if (IsOk) return new Option<TValue>(_value);

            errorHandler(_error);
            return Option<TValue>.None;
        }

        public Option<TError> HandleOk(Action<TValue> okHandler)
        {
            if (okHandler == null) throw new ArgumentNullException(nameof(okHandler));
            if (IsError) return new Option<TError>(_error);

            okHandler(_value);
            return Option<TError>.None;
        }
    }
}

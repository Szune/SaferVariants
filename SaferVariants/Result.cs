using System;
using System.Runtime.CompilerServices;

namespace SaferVariants
{
    /// <summary>
    /// Intended as a discriminated union of either <see cref="Ok{TValue,TError}"/> or <see cref="Err{TValue,TError}"/>.
    /// </summary>
    public interface IResult<TValue, TError>
    {
        /// <summary>
        /// If the IResult is <see cref="Ok{TValue,TError}"/>, the transform is applied to the inner value and returned as a new IResult.
        /// </summary>
        /// <param name="transform">The transformation to apply to the value.</param>
        IResult<TResult, TError> Map<TResult>(Func<TValue, IResult<TResult, TError>> transform);
        /// <summary>
        /// If the IResult is <see cref="Ok{TValue,TError}"/>, the continuation is applied to the inner value.
        /// </summary>
        void Then(Action<TValue> action);
        /// <summary>
        /// Returns the inner value if the IResult is <see cref="Ok{TValue,TError}"/>, returns the specified <paramref name="elseValue"/> otherwise.
        /// </summary>
        TValue ValueOr(TValue elseValue);
        /// <summary>
        /// Returns true and binds the inner value to the out variable <paramref name="value"/> if the IResult is <see cref="Ok{TValue,TError}"/>, returns false otherwise.
        /// </summary>
        bool IsOk(out TValue value);
        /// <summary>
        /// Returns true if the IResult is <see cref="Ok{TValue,TError}"/>, returns false otherwise.
        /// </summary>
        bool IsOk();
        /// <summary>
        /// Returns true and binds the inner value to the out variable <paramref name="error"/> if the IResult is <see cref="Err{TValue,TError}"/>, returns false otherwise.
        /// </summary>
        bool IsErr(out TError error);
        /// <summary>
        /// Returns true if the IResult is <see cref="Err{TValue,TError}"/>, returns false otherwise.
        /// </summary>
        bool IsErr();
    }

    public static class Result
    {
        public static IResult<TValue, TError> Ok<TValue, TError>(TValue value)
        {
            return new Ok<TValue, TError>(value);
        }

        public static IResult<TValue, TError> Err<TValue, TError>(TError error)
        {
            return new Err<TValue, TError>(error);
        }

        public static InvalidResultVariantException Invalid([CallerMemberName] string method = "",
            [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0) =>
            new InvalidResultVariantException(method, filePath, lineNumber);
        
        /// <summary>
        /// Ensures that the <see cref="IResult{TValue,TError}"/> is a valid result variant, throws an exception otherwise.
        /// </summary>
        /// <exception cref="InvalidResultVariantException"></exception>
        public static void EnsureValid<TValue,TError>(IResult<TValue,TError> value, [CallerMemberName] string method = "",
            [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            switch (value)
            {
                case Ok<TValue,TError> _:
                case Err<TValue,TError> _:
                    break;
                default:
                    throw Invalid(method, filePath, lineNumber);
            }
        }
    }

    public readonly struct Ok<TValue, TError> : IResult<TValue, TError>
    {
        public Ok(TValue value)
        {
            Value = value;
        }

        public TValue Value { get; }

        public IResult<TResult, TError> Map<TResult>(Func<TValue, IResult<TResult, TError>> transform)
        {
            return transform(Value);
        }

        public void Then(Action<TValue> action)
        {
            action(Value);
        }

        public TValue ValueOr(TValue elseValue)
        {
            return Value;
        }

        public bool IsOk(out TValue value)
        {
            value = Value;
            return true;
        }

        public bool IsOk()
        {
            return true;
        }

        public bool IsErr(out TError error)
        {
            error = default;
            return false;
        }

        public bool IsErr()
        {
            return false;
        }
    }

    public readonly struct Err<TValue, TError> : IResult<TValue, TError>
    {
        public Err(TError error)
        {
            Error = error;
        }

        public TError Error { get; }

        public IResult<TResult, TError> Map<TResult>(Func<TValue, IResult<TResult, TError>> transform)
        {
            return Result.Err<TResult, TError>(Error);
        }

        public void Then(Action<TValue> action)
        {
        }

        public TValue ValueOr(TValue elseValue)
        {
            return elseValue;
        }

        public bool IsOk(out TValue value)
        {
            value = default;
            return false;
        }

        public bool IsOk()
        {
            return false;
        }

        public bool IsErr(out TError error)
        {
            error = Error;
            return true;
        }

        public bool IsErr()
        {
            return true;
        }
    }
}
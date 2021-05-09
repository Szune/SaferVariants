using System;
using System.Runtime.CompilerServices;

namespace SaferVariants
{
    public interface IResult<TValue, TError>
    {
        IResult<TResult, TError> Map<TResult>(Func<TValue, IResult<TResult, TError>> transform);
        void Then(Action<TValue> action);
        TValue ValueOr(TValue elseValue);
        bool IsOk(out TValue value);
        bool IsOk();
        bool IsErr(out TError error);
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
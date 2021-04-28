using System;

namespace SaferVariants
{
    public interface IResult<TValue, TError>
    {
        IResult<TResult, TError> Map<TResult>(Func<TValue, IResult<TResult, TError>> transform);
        TValue ValueOr(TValue elseValue);
        bool IsOk();
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

        public TValue ValueOr(TValue elseValue)
        {
            return Value;
        }

        public bool IsOk()
        {
            return true;
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

        public TValue ValueOr(TValue elseValue)
        {
            return elseValue;
        }

        public bool IsOk()
        {
            return false;
        }
    }
}
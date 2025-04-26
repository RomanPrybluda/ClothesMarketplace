using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Сommon.Wrappers
{
    public class Result<T>
    {
        public T? Value { get; }

        public CustomExceptionType Error { get; }

        public bool IsSuccess { get; }

        public bool IsFailure => !IsSuccess;

        private Result(T value)
        {
            Value = value;
            IsSuccess = true;
            Error = CustomExceptionType.None;
        }

        private Result(CustomExceptionType error)
        {
            Error = error;
            IsSuccess = false;
            Value = default;
        }

        public static Result<T> Success(T value) => new(value);

        public static Result<T> Failure(CustomExceptionType error) => new(error);
    }
}

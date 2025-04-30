using Domain.Helpers;
using Microsoft.AspNetCore.Identity;

namespace Domain.Сommon.Wrappers
{
    public class Result<T>
    {
        public T? Value { get; }

        public List<string> Errors { get; }

        public bool IsSuccess { get; }

        public bool IsFailure => !IsSuccess;

        private Result(T value)
        {
            Value = value;
            IsSuccess = true;
            Errors = new();
        }

        private Result(Exception error)
        {
            Errors = new();
            IsSuccess = false;
            Value = default;
            Errors.Add(error.Message.ToString());
        }

        private Result(List<Exception> errors)
        {
            Errors = errors.Select(e => e.Message).ToList();
            IsSuccess = false;
            Value = default;
        }

        private Result(IEnumerable<IdentityError> errors)
        {
            Errors = errors.Select(e => e.Description).ToList();
            IsSuccess = false;
            Value = default;
        }

        private Result(IdentityError error)
        {
            Errors = new() { error.Description };
            IsSuccess = false;
            Value = default;
        }

        public static Result<T> Success(T value) => new(value);

        public static Result<T> Failure(Exception error) => new(error);

        public static Result<T> Failure(List<Exception> errors) => new(errors);

        public static Result<T> Failure(IEnumerable<IdentityError> errors) => new(errors);

        public static Result<T> Failure(IdentityError error) => new(error);

        public ErrorResponse ToErrorResponse() => new ErrorResponse { Errors = this.Errors };
    }
}

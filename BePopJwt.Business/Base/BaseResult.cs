using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;


namespace BePopJwt.Business.Base
{
    public class BaseResult<T>
    {
        public T? Data { get; set; }

        public List<ErrorDetail>? Errors { get; set; }

        public bool IsSuccess => Errors == null || !Errors.Any();

        // ✅ SUCCESS
        public static BaseResult<T> Success(T data)
            => new() { Data = data };

        // ❌ STRING ERROR
        public static BaseResult<T> Fail(string message)
            => new()
            {
                Errors = new List<ErrorDetail>
                {
                new ErrorDetail
                {
                    PropertyName = "General",
                    ErrorMessage = message
                }
                }
            };

      
        public static BaseResult<T> Fail(List<ValidationFailure> errors)
            => new()
            {
                Errors = errors.Select(e => new ErrorDetail
                {
                    PropertyName = e.PropertyName,
                    ErrorMessage = e.ErrorMessage
                }).ToList()
            };

        // ❌ IDENTITY ERROR
        public static BaseResult<T> Fail(IEnumerable<IdentityError> errors)
            => new()
            {
                Errors = errors.Select(e => new ErrorDetail
                {
                    PropertyName = e.Code,
                    ErrorMessage = e.Description
                }).ToList()
            };


    }
}

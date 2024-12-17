using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public class OperationResult<T>
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public T? Data { get; set; }
        public IEnumerable<string> ValidationErrors { get; set; }
        public int StatusCode { get; set; }

        public static OperationResult<T> SuccessResult(T data, ILogger logger)
        {
            logger.LogInformation("Operation was successful");
            return new OperationResult<T>
            {
                Success = true,
                Data = data,
                StatusCode = 200 // OK status
            };
        }

        public static OperationResult<T> FailureResult(string errorMessage, ILogger logger, int statusCode = 400)
        {
            logger.LogError(errorMessage);
            return new OperationResult<T>
            {
                Success = false,
                ErrorMessage = errorMessage,
                StatusCode = statusCode
            };
        }

        public static OperationResult<T> FailureResult(IEnumerable<string> validationErrors, ILogger logger, int statusCode = 400)
        {
            foreach (var error in validationErrors)
            {
                logger.LogError(error);
            }
            return new OperationResult<T>
            {
                Success = false,
                ValidationErrors = validationErrors,
                StatusCode = statusCode
            };
        }
    }

}

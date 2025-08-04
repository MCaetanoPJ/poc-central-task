using System.Net;

namespace CentralTask.Core.RepositoryBase
{
    public class ApiResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool Success { get; set; } = true;
        public List<string>? ErrorMessages { get; set; }
        public T? Result { get; set; }

        public ApiResponse() { }

        public ApiResponse(HttpStatusCode statusCode, bool success, List<string>? errorMessages, T result)
        {
            StatusCode = statusCode;
            Success = success;
            ErrorMessages = errorMessages;
            Result = result;
        }
    }
}

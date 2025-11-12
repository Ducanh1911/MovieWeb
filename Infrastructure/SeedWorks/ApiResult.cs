using System.Text.Json.Serialization;

namespace MovieWebApp.Infrastructure.SeedWorks
{
    public class ApiResult<T>
    {
        [JsonConstructor]
        public ApiResult(bool isSucceeded, string message = null, int statusCode = 200)
        {
            Message = message;
            IsSucceeded = isSucceeded;
            StatusCode = statusCode;
        }

        public ApiResult() { }

        public ApiResult(bool isSucceeded, T data, string message = null, int statusCode = 200)
        {
            Data = data;
            Message = message;
            IsSucceeded = isSucceeded;
            StatusCode = statusCode;
        }

        public ApiResult(
            bool isSucceeded,
            string[] errors,
            T data,
            string message = null,
            int statusCode = 200
        )
        {
            Errors = errors;
            Message = message;
            Data = data;
            IsSucceeded = isSucceeded;
            StatusCode = statusCode;
        }

        public bool IsSucceeded { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public string[] Errors { get; set; }
    }
}

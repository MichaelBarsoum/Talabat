
namespace Talabat.API.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ApiResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultStatusCode(statusCode);
        }

        private string? GetDefaultStatusCode(int statusCode)
        {
            return StatusCode switch
            {
                400 => "Bad Request",
                401 => "User Unauthorized",
                403 => "User Forbidden",
                404 => "Not Found",
                405 => "Method Not Allowed",
                409 => "This User is Already Exists",
                500 => "Internal Server Error",
                502 => "Bad GateWay",
                503 => "Service Un Available",
                504 => "GateWay TimeOut"
            };
        }
    }
}

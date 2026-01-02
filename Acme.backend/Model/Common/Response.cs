namespace Model.Common
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new();

        public static Response<T> SuccessResponse(T data, string message = "Operación exitosa")
        {
            return new Response<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static Response<T> ErrorResponse(string message, List<string>? errors = null)
        {
            return new Response<T>
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>()
            };
        }
    }
}

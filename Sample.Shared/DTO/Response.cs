namespace Sample.Shared.DTO
{
    public class Response<T>
    {
        public Response()
        {
            Message = string.Empty;
        }
        public Response(T data)
        {
            Succeeded = true;
            Message = string.Empty;
            Errors = null;
            Data = data;
        }
        public T? Data { get; set; }
        public bool Succeeded { get; set; }
        public string[]? Errors { get; set; }
        public string Message { get; set; }
    }
}

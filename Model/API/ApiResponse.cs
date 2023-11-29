namespace Dictionary.Model.API
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public ErrorDetails? Error { get; set; }
    }
}

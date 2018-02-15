namespace Auth.Models
{
    public class ApiResponse
    {
        public bool Success => string.IsNullOrEmpty(Error);

        public string Error { get; set; }
    }

    public class ApiResponse<T> : ApiResponse where T : class
    {
        public T Result { get; set; }
    }
}

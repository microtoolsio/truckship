namespace Auth.Domain
{
    public class ExecutionResult
    {
        public bool Success => string.IsNullOrEmpty(Error);

        public string Error { get; set; }
    }

    public class ExecutionResult<T> : ExecutionResult where T : class
    {
        public T Result { get; set; }
    }
}

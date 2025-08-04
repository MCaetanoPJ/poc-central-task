namespace CentralTask.Core.RepositoryBase
{
    public class ValidateResult
    {
        public List<string> Messages { get; set; } = new List<string>();
        public bool Sucess => !Messages.Any();

        public void AddMessage(string message) => Messages.Add(message);
    }

    public class ValidateResult<T> : ValidateResult
    {
        public T? Data { get; set; }
    }
}
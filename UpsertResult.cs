namespace Cosential.Integrations.Compass.Client
{
    public enum UpsertAction
    {
        Created,
        Updated,
        None
    }

    public class UpsertResult<T>
    {
        public UpsertAction Action { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
    }
}
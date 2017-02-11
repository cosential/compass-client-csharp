namespace Cosential.Integrations.CompassApiClient
{
    public enum UpsertAction
    {
        Created,
        Updated
    }

    public class UpsertResult<T>
    {
        public UpsertAction Action { get; set; }
        public T Data { get; set; }
    }
}
namespace Cosential.Integrations.Compass.Client.Models.Interfaces
{
    public interface IValueList{
        string PrimaryKeyName { get; }
        object PrimaryKey { get; set; }
        bool PrimaryIsDeleted { get; }
    }
}
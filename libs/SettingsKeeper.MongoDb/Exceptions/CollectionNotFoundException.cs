using System.Runtime.Serialization;

namespace SettingsKeeper.MongoDb.Exceptions;

[Serializable]
public class CollectionNotFoundException : Exception
{
    private const string _message = "Не удалось найти указанную коллекцию";

    public CollectionNotFoundException() : base(_message)
    {
    }

    public CollectionNotFoundException(SerializationInfo info, StreamingContext context):base(info, context)
    {
    }
}
using System.Runtime.Serialization;

namespace SettingsKeeper.MongoDb.Exceptions;

[Serializable]
public class DbNotFoundException : Exception
{
    private const string _message = "Не удалось найти указанную базу данных";

    public DbNotFoundException() : base(_message)
    {
    }

    public DbNotFoundException(SerializationInfo info, StreamingContext context):base(info, context)
    {
    }
}
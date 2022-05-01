using System.Runtime.Serialization;

namespace SettingsKeeper.MongoDb.Exceptions;

[Serializable]
public class NameContainsException: Exception
{
    private const string _message = "Елемент с данным именем уже существует";
    public NameContainsException(): base(_message)
    {
        
    }

    public NameContainsException(SerializationInfo info, StreamingContext context):base(info, context)
    {
    }
}
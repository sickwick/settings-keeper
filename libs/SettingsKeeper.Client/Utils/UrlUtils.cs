namespace SettingsKeeper.Client.Utils;

public class UrlUtils
{
    public static string BuildPath(params string[] paths)
    {
        return $"{string.Join("/",paths.Select(p=>p.Trim('/')))}";
    }
}
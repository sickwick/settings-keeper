namespace SettingsKeeper.Core.Abstract;

public interface ISettingsService
{
    Task<string> GetSettingsAsync(string name, CancellationToken cancellationToken);
}
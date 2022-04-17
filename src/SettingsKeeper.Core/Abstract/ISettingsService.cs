namespace SettingsKeeper.Core.Abstract;

public interface ISettingsService
{
    Task<string> GetSettingsAsync(string name, CancellationToken cancellationToken);

    Task AddSettingsAsync(string name, string settings, CancellationToken cancellationToken);
}
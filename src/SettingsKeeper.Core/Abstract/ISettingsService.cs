namespace SettingsKeeper.Core.Abstract;

public interface ISettingsService
{
    Task<string> GetSettingsAsync(string name, CancellationToken cancellationToken);

    Task AddSettingsAsync(string name, string settings, CancellationToken cancellationToken);

    Task ChangeSettingsAsync(string name, string settings, CancellationToken cancellationToken);

    Task RemoveSettingsAsync(string name, CancellationToken cancellationToken);
}
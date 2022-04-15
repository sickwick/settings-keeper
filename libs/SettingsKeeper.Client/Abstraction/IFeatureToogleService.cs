namespace SettingsKeeper.Client.Abstraction;

public interface IFeatureToogleService
{
    Task<bool> IsFeatureToogleEnabled(string name, CancellationToken cancellationToken);
}
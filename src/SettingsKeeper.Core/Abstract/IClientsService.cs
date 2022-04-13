namespace SettingsKeeper.Core.Abstract;

public interface IClientsService
{
    Task<IEnumerable<string>> GetAllClients();

    Task AddNewClient(string name);
    
    void SendMessageToClient<T>(string name, T message) where T: class;

    Task SendMessageToAllClients<T>(T message) where T: class;
}
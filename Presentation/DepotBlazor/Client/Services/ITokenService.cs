namespace DepotBlazor.Client.Services
{
	public interface ITokenService
	{
		Task<bool> IsTokenValid();
		Task<int> GetUserId();
		Task<string> GetUserName();
		Task Clear();
		Task Set(string token);
		Task<string> Get();
    }
}

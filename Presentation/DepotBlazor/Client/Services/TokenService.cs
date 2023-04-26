using Blazored.LocalStorage;
using Domain.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DepotBlazor.Client.Services
{
	public class TokenService : ITokenService
	{
		ILocalStorageService localStorage { get; set; }
		const string tokenName = "token";
        public TokenService(ILocalStorageService _localStorage)
        {
            localStorage = _localStorage;
        }

		public async Task Clear() {
			await localStorage.SetItemAsync(tokenName, string.Empty);
		}
        public async Task<string> Get()
        {
            return await localStorage.GetItemAsync<string>(tokenName);
        }

        public async Task Set(string token)
        {
            await localStorage.SetItemAsync(tokenName, token);
        }
        public async Task<bool> IsTokenValid()
		{
			var token = await localStorage.GetItemAsync<string>(tokenName);
			if (!string.IsNullOrEmpty(token))
			{
				try
				{
					var date = new JwtSecurityTokenHandler().ReadJwtToken(token);
					var expiration = date.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Expiration).Value;
					var now = DateTime.Now;
					var exp = Convert.ToDateTime(expiration);
					if (exp > now)
						return true;
					else
						return false;
				}
				catch (Exception)
				{
					return false;
				}
			}
			return false;
		}

		public async Task<int> GetUserId()
		{
			var token = await localStorage.GetItemAsync<string>(tokenName);
			if (!string.IsNullOrEmpty(token))
			{
				try
				{
					var context = new JwtSecurityTokenHandler().ReadJwtToken(token);
					var id = context.Claims.FirstOrDefault(x => x.Type == SettingsConfig.Id).Value;
					int userId = 0;
					if (int.TryParse(id, out userId))
						return userId;
					else
						return 0;
				}
				catch (Exception)
				{
					return 0;
				}
			}
			return 0;
		}
		public async Task<string> GetUserName()
		{
			var token = await localStorage.GetItemAsync<string>(tokenName);
			if (!string.IsNullOrEmpty(token))
			{
				try
				{
					var context = new JwtSecurityTokenHandler().ReadJwtToken(token);
					var name = context.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
					if(!string.IsNullOrEmpty(name))
						return name;
					else
						return string.Empty;
				}
				catch (Exception)
				{
					return string.Empty;
				}
			}
			return string.Empty;
		}
	}
}

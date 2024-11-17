using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.Interfaces;

namespace WebApiVRoom.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetAccessTokenAsync(string userId)
        {
            var response = await _httpClient.GetAsync($"https://oauth2provider.com/token?userId={userId}");
            var token = await response.Content.ReadAsStringAsync();
            return token;
        }
    }
}

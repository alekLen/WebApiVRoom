using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiVRoom.DAL.Entities;
using WebApiVRoom.DAL.EF;
using WebApiVRoom.DAL.Interfaces;
using System.Runtime.InteropServices;
using System.Text.Json;


namespace WebApiVRoom.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private VRoomContext db;

        public UserRepository(VRoomContext context)
        {
            this.db = context;
        }     
      
        public async Task<User> Get(int id)
        {
            return await db.Users.FirstOrDefaultAsync(m => m.Id == id);
        }
      
    }


    //public class UserRepository : IUserRepository
    //{
    //    private readonly HttpClient _httpClient;
    //    private const string ClerkApiBaseUrl = "https://api.clerk.dev/v1";
    //    private const string ApiKey = "Ваш_Ключ_API"; // Замените на ваш реальный ключ API Clerk

    //    public UserRepository(HttpClient httpClient)
    //    {
    //        _httpClient = httpClient;
    //        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiKey}");
    //    }

    //    // Получение пользователя по ID
    //    public async Task<String> GetUserByIdAsync(string userId)
    //    {
    //        var response = await _httpClient.GetAsync($"{ClerkApiBaseUrl}/users/{userId}");
    //        response.EnsureSuccessStatusCode();
    //        return await response.Content.ReadAsStringAsync();

    //    }
    //    public async Task<User> GetUserByIdAsync2(string userId)
    //    {
    //        var response = await _httpClient.GetAsync($"{ClerkApiBaseUrl}/users/{userId}");
    //        response.EnsureSuccessStatusCode();

    //        var user = JsonSerializer.Deserialize<User>(await response.Content.ReadAsStringAsync());
    //        return user;
    //    }

    //    // Получение всех пользователей
    //    public async Task<string> GetAllUsersAsync()
    //    {
    //        var response = await _httpClient.GetAsync($"{ClerkApiBaseUrl}/users");
    //        response.EnsureSuccessStatusCode();
    //        return await response.Content.ReadAsStringAsync();
    //    }

    //    // Создание нового пользователя
    //    public async Task<string> CreateUserAsync(string email, string password)
    //    {
    //        var newUser = new
    //        {
    //            email_address = email,
    //            password = password
    //        };

    //        var content = new StringContent(JsonSerializer.Serialize(newUser), Encoding.UTF8, "application/json");
    //        var response = await _httpClient.PostAsync($"{ClerkApiBaseUrl}/users", content);
    //        response.EnsureSuccessStatusCode();
    //        return await response.Content.ReadAsStringAsync();
    //    }

    //    // Обновление данных пользователя
    //    public async Task<string> UpdateUserAsync(string userId, object updateData)
    //    {
    //        var content = new StringContent(JsonSerializer.Serialize(updateData), Encoding.UTF8, "application/json");
    //        var response = await _httpClient.PatchAsync($"{ClerkApiBaseUrl}/users/{userId}", content);
    //        response.EnsureSuccessStatusCode();
    //        return await response.Content.ReadAsStringAsync();
    //    }

    //    // Удаление пользователя
    //    public async Task DeleteUserAsync(string userId)
    //    {
    //        var response = await _httpClient.DeleteAsync($"{ClerkApiBaseUrl}/users/{userId}");
    //        response.EnsureSuccessStatusCode();
    //    }
    //}

}

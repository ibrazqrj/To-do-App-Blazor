using System.Net.Http.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Syncfusion.Blazor.Inputs;
using System.Collections.Generic;
using Microsoft.Graph.Models;
using System.Linq.Expressions;


namespace BlazorToDoApp.Services
{
    public class HttpTodoService : ITodoService
    {
        private readonly HttpClient _httpClient;
        private readonly IAccessTokenProvider _tokenProvider;

        public HttpTodoService(HttpClient httpClient, IAccessTokenProvider tokenProvider)
        {
            _httpClient = httpClient;
            _tokenProvider = tokenProvider;
        }

        public async Task<List<TodoItem>?> GetTodosAsync(int offset = 0, int limit = 10, bool? isCompleted = null)
        {
            try
            {
                var completed = isCompleted != null ? $"&completed={isCompleted}" : "";
                var request = new HttpRequestMessage(HttpMethod.Get, $"todo?offset={offset}&limit={limit}{completed}");
                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<TodoItem>>();
                }
                else
                {
                    throw new HttpRequestException($"API call failed: {response.StatusCode}");
                }
            }
            catch (TaskCanceledException)
            {
                throw new HttpRequestException("Timed out.");
            }
        }

        public async Task<int?> GetTodoCount(bool completed = false)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"todo/count?completed={completed}");
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<int>();
            }

            return null;
        }

        public async Task<bool> CreateTodoAsync(TodoItem newTodo)
        {
            try
            {
                var tokenResult = await _tokenProvider.RequestAccessToken();
                if (!tokenResult.TryGetToken(out var token))
                {
                    return false;
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
                var response = await _httpClient.PostAsJsonAsync("http://localhost:5085/api/Todo", newTodo);

                return response.IsSuccessStatusCode;
            }
            catch (TaskCanceledException)
            {
                throw new HttpRequestException("Timed out.");
            }
        }

        public async Task<TodoItem?> GetTodoByIdAsync(int id)
        {
            try
            {
                var tokenResult = await _tokenProvider.RequestAccessToken();
                if (!tokenResult.TryGetToken(out var token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token?.Value);
                }

                var response = await _httpClient.GetAsync($"http://localhost:5085/api/Todo/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<TodoItem>();
                }
                return null;
            }
            catch (TaskCanceledException)
            {
                throw new HttpRequestException("Timed out.");
            }
        }

        public async Task<bool> UpdateTodoAsync(TodoItem updatedTodo)
        {
            var tokenResult = await _tokenProvider.RequestAccessToken();
            if (tokenResult.TryGetToken(out var token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
            }

            var response = await _httpClient.PutAsJsonAsync($"http://localhost:5085/api/Todo/{updatedTodo.Id}", updatedTodo);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteTodoAsync(int id)
        {
            var tokenResult = await _tokenProvider.RequestAccessToken();
            if (tokenResult.TryGetToken(out var token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
            }

            var response = await _httpClient.DeleteAsync($"http://localhost:5085/api/Todo/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAllBinTodosAsync(bool? isCompleted = true)
        {
            var tokenResult = await _tokenProvider.RequestAccessToken();
            if (tokenResult.TryGetToken(out var token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
            }

            var completed = isCompleted != null ? $"completed={isCompleted.ToString().ToLower()}" : "";
            var response = await _httpClient.DeleteAsync($"http://localhost:5085/api/Todo?{completed}");

            return response.IsSuccessStatusCode;
        }
    }
}

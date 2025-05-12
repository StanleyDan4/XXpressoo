using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xpressoo.Models;
using XPressoo.Models;
using XPressoo.Models.Dtos;
using XXpressoo.Models.Dtos;



namespace XXpressoo.Services
{
    public class ApiService
    {
        public readonly HttpClient client;
        private const string BaseUrl = "http://10.0.2.2:5000/api/"; // Замените на IP вашего сервера

        public ApiService()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                {
                    // Только для тестирования! В продакшене используйте доверенные сертификаты
                    return true;
                }
            };

            client = new HttpClient(handler)
            {
                BaseAddress = new Uri(BaseUrl)
            };

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task AddToCartAsync(int userId, int productId)
        {
            if (userId <= 0 || productId <= 0)
            {
                throw new Exception("Некорректные данные пользователя или продукта");
            }

            var dto = new
            {
                UserId = userId,
                ProductId = productId,
                Quantity = 1
            };

            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("baskets", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorText = await response.Content.ReadAsStringAsync();
                throw new Exception($"Ошибка добавления в корзину: {errorText}");
            }
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            var response = await client.GetAsync("products");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Ошибка загрузки продуктов: {error}");
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Product>>(json);
        }
        public async Task<List<BasketItem>> GetCartAsync(int userId)
        {
            if (userId <= 0)
                throw new Exception("Пользователь не авторизован");

            var response = await client.GetAsync($"baskets?userId={userId}");

            if (!response.IsSuccessStatusCode)
            {
                var errorText = await response.Content.ReadAsStringAsync();
                throw new Exception($"Ошибка загрузки корзины: {errorText}");
            }

            var json = await response.Content.ReadAsStringAsync();

            try
            {
                return JsonConvert.DeserializeObject<List<BasketItem>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка десериализации корзины: {ex.Message}");
            }
        }

        public async Task UpdateCartItemAsync(int userId, int productId, int quantity)
        {
            var dto = new
            {
                UserId = userId,
                ProductId = productId,
                Quantity = quantity
            };

            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            var response = await client.PutAsync("api/baskets", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Ошибка обновления корзины: {error}");
            }
        }
        public async Task<User> LoginUserAsync(string email, string password)
        {
            var dto = new
            {
                email,
                password
            };

            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("users/login", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorText = await response.Content.ReadAsStringAsync();
                throw new Exception($"Ошибка входа: {errorText}");
            }

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<User>(result);
        }
        public async Task RemoveFromCartAsync(int userId, int productId)
        {
            var dto = new
            {
                UserId = userId,
                ProductId = productId
            };

            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("baskets/remove", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Ошибка удаления из корзины: {error}");
            }
        }
        public async Task CheckoutAsync(int userId, List<BasketItem> items)
        {
            var dto = new
            {
                UserId = userId,
                Items = items.Select(i => new
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList()
            };

            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("orders", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Ошибка оформления заказа: {error}");
            }
        }


        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            try
            {
                var url = $"users/exists?email={Uri.EscapeDataString(email)}";
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorText = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Ошибка проверки email (HTTP {response.StatusCode}): {errorText}");
                    throw new Exception("Ошибка сервера при проверке email");
                }

                var json = await response.Content.ReadAsStringAsync();

                // Десериализуем как объект
                var result = JsonConvert.DeserializeObject<EmailExistResponse>(json);
                return result?.Success == true && result.Exists;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Исключение при проверке email: {ex.Message}");
                throw;
            }
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            var response = await client.GetAsync($"users/email?email={Uri.EscapeDataString(email)}");
            if (!response.IsSuccessStatusCode)
            {
                var errorText = await response.Content.ReadAsStringAsync();
                throw new Exception($"Ошибка загрузки пользователя: {errorText}");
            }
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<User>(json);
        }
        public async Task<User> GetUserAsync(int userId)
        {
            var response = await client.GetAsync($"users/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                var errorText = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[ERROR] Запрос к users/{userId} завершился с кодом {response.StatusCode}: {errorText}");
                throw new Exception($"Ошибка загрузки пользователя: {errorText}");
            }

            var json = await response.Content.ReadAsStringAsync();

            try
            {
                var user = JsonConvert.DeserializeObject<User>(json);
                if (user == null || user.UserId != userId)
                    throw new Exception($"Пользователь с ID {userId} не найден.");
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Ошибка десериализации: {ex.Message}");
                throw new Exception($"Ошибка десериализации: {ex.Message}");
            }
        }
        public async Task SubmitReviewAsync(ReviewDto dto)
        {
            if (dto.UserId <= 0 || dto.ProductId <= 0 || dto.Rating < 1 || dto.Rating > 5)
            {
                throw new ArgumentException("Некорректные данные отзыва");
            }

            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("reviews", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Ошибка отправки отзыва: {error}");
            }
        }
        public async Task UpdateUserAsync(int userId, string firstName, string lastName, string password)
        {
            var dto = new UserDto
            {
                FirstName = firstName,
                LastName = lastName,
                Password = password // Может быть null или пустым
            };

            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"users/{userId}", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorText = await response.Content.ReadAsStringAsync();
                throw new Exception($"Ошибка обновления пользователя: {errorText}");
            }
        }
        public async Task<int> RegisterUserAsync(string email, string firstName, string lastName, string password)
        {
            var dto = new
            {
                email,
                firstName,
                lastName,
                password
            };

            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("users", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonConvert.DeserializeObject<ApiResponse>(await response.Content.ReadAsStringAsync());
                throw new Exception(errorResponse?.Message ?? "Неизвестная ошибка");
            }

            var result = JsonConvert.DeserializeObject<RegistrationResult>(await response.Content.ReadAsStringAsync());

            if (result.Success && result.UserId > 0)
            {
                App.CartUserId = result.UserId;
                return result.UserId;
            }

            throw new Exception("Ошибка регистрации: ID пользователя не получен");
        }

    }
}
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace XXpressoo.Services
{
    public class ApiService
    {
        private readonly HttpClient client;
        private const string BaseUrl = "https://localhost:5001/api/";  // или https://, если сервер поддерживает HTTPS

        public ApiService()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                {
                    if (cert.Issuer.Equals("CN=localhost"))
                        return true;
                    return errors == System.Net.Security.SslPolicyErrors.None;
                }
            };

            client = new HttpClient(handler)
            {
                BaseAddress = new Uri(BaseUrl)
            };
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
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
                return JsonConvert.DeserializeObject<bool>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Исключение при проверке email: {ex}");
                throw;
            }
        }

        public async Task<bool> RegisterUserAsync(string email, string firstName, string lastName, string password)
        {
            try
            {
                var json = JsonConvert.SerializeObject(new
                {
                    email,
                    firstName,
                    lastName,
                    password
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("users", content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Ошибка регистрации (HTTP {response.StatusCode}): {errorContent}");

                if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    throw new Exception("Пользователь с таким email уже существует");
                }

                throw new Exception($"Ошибка сервера: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка регистрации: {ex}");
                throw;
            }
        }
    }
}
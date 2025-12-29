using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChatBotApp.Services
{
    public class LlmService
    {
        private readonly HttpClient _httpClient;
        private string _apiKey = "YOUR_API_KEY_HERE"; // Placeholder
        private const string ApiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key=";

        public LlmService()
        {
            _httpClient = new HttpClient();
        }

        public void SetApiKey(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<string> GetResponseAsync(string prompt)
        {
            if (string.IsNullOrWhiteSpace(_apiKey) || _apiKey == "YOUR_API_KEY_HERE")
            {
                return "Please provide a valid Gemini API Key.";
            }

            try
            {
                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = prompt }
                            }
                        }
                    }
                };

                var jsonRequest = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(ApiUrl + _apiKey, content);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(jsonResponse);
                
                // Navigate to candidates[0].content.parts[0].text
                var text = doc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                return text ?? "No response content received.";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}

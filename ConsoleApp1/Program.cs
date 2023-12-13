using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        string apiKey = "_-_-_-_-_-_secretapi_-_-_-_-_-_";
        string apiUrl = "https://api.openai.com/v1/chat/completions";

        bool chatting = true;
        Console.WriteLine("Escribe tu pregunta o escribe 'salir' para terminar la conversación:");

        while (chatting)
        {
            string query = Console.ReadLine();

            if (query.ToLower() == "salir")
            {
                chatting = false;
                Console.WriteLine("Conversación finalizada.");
                break;
            }

            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var request = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "system", content = "Eres un asistente útil que proporciona resultados de búsqueda." },
                    new { role = "user", content = query }
                }
            };

            var jsonRequest = JsonSerializer.Serialize(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<ChatResponse>(responseContent);

                Console.WriteLine("\nWiz:");
                Console.WriteLine(responseObject.choices[0].message.content + "\n"); // Salto de línea añadido
            }
            else
            {
                Console.WriteLine($"Error al realizar la búsqueda. Código de estado: {response.StatusCode}");
            }
        }
    }
}

public class ChatResponse
{
    public ChatChoice[] choices { get; set; }
}

public class ChatChoice
{
    public ChatMessage message { get; set; }
}

public class ChatMessage
{
    public string content { get; set; }
}

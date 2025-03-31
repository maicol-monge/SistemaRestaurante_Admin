using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

public class ImgBBService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public ImgBBService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["ImgBB:ApiKey"];
    }

    public async Task<string> UploadImageAsync(Stream imageStream, string fileName)
    {
        using var content = new MultipartFormDataContent();
        var imageContent = new StreamContent(imageStream);
        imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/*");
        content.Add(imageContent, "image", fileName);

        var response = await _httpClient.PostAsync($"https://api.imgbb.com/1/upload?key={_apiKey}", content);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        using var jsonDoc = JsonDocument.Parse(responseString);
        var root = jsonDoc.RootElement;
        var imageUrl = root.GetProperty("data").GetProperty("url").GetString();

        return imageUrl;
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace NwClient.Pages.Home
{
    [Authorize]
    public class HomeModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public string? HeroImageUrl { get; set; }

        public HomeModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("NwAPI");
            var response = await client.GetAsync("api/Image/home");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var doc = JsonDocument.Parse(json);
                HeroImageUrl = doc.RootElement.GetProperty("url").GetString();
            }
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NwClient.Contracts;
using System.Net.Http.Json;

namespace NwClient.Pages.Inspiration
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public List<SessionContract> Sessions { get; set; } = new();
        public List<DifficultyContract> Difficulties { get; set; } = new();
        public string? DefaultSessionImageUrl { get; set; }
        public int? FilterDifficultyId { get; set; }

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync(int? filterDifficultyId)
        {
            FilterDifficultyId = filterDifficultyId;
            var client = _httpClientFactory.CreateClient("NwAPI");

            var difficulties = await client.GetFromJsonAsync<List<DifficultyContract>>("api/Difficulty");
            Difficulties = difficulties ?? new();

            try
            {
                var defaultImg = await client.GetFromJsonAsync<UrlResult>("api/Image/default-session");
                DefaultSessionImageUrl = defaultImg?.Url;
            }
            catch { }

            if (filterDifficultyId.HasValue)
            {
                var filtered = await client.GetFromJsonAsync<List<SessionContract>>($"api/Session/filter?difficultyId={filterDifficultyId}");
                Sessions = filtered ?? new();
            }
            else
            {
                var all = await client.GetFromJsonAsync<List<SessionContract>>("api/Session");
                Sessions = all ?? new();
            }
        }
            private class UrlResult
            {
                public string? Url { get; set; }
            }
        }
    }

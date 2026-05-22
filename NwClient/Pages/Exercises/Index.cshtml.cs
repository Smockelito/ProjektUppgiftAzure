using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NwClient.Contracts;
using System.Net.Http.Json;

namespace NwClient.Pages.Exercises
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty(SupportsGet = true)]
        public int? FilterDifficultyId { get; set; }

        public List<SessionContract> Sessions { get; set; } = new();
        public List<DifficultyContract> Difficulties { get; set; } = new();

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("NwAPI");

            var difficulties = await client.GetFromJsonAsync<List<DifficultyContract>>("api/Difficulty");
            Difficulties = difficulties ?? new();

            var all = await client.GetFromJsonAsync<List<SessionContract>>("api/Session");
            Sessions = (all ?? new())
                .Where(s => FilterDifficultyId == null || s.DifficultyIds.Contains(FilterDifficultyId.Value))
                .ToList();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("NwAPI");
            await client.DeleteAsync($"api/Session/{id}");
            return RedirectToPage(new { FilterDifficultyId });
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NwClient.Contracts;
using System.Net.Http.Json;

namespace NwClient.Pages.Inspiration
{
    [Authorize]
    public class RandomModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public List<DifficultyContract> Difficulties { get; set; } = new();
        public List<SessionContract> RandomSessions { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int? SelectedDifficultyId { get; set; }

        public bool HasSearched { get; set; }

        public RandomModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("NwAPI");
            var difficulties = await client.GetFromJsonAsync<List<DifficultyContract>>("api/Difficulty");
            Difficulties = difficulties ?? new();

            if (SelectedDifficultyId.HasValue)
            {
                HasSearched = true;
                var sessions = await client.GetFromJsonAsync<List<SessionContract>>(
                    $"api/Session/random?difficultyId={SelectedDifficultyId}&count=3");
                RandomSessions = sessions ?? new();
            }
        }

        public IActionResult OnPost()
        {
            return RedirectToPage(new { SelectedDifficultyId });
        }
    }
}

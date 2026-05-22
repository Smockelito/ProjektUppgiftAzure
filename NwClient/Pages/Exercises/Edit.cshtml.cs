using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NwClient.Contracts;
using System.Net.Http.Json;

namespace NwClient.Pages.Exercises
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public List<DifficultyContract> Difficulties { get; set; } = new();

        [BindProperty]
        public int Id { get; set; }

        [BindProperty]
        public string Name { get; set; } = "";

        [BindProperty]
        public string Description { get; set; } = "";

        [BindProperty]
        public string? ImageUrl { get; set; }

        [BindProperty]
        public List<int> SelectedDifficultyIds { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("NwAPI");

            var session = await client.GetFromJsonAsync<SessionContract>($"api/Session/{id}");
            if (session == null) return NotFound();

            Id = session.Id;
            Name = session.Name;
            Description = session.Description;
            ImageUrl = session.ImageUrl;
            SelectedDifficultyIds = session.DifficultyIds;

            await LoadDifficultiesAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                ModelState.AddModelError("Name", "Namn är obligatoriskt.");
                await LoadDifficultiesAsync();
                return Page();
            }

            var client = _httpClientFactory.CreateClient("NwAPI");
            var dto = new CreateSessionContract
            {
                Name = Name,
                Description = Description,
                ImageUrl = ImageUrl,
                DifficultyIds = SelectedDifficultyIds
            };

            var response = await client.PutAsJsonAsync($"api/Session/{Id}", dto);
            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage = "Kunde inte spara ändringarna. Försök igen.";
                await LoadDifficultiesAsync();
                return Page();
            }

            return RedirectToPage("Index");
        }

        private async Task LoadDifficultiesAsync()
        {
            var client = _httpClientFactory.CreateClient("NwAPI");
            var difficulties = await client.GetFromJsonAsync<List<DifficultyContract>>("api/Difficulty");
            Difficulties = difficulties ?? new();
        }
    }
}

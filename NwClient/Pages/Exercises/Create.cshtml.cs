using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NwClient.Contracts;
using System.Net.Http.Json;

namespace NwClient.Pages.Exercises
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public List<DifficultyContract> Difficulties { get; set; } = new();

        [BindProperty]
        public string Name { get; set; } = "";

        [BindProperty]
        public string Description { get; set; } = "";

        [BindProperty]
        public List<int> SelectedDifficultyIds { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            await LoadDifficultiesAsync();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile? ImageFile)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                ModelState.AddModelError("Name", "Namn är obligatoriskt.");
                await LoadDifficultiesAsync();
                return Page();
            }

            var client = _httpClientFactory.CreateClient("NwAPI");

            string? imageUrl = null;
            if (ImageFile != null && ImageFile.Length > 0)
            {
                using var content = new MultipartFormDataContent();
                using var stream = ImageFile.OpenReadStream();
                content.Add(new StreamContent(stream), "file", ImageFile.FileName);

                var uploadResponse = await client.PostAsync("api/Image/upload", content);
                if (uploadResponse.IsSuccessStatusCode)
                {
                    var result = await uploadResponse.Content.ReadFromJsonAsync<UploadResult>();
                    imageUrl = result?.Url;
                }
                else
                {
                    ErrorMessage = "Kunde inte ladda upp bilden. Försök igen.";
                    await LoadDifficultiesAsync();
                    return Page();
                }
            }

            var dto = new CreateSessionContract
            {
                Name = Name,
                Description = Description,
                ImageUrl = imageUrl,
                DifficultyIds = SelectedDifficultyIds
            };

            var response = await client.PostAsJsonAsync("api/Session", dto);
            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage = "Kunde inte skapa övningen. Försök igen.";
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

        private class UploadResult
        {
            public string? Url { get; set; }
        }
    }
}

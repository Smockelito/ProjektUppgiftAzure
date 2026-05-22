using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NwClient.Contracts;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace NwClient.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public LoginContract Input { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var client = _httpClientFactory.CreateClient("NwAPI");

            var response = await client.PostAsJsonAsync("api/Auth/login", new
            {
                Input.Email,
                Input.Password
            });

            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage = "Felaktig e-post eller lösenord.";
                return Page();
            }

            var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
            if (result?.Token == null)
            {
                ErrorMessage = "Kunde inte logga in. Försök igen.";
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, Input.Email),
                new Claim("jwt", result.Token)
            };

            var identity = new ClaimsIdentity(claims, "CookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("CookieAuth", principal);

            return RedirectToPage("/Home/Index");
        }

        private class TokenResponse
        {
            [JsonPropertyName("token")]
            public string? Token { get; set; }
        }
    }
}

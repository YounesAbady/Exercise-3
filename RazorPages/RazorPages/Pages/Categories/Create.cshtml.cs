using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using CurrieTechnologies.Razor.SweetAlert2;
using FluentValidation;

namespace RazorPages.Pages.Categories
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        IConfiguration config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables()
        .Build();
        [TempData]
        public string Msg { get; set; }
        [TempData]
        public string Status { get; set; }
        [Required]
        public string Category;
        public async Task<IActionResult> OnPost(string category)
        {
            if (category!=null)
            {
                var httpClient = HttpContext.RequestServices.GetService<IHttpClientFactory>();
                var client = httpClient.CreateClient();
                client.BaseAddress = new Uri(config["BaseAddress"]);
                var jsonCategory = JsonSerializer.Serialize(category);
                var content = new StringContent(jsonCategory, Encoding.UTF8, "application/json");
                var request = await client.PostAsync($"/api/add-category/{category}", content);
                if (request.IsSuccessStatusCode)
                {
                    Msg = "Successfully Created!";
                    Status = "success";
                    return RedirectToPage("ListCategories");
                }
            }
            else
            {
                Msg = "cant be empty!";
                Status = "error";
                return RedirectToPage();
            }
            return RedirectToPage();
        }
    }
}

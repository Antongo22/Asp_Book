using Asp_Book.Chapter05.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Asp_Book.Chapter05.Pages;

public class IndexModel : PageModel
{
    [BindProperty]
    public ContactFormModel DemoForm { get; set; } = new();

    public string? SuccessMessage { get; set; }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        SuccessMessage = $"Спасибо, {DemoForm.Name}! Ваше сообщение получено.";
        DemoForm = new ContactFormModel();
        ModelState.Clear();
        
        return Page();
    }
}

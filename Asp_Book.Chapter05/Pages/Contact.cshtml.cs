using Asp_Book.Chapter05.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Asp_Book.Chapter05.Pages;

public class ContactModel : PageModel
{
    [BindProperty]
    public ContactFormModel ContactForm { get; set; } = new();

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

        // Здесь обычно сохраняем данные в БД или отправляем email
        SuccessMessage = $"Спасибо, {ContactForm.Name}! Ваше сообщение получено.";
        
        // Очищаем форму после успешной отправки
        ContactForm = new ContactFormModel();
        ModelState.Clear();
        
        return Page();
    }
}

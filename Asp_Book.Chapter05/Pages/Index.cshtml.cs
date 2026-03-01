using Asp_Book.Chapter05.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Asp_Book.Chapter05.Pages;

public class IndexModel : PageModel
{
    [BindProperty]
    public ContactFormModel DemoForm { get; set; } = new();

    [BindProperty]
    public RegistrationModel RegistrationForm { get; set; } = new();

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

        // Устанавливаем сообщение об успехе
        var name = DemoForm.Name;
        SuccessMessage = $"Спасибо, {name}! Ваше сообщение получено.";
        ViewData["SuccessMessage"] = SuccessMessage; // Дублируем в ViewData для надежности
        
        // Очищаем форму после успешной отправки
        DemoForm = new ContactFormModel();
        ModelState.Clear();
        
        // Возвращаем страницу с сообщением об успехе
        return Page();
    }

    public IActionResult OnPostRegister()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Устанавливаем сообщение об успехе
        SuccessMessage = $"Пользователь {RegistrationForm.Username} успешно зарегистрирован!";
        ViewData["SuccessMessage"] = SuccessMessage;
        
        // Очищаем форму после успешной регистрации
        RegistrationForm = new RegistrationModel();
        ModelState.Clear();
        
        return Page();
    }
}

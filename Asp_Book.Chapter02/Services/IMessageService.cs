namespace Asp_Book.Chapter02.Services;

public interface IMessageService
{
    string GetMessage(string name);
    void LogMessage(string message);
}

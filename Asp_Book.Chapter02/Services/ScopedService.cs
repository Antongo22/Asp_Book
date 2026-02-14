namespace Asp_Book.Chapter02.Services;

public class ScopedService : IScopedService
{
    private readonly Guid _id;

    public ScopedService()
    {
        _id = Guid.NewGuid();
    }

    public Guid GetId() => _id;
    public string GetServiceType() => "Scoped";
}

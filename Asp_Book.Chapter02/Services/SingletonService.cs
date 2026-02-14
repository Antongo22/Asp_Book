namespace Asp_Book.Chapter02.Services;

public class SingletonService : ISingletonService
{
    private readonly Guid _id;

    public SingletonService()
    {
        _id = Guid.NewGuid();
    }

    public Guid GetId() => _id;
    public string GetServiceType() => "Singleton";
}

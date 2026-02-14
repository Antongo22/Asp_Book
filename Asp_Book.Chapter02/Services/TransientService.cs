namespace Asp_Book.Chapter02.Services;

public class TransientService : ITransientService
{
    private readonly Guid _id;

    public TransientService()
    {
        _id = Guid.NewGuid();
    }

    public Guid GetId() => _id;
    public string GetServiceType() => "Transient";
}

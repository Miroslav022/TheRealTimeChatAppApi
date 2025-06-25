using RealTimeChatApp.Application.Abstractions.Repositories;

namespace RealTimeChatApp.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AspContext _aspContext;
    public UnitOfWork(AspContext aspContext) => _aspContext = aspContext;
    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
       return _aspContext.SaveChangesAsync(cancellationToken);
    }
}

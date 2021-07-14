namespace SuperLocker.Infrastructure.Providers
{
    public interface IDatabaseConnectionProvider<T>
    {
        T Get();
    }
}
namespace SuperLocker.DataContext.Providers
{
    public interface IDatabaseConnectionProvider<T>
    {
        T Get();
    }
}
namespace EnergyBalancesApi.Services
{
    public interface ILoad<T>
    {
        Task LoadAsync(IEnumerable<T> entities);
    }
}

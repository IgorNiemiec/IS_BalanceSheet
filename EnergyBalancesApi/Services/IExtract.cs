namespace EnergyBalancesApi.Services
{
    public interface IExtract<T>
    {
        Task<T> ExtractAsync();
    }
}

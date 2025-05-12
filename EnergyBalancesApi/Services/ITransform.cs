namespace EnergyBalancesApi.Services
{
    public interface ITransform<TIn, TOut>
    {
        Task<TOut> TransformAsync(TIn input);
    }
}

namespace JsonContractSimplifier.Services.ConverterLocator
{
    public interface IObjectConverter<T> : IConverter
    {
        object Convert(T target);
    }
}
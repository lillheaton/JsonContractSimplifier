namespace EOls.Serialization.Services.ConverterLocator
{
    public interface IObjectConverter<T> : IConverter where T : class
    {
        object Convert(T target);
    }
}
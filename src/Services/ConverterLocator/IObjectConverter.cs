namespace EOls.Serialization.Services.ConverterLocator
{
    public interface IObjectConverter<T> : IConverter
    {
        object Convert(T target);
    }
}
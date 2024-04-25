
namespace Services.FileConversionService
{
    public enum ConverterType
    {
        JsonConverter,
        CsvConverter
    }
    public interface IFileConversionService
    {
        IConverter GetConverter(ConverterType type);
    }
}
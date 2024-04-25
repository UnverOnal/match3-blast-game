using System;
using Services.FileConversionService.Converters;

namespace Services.FileConversionService
{
    public class FileConversionService : IFileConversionService
    {
        public IConverter GetConverter(ConverterType type)
        {
            switch (type)
            {
                case ConverterType.JsonConverter:
                    return new JsonConverter();
                case ConverterType.CsvConverter:
                    return new CsvConverter();
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}

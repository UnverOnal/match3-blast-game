using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using UnityEngine;

namespace Services.FileConversionService.Converters
{
    public class CsvConverter : IConverter
    {
        public List<T> GetDataCollection<T>(TextAsset textAsset) where T : new()
        {
            var dataCollection = new List<T>();
            var content = textAsset.text;

            using var reader = new StringReader(content);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            csv.Read();
            csv.ReadHeader();
            while (csv.Read())
                dataCollection.Add(ParseToObject<T>(csv));

            return dataCollection;
        }
        
        public T GetData<T>(TextAsset textAsset) where T : new()
        {
            var content = textAsset.text;
            using var reader = new StringReader(content);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            csv.Read();
            csv.ReadHeader();
            if (csv.Read())
                return ParseToObject<T>(csv);
            return default;
        }


        private T ParseToObject<T>(CsvReader csv) where T : new()
        {
            var data = new T();
            var dataInfo = typeof(T).GetFields();

            for (var i = 0; i < dataInfo.Length; i++)
            {
                var fieldInfo = dataInfo[i];
                var fieldType = fieldInfo.FieldType;
                var value = GetField(csv.GetField(fieldInfo.Name), fieldType);
                var convertedValue = Convert.ChangeType(value, fieldType);
                fieldInfo.SetValue(data, convertedValue);
            }

            return data;
        }

        private object GetField(string fieldContent, Type type)
        {
            object fieldValue;

            if (type == typeof(int))
                fieldValue = int.TryParse(fieldContent, out var value) ? value: default;
            else if (type == typeof(float))
                fieldValue = float.TryParse(fieldContent, out var value) ? value : default;
            else
                fieldValue = fieldContent;

            return fieldValue;
        }
    }
}
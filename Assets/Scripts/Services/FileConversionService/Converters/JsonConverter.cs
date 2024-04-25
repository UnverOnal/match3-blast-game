using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Services.FileConversionService.Converters
{
    public class JsonConverter : IConverter
    {
        /// <summary>
        /// If the data is a collection...
        /// </summary>
        public List<T> GetDataCollection<T>(TextAsset textAsset) where T : new()
        {
            var dataCollection = GetData<List<T>>(textAsset);
            return dataCollection;
        }
        
        public T GetData<T>(TextAsset textAsset) where T : new()
        {
            var content = textAsset.text;
            var convertedObject = JsonConvert.DeserializeObject<T>(content);
            return convertedObject;
        }
    }
}
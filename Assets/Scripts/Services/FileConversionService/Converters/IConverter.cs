using System.Collections.Generic;
using UnityEngine;

namespace Services.FileConversionService
{
    public interface IConverter
    {
        public List<T> GetDataCollection<T>(TextAsset textAsset)where T : new();
        public T GetData<T>(TextAsset textAsset) where T : new();
    }
}
using System;
using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Services.DataStorageService
{
    public class DataStorageService : IDataStorageService
    {
        private const string Filename = "playerData.json";
        private const string DirectoryName = "saveFiles";

        private string DirectoryPath => $"{Application.persistentDataPath}/{DirectoryName}";
        private string FilePath => $"{DirectoryPath}/{Filename}";
        
        public async UniTask<T> GetFileContentAsync<T>() where T : LocalSaveData
        {
            if (File.Exists(FilePath))
            {
                using var reader = new StreamReader(FilePath);
                var fileContent = await reader.ReadToEndAsync();
                var deserialized = JsonConvert.DeserializeObject<T>(fileContent);
                return deserialized;
            }

            return Activator.CreateInstance<T>();
        }

        public void SetFileContent<T>(T data) where T : LocalSaveData
        {
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }

            var serialized = JsonConvert.SerializeObject(data);
            File.WriteAllText(FilePath, serialized);
        }
    }
}

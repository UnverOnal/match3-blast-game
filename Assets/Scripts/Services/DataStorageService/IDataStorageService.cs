using Cysharp.Threading.Tasks;

namespace Services.DataStorageService
{
    public interface IDataStorageService
    {
        public UniTask<T> GetFileContentAsync<T>() where T : LocalSaveData;
        public void SetFileContent<T>(T data) where T : LocalSaveData;
    }
}
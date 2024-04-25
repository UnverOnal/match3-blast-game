using Board;

namespace Level
{
    public class LevelModel
    {
        private readonly LevelContainer _levelContainer;
        
        private int _levelIndex;
        
        public LevelModel(LevelContainer levelContainer)
        {
            _levelContainer = levelContainer;
            _levelIndex = -1;
        }
        
        public void UpdateLevel() => _levelIndex++;

        public LevelData GetLevelData()
        {
            _levelIndex %= _levelContainer.levels.Count;
            return _levelContainer.levels[_levelIndex];
        }
    }
}

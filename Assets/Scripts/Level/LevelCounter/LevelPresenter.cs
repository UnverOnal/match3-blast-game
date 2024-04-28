using VContainer;

namespace Level.LevelCounter
{
    public class LevelPresenter
    {
        private readonly LevelModel _levelModel;

        [Inject]
        public LevelPresenter(LevelContainer levelContainer)
        {
            _levelModel = new LevelModel(levelContainer);
        }

        public LevelData GetNextLevelData()
        {
            _levelModel.UpdateLevel();
            return _levelModel.GetLevelData();
        }

        public LevelData GetCurrentLevelData()
        {
            return _levelModel.GetLevelData();
        }
    }
}

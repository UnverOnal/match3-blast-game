using UI.Screens;

namespace Ui.Screens.LevelEnd
{
    public class LevelEndScreenView : ScreenView
    {
        private readonly LevelEndScreenResources _screenResources;

        public LevelEndScreenView(LevelEndScreenResources screenResources) : base(screenResources)
        {
            _screenResources = screenResources;
        }
    }
}

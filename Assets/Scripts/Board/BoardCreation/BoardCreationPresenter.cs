using Level;
using VContainer;

namespace Board.BoardCreation
{
    public class BoardCreationPresenter
    {
        [Inject] private LevelPresenter _levelPresenter;
        [Inject] private BoardModel _boardModel;
        private BoardCreationView _boardCreationView;
        
        public BoardCreationPresenter()
        {
            _boardCreationView = new BoardCreationView();
        }

        public void Create()
        {
            //Create logic
            //Set camera
        }
    }
}

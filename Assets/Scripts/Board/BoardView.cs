using Board.BoardCreation;
using Board.CellManagement;

namespace Board
{
    public class BoardView
    {
        private readonly BlockCreator _blockCreator;

        public BoardView(BlockCreator blockCreator)
        {
            _blockCreator = blockCreator;
        }

        public void Merge(CellGroup cellGroup)
        {
            
        }
        
        public void Collapse(){}

        public void Fill()
        {
            
        }

        public void Shuffle()
        {
            
        }
    }
}

using Cysharp.Threading.Tasks;
using GamePlay.Board.Steps.Fill;
using GamePlay.CellManagement;

namespace PowerUpManagement.PowerUpTypes
{
    public class Bomb : PowerUp
    {
        public override UniTask Explode(Cell[,] board, BoardFillPresenter fillPresenter)
        {
            throw new System.NotImplementedException();
        }
    }
}

namespace GamePlay.CellManagement
{
    public class Obstacle : CellBase
    {
        public bool CanExplode => _health <= 0;
        
        private int _health;

        public void Damage() => _health--;
    }
}

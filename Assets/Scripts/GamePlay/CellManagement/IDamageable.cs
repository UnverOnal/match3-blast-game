namespace GamePlay.CellManagement
{
    public interface IDamageable
    {
        bool CanExplode();
        void Damage();
        void Explode();
        BoardLocation GetLocation();
    }
}

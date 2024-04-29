using System.Collections.Generic;

namespace GamePlay.CellManagement
{
    public class CellGroup
    {
        public bool IsEmpty => blocks.Count < 2;

        public Dictionary<int, BoardLocation> bottomLocations;
        
        public readonly HashSet<Cell> blocks;

        public List<IDamageable> explodeables;
        //Damageable neighbours of the group
        private readonly HashSet<IDamageable> _damageables;

        public CellGroup()
        {
            _damageables = new HashSet<IDamageable>();
            bottomLocations = new Dictionary<int, BoardLocation>();
            blocks = new HashSet<Cell>();
            explodeables = new List<IDamageable>();
        }

        public void AddCell(Cell cell)
        {
            blocks.Add(cell);
            var location = cell.Location;
            SetBottomLocation(location);
        }

        public bool HasCell(Cell cell) => blocks.Contains(cell);

        public void AddDamageable(IDamageable damageable)
        {
            if (_damageables.Contains(damageable)) return;
            _damageables.Add(damageable);
        }

        public void DamageNeighbours(Cell[,] boardModelCells)
        {
            foreach (var damageable in _damageables)
            {
                damageable.Damage();
                if (!damageable.CanExplode()) continue;
                explodeables.Add(damageable);
                
                //If there are empty cells under the damageable...
                var location = damageable.GetLocation();
                for (int i = location.y-1 ; i >= 0; i--)
                {
                    var cell = boardModelCells[location.x, i];
                    if (cell == null)
                        location.y = i;
                    else
                        break;
                }
                SetBottomLocation(location);
            }
        }
        
        public void Reset()
        {
            blocks.Clear();
            _damageables.Clear();
            explodeables.Clear();
            bottomLocations.Clear();
        }
        
        private void SetBottomLocation(BoardLocation location)
        {
            if (bottomLocations.TryGetValue(location.x, out var currentLocation))
            {
                if (currentLocation.y > location.y)
                    bottomLocations[location.x] = location;
            }
            else
                bottomLocations.Add(location.x, location);
        }
    }
}

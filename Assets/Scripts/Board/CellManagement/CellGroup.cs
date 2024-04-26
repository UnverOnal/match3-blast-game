using System.Collections.Generic;
using UnityEngine;

namespace Board.CellManagement
{
    public class CellGroup
    {
        private readonly Dictionary<GameObject, Cell> _cells;

        public CellGroup()
        {
            _cells = new Dictionary<GameObject, Cell>();
        }

        public void Add(Cell cell)
        {
            _cells.Add(cell.GameObject, cell);
        }

        public bool HasCell(Cell cell) => _cells.ContainsKey(cell.GameObject);

        public void Reset() => _cells.Clear();
    }
}

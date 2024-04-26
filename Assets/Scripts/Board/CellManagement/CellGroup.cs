using System.Collections.Generic;
using UnityEngine;

namespace Board.CellManagement
{
    public class CellGroup
    {
        public bool IsEmpty => cells.Count < 2;

        public readonly Dictionary<GameObject, Cell> cells;

        public CellGroup()
        {
            cells = new Dictionary<GameObject, Cell>();
        }

        public void Add(Cell cell)
        {
            cells.Add(cell.GameObject, cell);
        }

        public bool HasCell(Cell cell) => cells.ContainsKey(cell.GameObject);

        public void Reset() => cells.Clear();
    }
}

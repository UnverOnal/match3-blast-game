using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<BoardLocation> GetLocations()
        {
            var locations = cells.Select(pair => pair.Value.Location).ToList();
            var distinctLocations = locations.GroupBy(location => location.x)
                .Select(group => group.OrderBy(loc => loc.y).FirstOrDefault());


            return distinctLocations;
        }
    }
}

using GameManagement;
using GamePlay.CellManagement;
using UnityEngine;

namespace Level.LevelCreation
{
    public class LevelFitter
    {
        public Bounds Bounds { get; private set; }
        
        private readonly float _cameraSizeOffset;
        private readonly float _boundsOffsetFactor;
        private readonly Vector3 _cameraPositionOffset;
        private readonly Camera _camera;

        public LevelFitter(GameSettings gameSettings)
        {
            _cameraSizeOffset = gameSettings.cameraSizeOffset;
            _boundsOffsetFactor = gameSettings.boundsOffsetFactor;
            _cameraPositionOffset = gameSettings.cameraPositionOffset;
            _camera = Camera.main;
        }

        public void AlignCamera(Cell[,] cells)
        {
            Bounds = CalculateBounds(cells);

            var cameraPosition = Bounds.center;
            var orthographicSize = CalculateOrthographicSize(Bounds);
            SetCamera(cameraPosition, orthographicSize);
        }

        private Bounds CalculateBounds(Cell[,] cells)
        {
            var minPosition = cells[0, 0].Position;
            var maxPosition = cells[0, 0].Position;
            
            var extents = cells[0,0].Extents * _boundsOffsetFactor;

            for (var i = 0; i < cells.GetLength(0); i++)
            for (var j = 0; j < cells.GetLength(1); j++)
            {
                var cell = cells[i, j];

                minPosition.x = Mathf.Min(minPosition.x, cell.Position.x - extents.x);
                minPosition.y = Mathf.Min(minPosition.y, cell.Position.y - extents.y);
                maxPosition.x = Mathf.Max(maxPosition.x, cell.Position.x + extents.x);
                maxPosition.y = Mathf.Max(maxPosition.y, cell.Position.y + extents.y);
            }

            var bounds = new Bounds
            {
                min = minPosition,
                max = maxPosition
            };
            return bounds;
        }

        private float CalculateOrthographicSize(Bounds bounds)
        {
            var screenAspect = (float)Screen.width / Screen.height;
            var boundsWidth = bounds.size.x;
            var orthographicSize = boundsWidth / 2;

            orthographicSize /= screenAspect;

            return orthographicSize;
        }

        private void SetCamera(Vector2 position, float orthographicSize)
        {
            var cameraTransform = _camera.transform;
            cameraTransform.position =
                new Vector3(position.x, position.y, cameraTransform.position.z) + _cameraPositionOffset;

            _camera.orthographicSize = orthographicSize + _cameraSizeOffset;
        }
    }
}
using GameManagement;
using UnityEngine;

namespace Board.BoardCreation
{
    public class LevelFitter
    {
        public Vector3 BoundsCenter => _bounds.center;

        private readonly float _cameraSizeOffset;
        private readonly Vector3 _cameraPositionOffset;
        private readonly Camera _camera;
        private Bounds _bounds;

        public LevelFitter(GameSettings gameSettings)
        {
            _cameraSizeOffset = gameSettings.cameraSizeOffset;
            _cameraPositionOffset = gameSettings.cameraPositionOffset;
            _camera = Camera.main;
        }

        public void AlignCamera(Cell[,] cells)
        {
            _bounds = CalculateBounds(cells);

            var cameraPosition = _bounds.center;
            var orthographicSize = CalculateOrthographicSize(_bounds);
            SetCamera(cameraPosition, orthographicSize);
        }

        private Bounds CalculateBounds(Cell[,] cells)
        {
            var minPosition = cells[0, 0].Position;
            var maxPosition = cells[0, 0].Position;

            for (var i = 0; i < cells.GetLength(0); i++)
            for (var j = 0; j < cells.GetLength(1); j++)
            {
                var cell = cells[i, j];

                minPosition.x = Mathf.Min(minPosition.x, cell.Position.x - cell.Scale.x / 2f);
                minPosition.y = Mathf.Min(minPosition.y, cell.Position.y - cell.Scale.y / 2f);
                maxPosition.x = Mathf.Max(maxPosition.x, cell.Position.x + cell.Scale.x / 2f);
                maxPosition.y = Mathf.Max(maxPosition.y, cell.Position.y + cell.Scale.y / 2f);
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
            var boundsHeight = bounds.size.y;

            var orthographicSize = Mathf.Max(boundsWidth / 2, boundsHeight / 2);

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
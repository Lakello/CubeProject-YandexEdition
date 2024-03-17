using UnityEngine;
using UnityEngine.UI;

namespace ArenaHero.UI
{
    [ExecuteAlways]
    [RequireComponent(typeof(GridLayoutGroup))]
    public sealed class GridCellSizeSetter : MonoBehaviour, ILayoutGroup
    {
        [SerializeField] private AspectType _aspectType;

        [SerializeField] private int _rowCount;
        [SerializeField] private int _columnCount;
        [SerializeField] private RectTransform _parent;

        private GridLayoutGroup _grid;

        private void OnValidate()
        {
            if (_grid == null)
                _grid = GetComponent<GridLayoutGroup>();

            if (_parent == null)
                _parent = GetComponent<RectTransform>();

            SetLayoutVertical();
        }

        private Vector2 CalculateSize()
        {
            Vector2 size;
            
            switch (_aspectType)
            {
                case AspectType.Horizontal:
                    CalculateX();
                    size.y = size.x;
                    break;
                case AspectType.Vertical:
                    CalculateY();
                    size.x = size.y;
                    break;
                case AspectType.None:
                    CalculateX();
                    CalculateY();
                    break;
                default:
                    size = Vector2.one;
                    break;
            }

            return size;

            void CalculateX() =>
                size.x = _parent.rect.size.x / _columnCount - _grid.spacing.x - _grid.padding.left - _grid.padding.right;

            void CalculateY() =>
                size.y = _parent.rect.size.y / _rowCount - _grid.spacing.y - _grid.padding.top - _grid.padding.bottom;
        }

        public void SetLayoutHorizontal()
        {
        }

        public void SetLayoutVertical() =>
            _grid.cellSize = CalculateSize();
    }
}
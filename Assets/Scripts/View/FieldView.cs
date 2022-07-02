using Model;
using Presenter;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class FieldView : MonoBehaviour
    {
        [SerializeField] private Controls _controls = null;
        [SerializeField] private CellView _cellPrefab = null;
        [SerializeField] private Transform _container = null;
        [SerializeField] private RectTransform _panel = null;
        [SerializeField] private Image _frame = null;
        [SerializeField] private Vector2 _maxSize = new Vector2(1000f, 1500f);

        private int _rows = 0, _cols = 0;
        private CellView[,] _cells = new CellView[0, 0];

        private void Start()
        {
            _controls.OnResized += Resize;
            _controls.OnMixed += Mix;
        }

        private void OnDestroy()
        {
            _controls.OnResized -= Resize;
            _controls.OnMixed -= Mix;
        }

        public void Resize(Field field)
        {
            Vector2 cellSize = Vector2.one * Math.Min(_maxSize.x / field.Cols, _maxSize.y / field.Rows);
            Vector2 panelSize = new Vector2(cellSize.x * field.Cols, cellSize.y * field.Rows);
            _panel.sizeDelta = panelSize;
            _frame.enabled = field.Length > 0;

            CellView[,] temp = new CellView[field.Rows, field.Cols];
            for (int r = 0; r < Math.Max(field.Rows, _rows); r++)
            {
                for (int c = 0; c < Math.Max(field.Cols, _cols); c++)
                {
                    if (r < Math.Min(field.Rows, _rows) && c < Math.Min(field.Cols, _cols))
                        temp[r, c] = _cells[r, c];
                    else if (r < field.Rows && c < field.Cols)
                    {
                        temp[r, c] = Instantiate(_cellPrefab, _container);
                        temp[r, c].Symbol = field[r, c];
                    }
                    else if (r < _rows && c < _cols)
                    {
                        CellView cell = _cells[r, c];
                        _cells[r, c] = null;
                        Destroy(cell.gameObject);
                    }
                }
            }
            _rows = field.Rows;
            _cols = field.Cols;
            _cells = temp;

            UpdatePositions();
        }

        public void Mix(int[] coords)
        {
            if (coords.Length != 2 * _rows * _cols) return;

            for (int r = _rows - 1; r >= 0; r--)
            {
                for (int c = _cols -1; c >= 0; c--)
                {
                    int x = coords[2 * (r * _cols + c)];
                    int y = coords[2 * (r * _cols + c) + 1];
                    (_cells[x, y], _cells[r, c]) = (_cells[r, c], _cells[x, y]);
                }
            }

            UpdatePositions();
        }

        private void UpdatePositions()
        {
            Vector2 cellSize = new Vector2(_panel.sizeDelta.x / _cols, _panel.sizeDelta.y / _rows);
            Vector2 fieldOffset = _panel.sizeDelta / 2f;
            fieldOffset.x = -fieldOffset.x;
            Vector2 cellOffset = -cellSize / 2f;
            cellOffset.x = -cellOffset.x;
            Vector2 offset = fieldOffset + cellOffset;

            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c < _cols; c++)
                {
                    Vector2 position = new Vector2(c * cellSize.x, r * -cellSize.y) + offset;
                    _cells[r, c].Position = position;
                    _cells[r, c].Size = cellSize;
                }
            }
        }
    }
}
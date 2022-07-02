using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Presenter
{
    public class Controls : MonoBehaviour
    {
        [SerializeField] private InputField _height = null;
        [SerializeField] private InputField _width = null;

        private Field _field = new Field();

        public event Action<Field> OnResized;
        public event Action<int[]> OnMixed;

        public void Resize()
        {
            int rows, cols;

            if (int.TryParse(_height.text, out rows)) rows = Mathf.Max(0, rows);
            else throw new ArgumentException($"Input Field '{nameof(_height)}' contains incorrect value '{_height.text}'");

            if (int.TryParse(_width.text, out cols)) cols = Mathf.Max(0, cols);
            else throw new ArgumentException($"Input Field '{nameof(_width)}' contains incorrect value '{_width.text}'");

            _field.Resize(rows, cols);
            OnResized?.Invoke(_field);
        }

        public void Mix()
        {
            int[] coords = _field.Mix();
            OnMixed?.Invoke(coords);
        }
    }
}

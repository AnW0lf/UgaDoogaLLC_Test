using System;
using UnityEngine;
using UnityEngine.UI;

namespace Presenter
{
    public class InputCorrector : MonoBehaviour
    {
        [SerializeField] private InputField _input = null;

        public void OnValueChanged()
        {
            if (int.TryParse(_input.text, out int value))
            {
                int abs = Math.Abs(value);
                if (abs != value) _input.text = abs.ToString();
            }
        }
    }
}

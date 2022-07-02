using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class CellView : MonoBehaviour
    {
        [SerializeField] private RectTransform _rect = null;
        [SerializeField] private Text _symbol = null;
        [SerializeField] private float _translatingDuration = 1f;
        [SerializeField] private float _sizingDuration = 1f;

        public char Symbol
        {
            get
            {
                if (_symbol.text.Length > 0)
                    return _symbol.text[0];
                return ' ';
            }
            set => _symbol.text = value.ToString();
        }

        public Vector2 Size
        {
            get => _rect.sizeDelta;
            set
            {
                if (_sizing != null) StopCoroutine(_sizing);
                _sizing = StartCoroutine(Sizing(value, _sizingDuration));
            }
        }

        private Coroutine _sizing = null;
        private IEnumerator Sizing(Vector2 size, float duration)
        {
            float timer = 0f;
            while (timer <= duration)
            {
                timer += Time.deltaTime;
                _rect.sizeDelta = Vector2.Lerp(_rect.sizeDelta, size, timer / duration);
                yield return null;
            }
            _sizing = null;
        }

        public Vector2 Position
        {
            get => _rect.anchoredPosition;
            set
            {
                if (_translation != null) StopCoroutine(_translation);
                _translation = StartCoroutine(Translation(value, _translatingDuration));
            }
        }

        private Coroutine _translation = null;
        private IEnumerator Translation(Vector2 position, float duration)
        {
            float timer = 0f;
            while(timer <= duration)
            {
                timer += Time.deltaTime;
                _rect.anchoredPosition = Vector2.Lerp(_rect.anchoredPosition, position, timer / duration);
                yield return null;
            }
            _translation = null;
        }
    }
}

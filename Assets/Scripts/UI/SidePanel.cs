using System.Collections;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    public class SidePanel : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField, Min(0f)] private float duration = 0.3f;
        [SerializeField] private float hiddenPosX = -225f;
        
        private RectTransform _rectTransform;
        private bool _isHidden;
        private Vector2 _visiblePos;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _visiblePos = _rectTransform.anchoredPosition;
        }

        public void TogglePanel()
        {
            StopAllCoroutines();
            
            StartCoroutine(_isHidden ? MovePanel(_visiblePos) : MovePanel(new Vector2(hiddenPosX, _visiblePos.y)));
            _isHidden = !_isHidden;
        }

        private IEnumerator MovePanel(Vector2 targetPos)
        {
            Vector2 startPos = _rectTransform.anchoredPosition;
            float time = 0f;

            while (time < duration)
            {
                _rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            
            _rectTransform.anchoredPosition = targetPos;
        }
    }
}

using UnityEngine;

namespace Objects
{
    [RequireComponent(typeof(Renderer))]
    public class ObjectController : MonoBehaviour
    {
        [SerializeField] private Color32 baseColor;
        [Space]
        public bool isSelected;
        
        private Renderer _renderer;
        private bool _isVisible = true;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _renderer.material.color = baseColor;
        }

        public void SetTransparency(float alpha)
        {
            Color color = _renderer.material.color;
            color.a = alpha;
            
            _renderer.material.color = color;
        }
        
        public void SetColor(Color color)
        {
            float currentAlpha = _renderer.material.color.a;
            color.a = currentAlpha;
            
            _renderer.material.color = color;
        }

        public void SetVisibility(bool visible)
        {
            _isVisible = visible;
            _renderer.enabled = visible;
        }
        
        public bool IsVisible() => _isVisible;
    }
}

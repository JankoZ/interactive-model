using Objects;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class UIObject : MonoBehaviour
    {
        [Header("Elements")]
        [SerializeField] private Toggle selectionToggle;
        [SerializeField] private Button redButton;
        [SerializeField] private Button greenButton;
        [SerializeField] private Button blueButton;
        [SerializeField] private Button visibilityButton;
        
        [Space, Header("Colors")]
        [SerializeField] private Color32 redColor = new(153, 61, 61, 255);
        [SerializeField] private Color32 greenColor = new(61, 153, 99, 255);
        [SerializeField] private Color32 blueColor = new(69, 61, 153, 255);
        [Space]
        [SerializeField] private Color32 normalColor = new(64, 64, 64, 255);
        [SerializeField] private Color32 transparentColor = new(64, 64, 64, 100);
        
        private ObjectController _linkedObject;
        private UIManager _manager;

        public void Setup(ObjectController linkedObject, UIManager manager)
        {
            _linkedObject = linkedObject;
            _manager = manager;
            
            if (Camera.main)
            {
                var cameraController = Camera.main.GetComponent<CameraController>();
                GetComponent<Button>().onClick.AddListener(() => cameraController.FocusOn(_linkedObject.transform));
            }

            selectionToggle.onValueChanged.AddListener(OnToggleClicked);
            redButton.onClick.AddListener(() => _linkedObject.SetColor(redColor));
            greenButton.onClick.AddListener(() => _linkedObject.SetColor(greenColor));
            blueButton.onClick.AddListener(() => _linkedObject.SetColor(blueColor));
            visibilityButton.onClick.AddListener(ToggleVisibility);
        }

        private void OnToggleClicked(bool value)
        {
            _linkedObject.isSelected = value;
            _manager.UpdateMasterSelectState();
        }

        private void ToggleVisibility()
        {
            bool newState = !_linkedObject.IsVisible();
            
            _linkedObject.SetVisibility(newState);
            UpdateUIState(_linkedObject.isSelected, newState);
            _manager.UpdateMasterVisibilityState();
        }

        public void UpdateUIState(bool isSelected, bool isVisible)
        {
            selectionToggle.SetIsOnWithoutNotify(isSelected);
            visibilityButton.GetComponentInChildren<Image>().color = isVisible ? normalColor : transparentColor;
        }
    }
}

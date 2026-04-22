using System.Collections.Generic;
using System.Linq;
using Objects;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("Elements")]
        [SerializeField] private Button transparencyButton1;
        [SerializeField] private Button transparencyButton2;
        [SerializeField] private Button transparencyButton3;
        [SerializeField] private Button transparencyButton4;
        [Space]
        [SerializeField] private Toggle visibilityAllToggle;
        [SerializeField] private Toggle selectAllToggle;
        [Space]
        [SerializeField] private GameObject uiObjectPrefab;
        [SerializeField] private Transform listContent;
        
        private readonly List<ObjectController> _objects = new();
        private readonly List<UIObject> _uiObjects = new();

        private void Start()
        {
            _objects.AddRange(FindObjectsOfType<ObjectController>());

            foreach (var obj in _objects)
            {
                var uiObj = Instantiate(uiObjectPrefab, listContent).GetComponent<UIObject>();
                uiObj.Setup(obj, this);
                _uiObjects.Add(uiObj);
            }
            
            transparencyButton1.onClick.AddListener(() => SetTransparencyToSelected(1f));
            transparencyButton2.onClick.AddListener(() => SetTransparencyToSelected(0.75f));
            transparencyButton3.onClick.AddListener(() => SetTransparencyToSelected(0.5f));
            transparencyButton4.onClick.AddListener(() => SetTransparencyToSelected(0.25f));
            
            visibilityAllToggle.onValueChanged.AddListener(OnMasterVisibilityClicked);
            selectAllToggle.onValueChanged.AddListener(OnMasterSelectClicked);
        }

        public void SetTransparencyToSelected(float alpha)
        {
            foreach (var obj in _objects.Where(obj => obj.isSelected))
                obj.SetTransparency(alpha);
        }

        private void OnMasterVisibilityClicked(bool value)
        {
            foreach (var obj in _objects)
                obj.SetVisibility(value);
            RefreshAllUIObjects();
        }

        private void OnMasterSelectClicked(bool value)
        {
            foreach (var obj in _objects)
                obj.isSelected = value;
            RefreshAllUIObjects();
        }

        public void RefreshAllUIObjects()
        {
            for (int i = 0; i < _uiObjects.Count; ++i)
                _uiObjects[i].UpdateUIState(_objects[i].isSelected, _objects[i].IsVisible());
        }

        public void UpdateMasterVisibilityState()
        {
            bool anyVisible = _objects.All(o => o.IsVisible());
            visibilityAllToggle.SetIsOnWithoutNotify(anyVisible);
        }

        public void UpdateMasterSelectState()
        {
            bool areAllSelected = _objects.All(o => o.isSelected);
            selectAllToggle.SetIsOnWithoutNotify(areAllSelected);
        }

        public List<ObjectController> GetObjects() => _objects;
    }
}

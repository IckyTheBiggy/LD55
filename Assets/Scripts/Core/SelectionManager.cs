using System;
using UnityEngine;
using NnUtils.Scripts;

namespace Core
{
    public class SelectionManager : MonoBehaviour
    {
        private GameObject _hoveredObject;
        public GameObject HoveredObject
        {
            get => _hoveredObject;
            set
            {
                if (_hoveredObject == value) return;
                if (_hoveredObject != null && _hoveredObject.TryGetComponent<ISelectable>(out var selectable)) 
                    selectable.PointerExit();
                _hoveredObject = value;
                OnHoveredObjectChanged?.Invoke();
                if (value == null || !value.TryGetComponent(out selectable)) return;
                selectable.PointerEnter();
            }
        }
        public Action OnHoveredObjectChanged;
        
        private GameObject _selectedObject;
        public GameObject SelectedObject
        {
            get => _selectedObject;
            set
            {
                if (_selectedObject == value) return;
                if (_selectedObject != null && _selectedObject.TryGetComponent<ISelectable>(out var selectable))
                    selectable.Deselect();
                _selectedObject = value;
                OnSelectedObjectChanged?.Invoke();
                if (value == null || !value.TryGetComponent(out selectable)) return;
                selectable.Select();
            }
        }
        public Action OnSelectedObjectChanged;

        [SerializeField] private LayerMask _selectionLayerMask;
        
        private void Update()
        {
            var pos = Misc.GetPointerPos();
            HoveredObject = GetHoveredObject(pos);
            if (HoveredObject == null && PointerUp()) { SelectedObject = null; return; }
            if (HoveredObject == null || !HoveredObject.TryGetComponent<ISelectable>(out var selectable)) return;
            if (PointerDown()) selectable.PointerDown();
            if (Pointer()) selectable.Pointer();
            if (PointerUp()) selectable.PointerUp();
        }

        private GameObject GetHoveredObject(Vector3 pointerPos)
        {
            var mainCamera = GameManager.Instance.MainCam;
            var ray = mainCamera.ScreenPointToRay(pointerPos);
            return Physics.Raycast(ray, out var hit, 300, _selectionLayerMask) ?
                hit.transform.gameObject : null;
        }
        
        private bool PointerDown() =>
            !Misc.IsPointerOverUI &&
            Input.GetKeyDown(KeyCode.Mouse0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);
        
        private bool Pointer() =>
            !Misc.IsPointerOverUI &&
            Input.GetKey(KeyCode.Mouse0) || Input.touchCount > 0;
        
        private bool PointerUp() =>
            !Misc.IsPointerOverUI &&
            Input.GetKeyUp(KeyCode.Mouse0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended);
 
    }
}

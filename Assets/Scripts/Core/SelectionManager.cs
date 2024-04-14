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
                if (_hoveredObject != null) _hoveredObject.GetComponent<ISelectable>().PointerExit();
                _hoveredObject = value;
                if (value == null || !value.TryGetComponent<ISelectable>(out var selectable)) return;
                selectable.PointerEnter();
            }
        }
        
        private GameObject _selectedObject;
        public GameObject SelectedObject
        {
            get => _selectedObject;
            set
            {
                if (_selectedObject == value) return;
                if (_selectedObject != null) _selectedObject.GetComponent<ISelectable>().Deselect();
                _selectedObject = value;
                _selectedObject.GetComponent<ISelectable>().Select();
            }
        }

        [SerializeField] private LayerMask _selectionLayerMask;
        
        private void Update()
        {
            var pos = GameManager.Instance.MainCam.ScreenToWorldPoint(Misc.GetPointerPos());
            HoveredObject = GetHoveredObject(pos);
            if (HoveredObject == null && PointerUp()) SelectedObject = null;
            if (HoveredObject == null) return;
            if (PointerDown()) HoveredObject.GetComponent<ISelectable>().PointerDown();
            if (Pointer()) HoveredObject.GetComponent<ISelectable>().PointerDown();
            if (PointerUp()) HoveredObject.GetComponent<ISelectable>().PointerUp();
        }

        private GameObject GetHoveredObject(Vector3 pointerPos) =>
            !Physics.Raycast(
                pointerPos,
                GameManager.Instance.MainCam.transform.forward,
                out var hit,
                300,
                _selectionLayerMask) ? null : hit.transform.gameObject;
        
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

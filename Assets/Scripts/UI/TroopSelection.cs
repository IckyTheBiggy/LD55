using Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TroopSelection : MonoBehaviour
    {
        private bool _isHovered;
        private bool _isClicked;
        private bool _isSelected;
        [SerializeField] private int _troopIndex;
        [SerializeField] private Image _image;
        [SerializeField] private Color _normalColor;
        [SerializeField] private Color _hoveredColor;
        [SerializeField] private Color _clickedColor;
        [SerializeField] private Color _selectedColor;
        
        private void SetTroop(int index) => GameManager.Instance.SelectedTroop = index;

        public void OnPointerEnter()
        {
            if (_isSelected || _isClicked) return;
            _isHovered = true;
            _image.color = _hoveredColor;
        }

        public void OnPointerExit()
        {
            if (_isSelected || _isClicked) return;
            _isHovered = false;
            _image.color = _normalColor;
        }

        public void OnPointerDown()
        {
            if (_isSelected) return;
            _image.color = _clickedColor;
            _isClicked = true;
        }

        public void OnPointerUp()
        {
            if (_isSelected) return;
            if (_isHovered) Select();
            _isClicked = false;
        }

        public void Select()
        {
            var selected = GameManager.Instance.SelectedSelection;
            if (selected != null) selected.Deselect();
            GameManager.Instance.SelectedSelection = this;
            _image.color = _selectedColor;
            SetTroop(_troopIndex);
            _isSelected = true;
        }

        public void Deselect()
        {
            _image.color = _normalColor;
            _isSelected = false;
        }
    }
}
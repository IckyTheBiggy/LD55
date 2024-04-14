using Core;
using UnityEngine;
using UnityEngine.UI;

namespace World
{
    public class TroopScript : MonoBehaviour, ISelectable, IDisplayable
    {
        [SerializeField] public string Name;
        [SerializeField] public int Health;
        [SerializeField] public int MaxHealth;
        [SerializeField] public Sprite Sprite;
        #region ISelectable
        public void PointerEnter()
        {
            Debug.Log("Pointer Enter");
        }

        public void PointerExit()
        {
            Debug.Log("Pointer Exit");
        }

        public void PointerDown()
        {
            Debug.Log("Pointer Down");
        }

        public void Pointer()
        {
            Debug.Log("Pointer");
        }

        public void PointerUp()
        {
            Debug.Log("Pointer Up");
        }

        public void Select()
        {
            Debug.Log("Select");
        }

        public void Deselect()
        {
            Debug.Log("Deselect");
        }
        #endregion

        #region IDisplayable
        public string GetName() => Name;
        public int GetHealth() => Health;
        public int GetMaxHealth() => MaxHealth;
        public Sprite GetSprite() => Sprite;
        #endregion
    }
}

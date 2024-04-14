using Core;
using UnityEngine;

namespace World
{
    public class TroopScript : MonoBehaviour, ISelectable
    {
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
    }
}

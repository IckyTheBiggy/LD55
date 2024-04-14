namespace Core
{
    public interface ISelectable
    {
        public void PointerEnter();
        public void PointerExit();
        public void PointerDown();
        public void Pointer();
        public void PointerUp();
        public void Select();
        public void Deselect();
    }
}
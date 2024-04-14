using UnityEngine;

namespace Core
{
    public interface IDisplayable
    {
        public string GetName();
        public int GetHealth();
        public int GetMaxHealth();
        public Sprite GetSprite();
    }
}
using UnityEngine;

public class BaseScript : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHealth;

    private int _health;
    
    private void Start()
    {
        _health = _maxHealth;
    }

    private void HandleBaseDestruction()
    {
        Debug.Log("Game Over");
    }

    public void Damage(int damage)
    {
        _health -= damage;
        
        if (_health >= 0)
            HandleBaseDestruction();
    }
}

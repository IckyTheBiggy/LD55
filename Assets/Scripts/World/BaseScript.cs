using UnityEngine;
using UnityEngine.UI;

public class BaseScript : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private Slider _healthBar;

    private int _health;
    
    private void Start()
    {
        _health = _maxHealth;
        _healthBar.maxValue = _maxHealth;
        _healthBar.value = _health;
    }

    private void HandleBaseDestruction()
    {
        Debug.Log("Game Over");
    }

    public void Damage(int damage)
    {
        _health -= damage;
        _healthBar.value = _health;
        
        if (_health <= 0)
            HandleBaseDestruction();
    }
}

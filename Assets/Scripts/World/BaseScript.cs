using NnUtils.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

public class BaseScript : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private NBar _healthBar;

    private int _health;
    
    private void Start()
    {
        _health = _maxHealth;
        _healthBar.Max = _maxHealth;
        _healthBar.Value = _health;
    }

    private void HandleBaseDestruction()
    {
        Debug.Log("Game Over");
    }

    public void Damage(int damage)
    {
        _health -= damage;
        _healthBar.Value = _health;
        
        if (_health <= 0)
            HandleBaseDestruction();
    }
}

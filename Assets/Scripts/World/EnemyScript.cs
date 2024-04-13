using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour, IDamageable
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _attackDistance;
    [SerializeField] private int _damageAmount;
    [SerializeField] private float _attackSpeed;
    
    [SerializeField] private int _maxHealth;

    private bool _attacking;

    private Transform _target;

    private int _health;
    
    private void Start()
    {
        _health = _maxHealth;
        
        _target = GameManager.Instance.Base;
    }

    private void Update()
    {
        float distanceToBase = Vector3.Distance(transform.position, _target.position);

        if (distanceToBase < _attackDistance)
        {
            Attack();
            _agent.SetDestination(transform.position);
        }
        else
            _agent.SetDestination(_target.position);
    }

    private void Attack()
    {
        if (!_attacking)
        {
            
            IDamageable damageable;

            damageable = _target.gameObject.GetComponent<IDamageable>();

            if (damageable != null)
                damageable.Damage(_damageAmount);
            
            _attacking = true;
            
            Invoke("ResetAttack", _attackSpeed);
        }
    }

    private void ResetAttack()
    {
        _attacking = false;
    }

    public void Damage(int damage)
    {
        
    }
}

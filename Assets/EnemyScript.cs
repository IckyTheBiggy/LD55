using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour, IDamageable
{
    [SerializeField] private NavMeshAgent _agent;
    
    [SerializeField] private int _maxHealth;

    private Transform _target;

    private int _health;
    
    private void Start()
    {
        _health = _maxHealth;

        _target = GameManager.Instance.Base;

        _agent.SetDestination(_target.position);
    }

    public void Damage(int damage)
    {
        
    }
}

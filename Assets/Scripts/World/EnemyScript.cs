using System;
using Core;
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
        
    }

    private void Update()
    {
        
    }

    public void Damage(int damage)
    {
        
    }
}

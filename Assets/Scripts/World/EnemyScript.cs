using System;
using Core;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour, IDamageable
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _detectionRange;
    [SerializeField] private float _baseAttackDistance;
    [SerializeField] private float _attackDistance;
    [SerializeField] private int _damageAmount;
    [SerializeField] private float _attackSpeed;
    
    [SerializeField] private int _maxHealth;

    private Animator _animator;
    private bool _attacking;

    private Transform _base;

    private int _health;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _health = _maxHealth;
        
        _base = GameManager.Instance.Base;
    }

    private void Update()
    {
        GameObject target = null;
        
        if (_rb.velocity.magnitude > 0)
            _animator.SetBool("Moving", true);
        else
            _animator.SetBool("Moving", false);

        Collider[] troops = Physics.OverlapSphere(transform.position, _detectionRange);
        foreach (var troop in troops)
        {
            if (troop.CompareTag("Troop"))
            {
                target = troop.gameObject;
                break;
            }
        }

        if (target != null)
        {
            _agent.SetDestination(target.transform.position);

            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            if (distanceToTarget <= _attackDistance)
            {
                _animator.SetTrigger("Attacking");
                Attack(target);
            }
        }

        else
        {
            _agent.SetDestination(_base.position);

            float distanceToBase = Vector3.Distance(transform.position, _base.transform.position);
            if (distanceToBase <= _baseAttackDistance)
            {
                _animator.SetTrigger("Attacking");
                Attack(_base.gameObject);
            }
        }
    }

    private void Attack(GameObject objectToAttack)
    {
        if (!_attacking)
        {
            IDamageable damageable;
            damageable = objectToAttack.GetComponent<IDamageable>();
            
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
        Debug.Log("Damaged");
        _health -= damage;
        
        if (_health <= 0)
            Destroy(gameObject);
    }
}

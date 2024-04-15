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
    [SerializeField] private ParticleSystem _hitParticles;
    [SerializeField] private ParticleSystem _destoryParticles;
    
    [SerializeField] private int _maxHealth;

    private Vector3 _destination;
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
        GameObject target = FindTarget();
        
        if (target != null)
        {
            Relocate(target.transform.position);

            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            if (distanceToTarget <= _attackDistance)
            {
                _animator.SetTrigger("Attacking");
                Attack(target);
            }
        }

        else
        {
            Relocate(_base.position);

            float distanceToBase = Vector3.Distance(transform.position, _base.transform.position);
            if (distanceToBase <= _baseAttackDistance)
            {
                _animator.SetTrigger("Attacking");
                Attack(_base.gameObject);
            }
        }

        _agent.SetDestination(_destination);
        
        if (_rb.velocity.magnitude > 0)
            _animator.SetBool("Moving", true);
        else
            _animator.SetBool("Moving", false);
    }

    private void Relocate(Vector3 position)
    {
        _destination = position;
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

    private void ResetAttack() => _attacking = false;
    
    private GameObject FindTarget()
    {
        Collider[] troops = Physics.OverlapSphere(transform.position, _detectionRange);
        foreach (var troop in troops)
        {
            if (troop.CompareTag("Troop"))
            {
                return troop.gameObject;
                break;
            }
        }

        return null;
    }

    public void Damage(int damage)
    {
        _health -= damage;
        Instantiate(_hitParticles, transform.position, Quaternion.identity);

        if (_health <= 0)
        {
            Instantiate(_destoryParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using World;

public class TroopAI : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private int _damage;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _detectionRange;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _attackDistance;

    private Animator _animator;
    private bool _foundTarget;
    private bool _attacking;

    private void Start() => _animator = GetComponentInChildren<Animator>();

    private void Update()
    {
        //if (_agent.hasPath) return;
        GameObject target = null;
        
        if (_rb.velocity.magnitude > 0)
            _animator.SetBool("Moving", true);
        else
            _animator.SetBool("Moving", false);

        Collider[] enemies = Physics.OverlapSphere(transform.position, _detectionRange);
        foreach (var enemy in enemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                target = enemy.gameObject;
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
    }

    private void Attack(GameObject objectToAttack)
    {
        if (!_attacking)
        {
            IDamageable damageable;
            damageable = objectToAttack.gameObject.GetComponent<IDamageable>();

            if (damageable != null)
                damageable.Damage(_damage);

            _attacking = true;
            
            Invoke("ResetAttack", _attackSpeed);
        }
    }

    private void ResetAttack()
    {
        _attacking = false;
    }
    
    public void Relocate(Vector3 position)
    {
        _agent.SetDestination(position); //nan add the rest of the logic here
        //I am thinking that he shouldn't target enemies if he is moving because that'd give u less control
    }
}

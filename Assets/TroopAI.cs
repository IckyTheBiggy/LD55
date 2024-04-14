using System;
using UnityEngine;
using UnityEngine.AI;

public class TroopAI : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _findRange;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _attackDistance;

    private Transform _target;
    private bool _foundTarget;

    private bool _attacking;

    private void Start()
    {

    }

    private void Update()
    {
        return;
        FindEnemy();

        if (_foundTarget)
            _agent.SetDestination(_target.position);
        else
            _agent.SetDestination(transform.position);

        if (_target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, _target.position);

            if (distanceToTarget < _attackDistance)
                Attack();
        }
    }

    private void FindEnemy()
    {
        if (_target == null)
            _foundTarget = false;

        if (_foundTarget)
            return;

        Collider[] colliders = Physics.OverlapSphere(transform.position, _findRange);

        foreach (var collider in colliders)
        {
            if (collider.tag == "Enemy")
                _target = collider.transform;
        }

        if (_target != null)
            _foundTarget = true;
    }

    private void Attack()
    {
        if (!_attacking)
        {
            IDamageable damageable;

            damageable = _target.gameObject.GetComponent<IDamageable>();

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

using System;
using UnityEngine;
using UnityEngine.AI;

public class TroopAI : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _findRange;

    private Transform _target;
    
    private void Start()
    {
        
    }

    private void Update()
    {
        FindEnemies();
    }

    private void FindEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _findRange);

        foreach (var collider in colliders)
        {
            if (collider.tag == "Enemy")
                _target = collider.transform;
        }

        if (_target != null)
            _agent.SetDestination(_target.position);
    }
    
}

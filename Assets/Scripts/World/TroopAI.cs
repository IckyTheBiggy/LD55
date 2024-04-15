using UnityEngine;
using UnityEngine.AI;

namespace World
{
    public class TroopAI : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private int _damage;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private float _detectionRange;
        [SerializeField] private float _attackSpeed;
        [SerializeField] private float _attackDistance;

        private Vector3 _destination;
        private bool _isRelocating;
        private Animator _animator;
        private bool _foundTarget;
        private bool _attacking;
        private static readonly int Moving = Animator.StringToHash("Moving");
        private static readonly int Attacking = Animator.StringToHash("Attacking");

        private void Start() => _animator = GetComponentInChildren<Animator>();

        private void Update()
        {
            _animator.SetBool(Moving, _rb.velocity.magnitude > 0.1f);
            var target = GetTarget();
            
            if (_isRelocating)
            {
                if (target != null || transform.position == _destination) _isRelocating = false;
                return;
            }

            _destination = target == null ? transform.position : target.transform.position;
            _agent.SetDestination(_destination);
            if (target != null) Attack(target);
        }

        private GameObject GetTarget()
        {
            var enemies = Physics.OverlapSphere(transform.position, _detectionRange);
            foreach (var enemy in enemies)
                if (enemy.CompareTag("Enemy")) return enemy.gameObject;
            return null;
        }
        
        private void Attack(GameObject target)
        {
            if (_attacking) return;
            var distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            if (distanceToTarget > _attackDistance) return;

            var damageable = target.GetComponent<IDamageable>();
            damageable?.Damage(_damage);
            _animator.SetTrigger(Attacking);
            _attacking = true;
            Invoke("ResetAttack", _attackSpeed);
        }

        private void ResetAttack() => _attacking = false;
    
        public void Relocate(Vector3 position)
        {
            _destination = position;
            if (!_agent.SetDestination(position)) return;
            Debug.Log("Here");
            _isRelocating = true;
        }
    }
}

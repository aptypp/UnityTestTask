using System;
using TestTask.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace TestTask.Units
{
    public abstract class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField]
        private int _damage;

        [SerializeField]
        private int _startHealth;

        [SerializeField]
        private int _restoreHealthCount;

        [SerializeField]
        private float _atackSpeed;

        [SerializeField]
        private float _attackRange;

        [SerializeField]
        private Animator _animatorController;

        [SerializeField]
        private NavMeshAgent _agent;

        [SerializeField]
        private ParticleSystem _highlightParticles;

        private int _health;
        private bool isDead;
        private float lastAttackTime;
        private float _targetMoveAnimationSpeed;
        private Action<int> _restorePlayerHealth;

        private static readonly int AttackId = Animator.StringToHash("Attack");
        private static readonly int SpeedId = Animator.StringToHash("Speed");
        private static readonly int DieId = Animator.StringToHash("Die");

        private void Update()
        {
            if (isDead) return;

            float distance = Vector3.Distance(transform.position, SceneManager.Instance.Player.transform.position);

            if (distance <= _attackRange)
            {
                _agent.isStopped = true;
                _targetMoveAnimationSpeed = 0;
                if (Time.time - lastAttackTime > _atackSpeed)
                {
                    lastAttackTime = Time.time;
                    SceneManager.Instance.Player.ReceiveDamage(_damage);
                    _animatorController.SetTrigger(AttackId);
                }
            }
            else
            {
                _agent.isStopped = false;
                _targetMoveAnimationSpeed = 1;
                _agent.SetDestination(SceneManager.Instance.Player.transform.position);
            }

            _animatorController.SetFloat(SpeedId,
                Mathf.Lerp(_animatorController.GetFloat(SpeedId), _targetMoveAnimationSpeed,
                    _agent.speed * Time.deltaTime));
        }


        public void Initialize(Action<int> restorePlayerHealth)
        {
            _health = _startHealth;
            _restorePlayerHealth = restorePlayerHealth;

            SceneManager.Instance.AddEnemie(this);
            _agent.SetDestination(SceneManager.Instance.Player.transform.position);
        }

        public void ReceiveDamage(int damage)
        {
            _health -= damage;

            if (_health > 0) return;

            Die();
        }

        protected virtual void Die()
        {
            SceneManager.Instance.RemoveEnemie(this);
            isDead = true;
            _animatorController.SetTrigger(DieId);
            _agent.isStopped = true;
            _restorePlayerHealth(_restoreHealthCount);
            Destroy(_highlightParticles);
        }
    }
}
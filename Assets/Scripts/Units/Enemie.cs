using System;
using UnityEngine;
using UnityEngine.AI;

namespace TestTask
{
    public class Enemie : MonoBehaviour, IDamageable
    {
        public int Damage;
        public int RestoreHealthCount;
        public float AtackSpeed;
        public float AttackRange = 2;

        public Animator AnimatorController;
        public NavMeshAgent Agent;

        [SerializeField]
        private int _startHealth;

        private int _health;
        private bool isDead;
        private float lastAttackTime;
        private Action<int> _onDead;

        private static readonly int AttackId = Animator.StringToHash("Attack");
        private static readonly int SpeedId = Animator.StringToHash("Speed");
        private static readonly int DieId = Animator.StringToHash("Die");

        public void Initialize(Action<int> onDead)
        {
            _health = _startHealth;
            _onDead = onDead;
            
            SceneManager.Instance.AddEnemie(this);
            Agent.SetDestination(SceneManager.Instance.Player.transform.position);
        }

        private void Update()
        {
            if (isDead) return;

            float distance = Vector3.Distance(transform.position, SceneManager.Instance.Player.transform.position);

            if (distance <= AttackRange)
            {
                Agent.isStopped = true;
                if (Time.time - lastAttackTime > AtackSpeed)
                {
                    lastAttackTime = Time.time;
                    SceneManager.Instance.Player.ReceiveDamage(Damage);
                    AnimatorController.SetTrigger(AttackId);
                }
            }
            else
            {
                Agent.SetDestination(SceneManager.Instance.Player.transform.position);
            }

            AnimatorController.SetFloat(SpeedId, Agent.speed);
        }

        public void ReceiveDamage(int damage)
        {
            _health -= damage;

            if (_health > 0) return;

            Die();
        }

        private void Die()
        {
            SceneManager.Instance.RemoveEnemie(this);
            isDead = true;
            AnimatorController.SetTrigger(DieId);
            Agent.isStopped = true;
            _onDead(RestoreHealthCount);
        }
    }
}
using UnityEngine;
using UnityEngine.AI;

namespace TeskTask
{
    public class Enemie : MonoBehaviour
    {
        public float Hp;
        public float Damage;
        public float AtackSpeed;
        public float AttackRange = 2;


        public Animator AnimatorController;
        public NavMeshAgent Agent;
        private bool isDead;

        private float lastAttackTime;


        private void Start()
        {
            SceneManager.Instance.AddEnemie(this);
            Agent.SetDestination(SceneManager.Instance.Player.transform.position);
        }

        private void Update()
        {
            if (isDead) return;

            if (Hp <= 0)
            {
                Die();
                Agent.isStopped = true;
                return;
            }

            float distance = Vector3.Distance(transform.position, SceneManager.Instance.Player.transform.position);

            if (distance <= AttackRange)
            {
                Agent.isStopped = true;
                if (Time.time - lastAttackTime > AtackSpeed)
                {
                    lastAttackTime = Time.time;
                    SceneManager.Instance.Player.Hp -= Damage;
                    AnimatorController.SetTrigger("Attack");
                }
            }
            else
            {
                Agent.SetDestination(SceneManager.Instance.Player.transform.position);
            }

            AnimatorController.SetFloat("Speed", Agent.speed);
            Debug.Log(Agent.speed);
        }


        private void Die()
        {
            SceneManager.Instance.RemoveEnemie(this);
            isDead = true;
            AnimatorController.SetTrigger("Die");
        }
    }
}
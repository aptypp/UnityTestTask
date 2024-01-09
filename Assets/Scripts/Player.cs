using System.Collections.Generic;
using UnityEngine;

namespace TeskTask
{
    public class Player : MonoBehaviour
    {
        public float Hp;
        public float Damage;
        public float AtackSpeed;
        public float AttackRange = 2;
        public Animator AnimatorController;
        private bool isDead;

        private float lastAttackTime;

        private void Update()
        {
            if (isDead) return;

            if (Hp <= 0)
            {
                Die();
                return;
            }


            List<Enemie> enemies = SceneManager.Instance.Enemies;
            Enemie closestEnemie = null;

            for (int i = 0; i < enemies.Count; i++)
            {
                Enemie enemie = enemies[i];
                if (enemie == null) continue;

                if (closestEnemie == null)
                {
                    closestEnemie = enemie;
                    continue;
                }

                float distance = Vector3.Distance(transform.position, enemie.transform.position);
                float closestDistance = Vector3.Distance(transform.position, closestEnemie.transform.position);

                if (distance < closestDistance) closestEnemie = enemie;
            }

            if (closestEnemie != null)
            {
                float distance = Vector3.Distance(transform.position, closestEnemie.transform.position);
                if (distance <= AttackRange)
                    if (Time.time - lastAttackTime > AtackSpeed)
                    {
                        //transform.LookAt(closestEnemie.transform);
                        transform.transform.rotation =
                            Quaternion.LookRotation(closestEnemie.transform.position - transform.position);

                        lastAttackTime = Time.time;
                        closestEnemie.Hp -= Damage;
                        AnimatorController.SetTrigger("Attack");
                    }
            }
        }

        private void Die()
        {
            isDead = true;
            AnimatorController.SetTrigger("Die");

            SceneManager.Instance.GameOver();
        }
    }
}
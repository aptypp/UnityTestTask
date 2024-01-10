using System;
using System.Collections.Generic;
using TestTask.Extensions.Observables;
using TestTask.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TestTask.Units
{
    public class Player : MonoBehaviour, IDamageable
    {
        public event Action lostClosestEnemy;
        public event Action<Enemy> findClosestEnemy;

        public Extensions.Observables.IObservable<int> health => _health;

        [field: SerializeField]
        public int startHealth;

        public int Damage;
        public int SuperDamage;
        public float AtackCooldown;
        public float SuperAtackCooldown;
        public float AttackRange = 2;
        public Animator AnimatorController;

        [SerializeField]
        private float _moveSpeed;

        [SerializeField]
        private float _rotationSpeed;

        [SerializeField]
        private ParticleSystem _highlightParticles;

        private bool _isDead;
        private float _lastAttackTime;
        private float _lastAttackCooldown;
        private float _targetMoveAnimationSpeed;
        private Enemy _closestEnemy;
        private Vector3 _position;
        private Vector3 _moveDirection;
        private Quaternion _moveRotation;
        private Observable<int> _health;

        private static readonly int AttackId = Animator.StringToHash("Attack");
        private static readonly int SuperAttackId = Animator.StringToHash("SuperAttack");
        private static readonly int DieId = Animator.StringToHash("Die");
        private static readonly int SpeedId = Animator.StringToHash("Speed");

        private void Awake()
        {
            _health = new Observable<int>(startHealth);
            _position = transform.position;
        }

        private void Update()
        {
            if (_isDead) return;

            TryMove();
            FindEnemy();
        }

        public void SetMoveDirection(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();

            _moveDirection = new Vector3(input.x, 0, input.y);

            if (input != Vector2.zero)
            {
                _moveRotation = Quaternion.LookRotation(_moveDirection);
            }

            _targetMoveAnimationSpeed = input == Vector2.zero ? 0 : 1;
        }


        public void AttackInput()
        {
            if (_isDead) return;

            TryAttackEnemy();
        }

        public void ReceiveDamage(int damage)
        {
            if (_isDead) return;

            _health.value -= damage;

            if (_health.value > 0) return;

            Die();
        }

        public bool SuperAttackInput()
        {
            if (_isDead) return false;

            return TrySuperAttackEnemy();
        }

        public void IncreaseHealth(int value) => _health.value += value;

        private void FindEnemy()
        {
            if (TryGetClosestEnemy(out Enemy enemy))
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

                if (distanceToEnemy > AttackRange)
                {
                    _closestEnemy = null;
                    lostClosestEnemy?.Invoke();
                    return;
                }

                _closestEnemy = enemy;
                findClosestEnemy?.Invoke(enemy);
                return;
            }

            if (_closestEnemy == null) return;

            _closestEnemy = null;
            lostClosestEnemy?.Invoke();
        }

        private bool TryGetClosestEnemy(out Enemy enemy)
        {
            IReadOnlyList<Enemy> enemies = SceneManager.Instance.enemies;

            Enemy closest = null;

            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy enemie = enemies[i];
                if (enemie == null) continue;

                if (closest == null)
                {
                    closest = enemie;
                    continue;
                }

                float distance = Vector3.Distance(transform.position, enemie.transform.position);
                float closestDistance = Vector3.Distance(transform.position, closest.transform.position);

                if (distance < closestDistance) closest = enemie;
            }

            enemy = closest;

            return enemy != null;
        }

        private void TryMove()
        {
            _position += _moveDirection * _moveSpeed * Time.deltaTime;

            if (transform.position != _position)
            {
                float lerpProgress = _moveSpeed * Time.deltaTime;

                transform.position = Vector3.Lerp(transform.position, _position, lerpProgress);
                AnimatorController.SetFloat(SpeedId,
                    Mathf.Lerp(AnimatorController.GetFloat(SpeedId), _targetMoveAnimationSpeed, lerpProgress));
            }

            if (transform.rotation != _moveRotation)
            {
                transform.rotation =
                    Quaternion.Lerp(transform.rotation, _moveRotation, _rotationSpeed * Time.deltaTime);
            }
        }

        private void TryAttackEnemy()
        {
            if (Time.time - _lastAttackTime <= AtackCooldown) return;

            _lastAttackTime = Time.time;
            _lastAttackCooldown = AtackCooldown;
            AnimatorController.SetTrigger(AttackId);

            if (_closestEnemy == null) return;

            AttackEnemy(_closestEnemy, Damage);
        }

        private bool TrySuperAttackEnemy()
        {
            if (Time.time - _lastAttackTime <= _lastAttackCooldown) return false;

            _lastAttackTime = Time.time;
            _lastAttackCooldown = SuperAtackCooldown;
            AnimatorController.SetTrigger(SuperAttackId);

            if (_closestEnemy == null) return true;

            AttackEnemy(_closestEnemy, SuperDamage);
            return true;
        }


        private void AttackEnemy(Enemy enemy, int damage)
        {
            transform.transform.rotation =
                Quaternion.LookRotation(enemy.transform.position - transform.position);

            enemy.ReceiveDamage(damage);
        }

        private void Die()
        {
            _isDead = true;
            AnimatorController.SetTrigger(DieId);
            Destroy(_highlightParticles);

            SceneManager.Instance.GameOver();
        }
    }
}
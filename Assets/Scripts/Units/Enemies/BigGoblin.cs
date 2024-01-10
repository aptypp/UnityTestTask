using TestTask.Interfaces;
using UnityEngine;

namespace TestTask.Units.Enemies
{
    public class BigGoblin : Enemy, IExtraEnemiesSpawnable
    {
        [SerializeField]
        private Enemy[] _enemiesToSpawnAfterDeath;

        protected override void Die()
        {
            base.Die();

            for (int enemyIndex = 0; enemyIndex < _enemiesToSpawnAfterDeath.Length; enemyIndex++)
            {
                Enemy enemy = Instantiate(_enemiesToSpawnAfterDeath[enemyIndex],
                    transform.position + new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)),
                    Quaternion.identity);

                enemy.Initialize(SceneManager.Instance.Player.IncreaseHealth);
            }
        }
    }
}
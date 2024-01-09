using System.Collections.Generic;
using UnityEngine;

namespace TestTask
{
    public class SceneManager : MonoBehaviour
    {
        public static SceneManager Instance;

        public Player Player;
        public List<Enemie> Enemies;
        public GameObject Lose;
        public GameObject Win;

        [SerializeField]
        private LevelConfig Config;

        private int currWave;

        private void Awake()
        {
            Instance = this;
        }

        public void Reset()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

        private void Start()
        {
            SpawnWave();
        }

        public void AddEnemie(Enemie enemie)
        {
            Enemies.Add(enemie);
        }

        public void RemoveEnemie(Enemie enemie)
        {
            Enemies.Remove(enemie);
            if (Enemies.Count == 0) SpawnWave();
        }

        public void GameOver()
        {
            Lose.SetActive(true);
        }

        private void SpawnWave()
        {
            if (currWave >= Config.Waves.Length)
            {
                Win.SetActive(true);
                return;
            }

            Wave wave = Config.Waves[currWave];
            foreach (GameObject character in wave.Characters)
            {
                Vector3 pos = new(Random.Range(-10, 10), 0, Random.Range(-10, 10));
                Instantiate(character, pos, Quaternion.identity);
            }

            currWave++;
        }
    }
}
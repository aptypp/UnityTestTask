using System.Collections.Generic;
using Game.Input;
using TestTask.Ui;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInput = UnityEngine.InputSystem.PlayerInput;

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
        private HudManager _hudManager;

        [SerializeField]
        private LevelConfig Config;

        [SerializeField]
        private PlayerInput _playerInput;

        [SerializeField]
        private PlayerUiInput _playerUiInput;

        private int _currentWave;

        private const string _INPUT_ACTION_MOVE_NAME = "Move";

        private void Awake()
        {
            Instance = this;
            InitializePlayer();
            InitializeHud();
        }

        private void InitializeHud()
        {
            _hudManager.Initialize();
            _hudManager.playerHealthModel.Initialize(_hudManager.playerHealthText, Player);
        }

        private void Start() => SpawnWave();

        public void Reset() => UnityEngine.SceneManagement.SceneManager.LoadScene(0);

        public void AddEnemie(Enemie enemie) => Enemies.Add(enemie);

        public void RemoveEnemie(Enemie enemie)
        {
            Enemies.Remove(enemie);

            if (Enemies.Count > 0) return;

            if (_currentWave >= Config.Waves.Length)
            {
                Win.SetActive(true);
                return;
            }

            SpawnWave();
        }

        public void GameOver() => Lose.SetActive(true);

        private void InitializePlayer()
        {
            InputAction moveAction = _playerInput.actions.FindAction(_INPUT_ACTION_MOVE_NAME);

            moveAction.performed += Player.SetMoveDirection;
            moveAction.canceled += Player.SetMoveDirection;

            Player.findClosestEnemy += _playerUiInput.OnFoundClosestEnemy;
            Player.lostClosestEnemy += _playerUiInput.OnLostClosestEnemy;

            _playerUiInput.attackButton.onClick += Player.AttackInput;
            _playerUiInput.superAttackButton.SetOnClick(() =>
            {
                bool isAttacked = Player.SuperAttackInput();

                if (!isAttacked) return;

                _playerUiInput.superAttackButton.ToCooldownState(Player.SuperAtackCooldown,
                    _playerUiInput.superAttackButton.ToInactiveState);
            });
        }

        private void SpawnWave()
        {
            Wave wave = Config.Waves[_currentWave];

            for (int enemyIndex = 0; enemyIndex < wave.Characters.Length; enemyIndex++)
            {
                Enemie enemyPrefab = wave.Characters[enemyIndex];
                Vector3 pos = new(Random.Range(-10, 10), 0, Random.Range(-10, 10));
                Enemie instance = Instantiate(enemyPrefab, pos, Quaternion.identity);

                instance.Initialize(Player.IncreaseHealth);
            }

            _currentWave++;

            _hudManager.waveInfoText.text = $"Wave {_currentWave}/{Config.Waves.Length}";
        }
    }
}
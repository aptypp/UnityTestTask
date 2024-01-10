using TMPro;

namespace TestTask.Ui
{
    public class PlayerHealthModel
    {
        private int _playerStartHealth;
        private TMP_Text _playerHealthText;

        public void Initialize(TMP_Text playerHealthText, Player player)
        {
            _playerHealthText = playerHealthText;
            _playerStartHealth = player.startHealth;

            player.health.Changed += (health, _) => SetHealth(health);
            SetHealth(player.health.value);
        }

        private void SetHealth(int currentHealth) =>
            _playerHealthText.text = $"Health {currentHealth}/{_playerStartHealth}";
    }
}
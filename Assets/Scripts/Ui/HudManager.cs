using TMPro;
using UnityEngine;

namespace TestTask.Ui
{
    public class HudManager : MonoBehaviour
    {
        [field: SerializeField]
        public TMP_Text waveInfoText { get; private set; }

        [field: SerializeField]
        public TMP_Text playerHealthText { get; private set; }

        public PlayerHealthModel playerHealthModel { get; private set; }

        public void Initialize()
        {
            playerHealthModel = new PlayerHealthModel();
        }
    }
}
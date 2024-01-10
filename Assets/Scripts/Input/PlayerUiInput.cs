using TestTask.Ui;
using TestTask.Units;
using UnityEngine;

namespace TestTask.Input
{
    public class PlayerUiInput : MonoBehaviour
    {
        [field: SerializeField]
        public QuickButton attackButton { get; private set; }

        [field: SerializeField]
        public SuperAttackButton superAttackButton { get; private set; }
        
        public void OnFoundClosestEnemy(Enemie enemy)
        {
            if (superAttackButton.currentState == SuperAttackButton.State.Cooldown) return;

            superAttackButton.ToActiveState();
        }

        public void OnLostClosestEnemy()
        {
            if (superAttackButton.currentState == SuperAttackButton.State.Cooldown) return;

            superAttackButton.ToInactiveState();
        }
    }
}
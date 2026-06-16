using UnityEngine;
using JRPG.Core;

namespace JRPG.Combat
{
    // Mengatur alur pertarungan menggunakan State Pattern
    public class CombatManager : MonoBehaviour
    {
        private CombatState currentState;

        [Header("Combatants")]
        public PlayerEntity Player;
        public EnemyEntity Enemy;

        private void Start()
        {
            ChangeState(new CombatStartState(this));
        }

        private void Update()
        {
            currentState?.UpdateState();
        }

        // Berpindah dari satu state ke state lainnya secara aman
        public void ChangeState(CombatState newState)
        {
            currentState?.ExitState();
            currentState = newState;
            currentState?.EnterState();
        }
    }
}
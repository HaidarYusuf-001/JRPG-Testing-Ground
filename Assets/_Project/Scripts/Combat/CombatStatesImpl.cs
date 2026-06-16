using UnityEngine;

namespace JRPG.Combat
{
    // State saat pertarungan baru dimulai untuk proses inisialisasi
    public class CombatStartState : CombatState
    {
        public CombatStartState(CombatManager manager) : base(manager) { }

        public override void EnterState()
        {
            Debug.Log("Combat Started: Setting up battlefield.");

            // Transisi ke giliran pemain setelah setup selesai
            combatManager.ChangeState(new PlayerTurnState(combatManager));
        }
    }

    // State saat menunggu pemain memilih aksi dari UI
    public class PlayerTurnState : CombatState
    {
        public PlayerTurnState(CombatManager manager) : base(manager) { }

        public override void EnterState()
        {
            Debug.Log("Player's Turn: Waiting for input...");
            // Nanti event sistem akan di-trigger di sini untuk memunculkan UI
        }
    }

    // State saat AI musuh mengeksekusi aksinya
    public class EnemyTurnState : CombatState
    {
        public EnemyTurnState(CombatManager manager) : base(manager) { }

        public override void EnterState()
        {
            Debug.Log("Enemy's Turn: AI is thinking...");

            // Simulasi AI selesai berpikir dan mengembalikan giliran ke pemain
            combatManager.ChangeState(new PlayerTurnState(combatManager));
        }
    }
}
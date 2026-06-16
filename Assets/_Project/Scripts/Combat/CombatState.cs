namespace JRPG.Combat
{
    // Base class untuk semua state dalam combat
    public abstract class CombatState
    {
        protected CombatManager combatManager;

        public CombatState(CombatManager manager)
        {
            combatManager = manager;
        }

        public virtual void EnterState() { }
        public virtual void UpdateState() { }
        public virtual void ExitState() { }
    }
}
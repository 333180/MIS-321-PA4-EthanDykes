namespace PirateBattleGame
{
    public class TurnResult
    {
        public string AttackerName { get; }
        public string DefenderName { get; }
        public int Damage { get; }
        public string AttackType { get; }
        public bool WasCrit { get; }
        public bool WasMiss { get; }

        public TurnResult(string attackerName, string defenderName, int damage, string attackType, bool wasCrit, bool wasMiss)
        {
            AttackerName = attackerName;
            DefenderName = defenderName;
            Damage = damage;
            AttackType = attackType;
            WasCrit = wasCrit;
            WasMiss = wasMiss;
        }

        public override string ToString()
        {
            if (WasMiss)
                return $"{AttackerName} missed {DefenderName} with {AttackType}";
            
            if (WasCrit)
                return $"{AttackerName} CRIT {DefenderName} for {Damage} with {AttackType}";
            
            return $"{AttackerName} hit {DefenderName} for {Damage} with {AttackType}";
        }
    }
}
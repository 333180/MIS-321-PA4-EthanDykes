namespace PirateBattleGame.Attacks
{
    public class DistractAttack : IAttack
    {
        private readonly Random random = new Random();

        public (int damage, bool isCrit, bool isMiss) ExecuteAttack(int basePower)
        {
            // 1 in 20 chance to miss (5%)
            if (random.Next(1, 21) == 1)
            {
                return (0, false, true);
            }

            // 1 in 20 chance to crit (5%)
            if (random.Next(1, 21) == 1)
            {
                return (basePower * 5, true, false);
            }

            return (basePower, false, false);
        }

        public string GetAttackName()
        {
            return "Distract Opponent (Mental Damage)";
        }
    }
}
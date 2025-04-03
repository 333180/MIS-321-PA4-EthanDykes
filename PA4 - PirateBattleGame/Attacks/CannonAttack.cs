namespace PirateBattleGame.Attacks
{
    public class CannonAttack : IAttack
    {
        private readonly Random random = new Random();

        public int ExecuteAttack(int basePower)
        {
            const int damage = 15;
            
            // 1 in 20 chance to miss
            if (random.Next(1, 21) == 1)
            {
                return 0;
            }

            // 1 in 20 chance to crit
            if (random.Next(1, 21) == 1)
            {
                return damage * 5;
            }

            return damage;
        }

        public string GetAttackName()
        {
            return "Cannon Fire";
        }
    }
}
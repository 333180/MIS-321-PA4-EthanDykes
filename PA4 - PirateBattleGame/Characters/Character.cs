using PirateBattleGame.Attacks;
using System;

namespace PirateBattleGame.Characters
{
    public abstract class Character
    {
        public string Name { get; }
        public int MaxPower { get; private set; }  
        public int Health { get; private set; }
        public int AttackStrength { get; private set; }
        public int DefensivePower { get; private set; }
        protected IAttack AttackBehavior { get; }
        protected readonly Random Random = new Random();

        protected Character(string name, IAttack attackBehavior)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            AttackBehavior = attackBehavior ?? throw new ArgumentNullException(nameof(attackBehavior));
            Health = 100;
            GenerateStats();
        }

        protected virtual void GenerateStats()
        {
            MaxPower = Random.Next(1, 101);
            AttackStrength = Random.Next(1, MaxPower + 1);
            DefensivePower = Random.Next(1, MaxPower + 1);
        }

        public (int damage, bool isCrit, bool isMiss) PerformAttack(Character defender)
        {
            if (defender == null) throw new ArgumentNullException(nameof(defender));
            
            var (baseDamage, isCrit, isMiss) = AttackBehavior.ExecuteAttack(AttackStrength);
            
            if (isMiss)
            {
                return (0, false, true);
            }

            double typeBonus = CalculateTypeBonus(defender);
            int netDamage = (int)((baseDamage - defender.DefensivePower) * typeBonus);
            int finalDamage = Math.Max(1, netDamage);
            
            defender.TakeDamage(finalDamage);
            return (finalDamage, isCrit, false);
        }

        public void TakeDamage(int damage)
        {
            Health = Math.Max(0, Health - damage);
        }

        public string GetStats() => $"ATK: {AttackStrength} | DEF: {DefensivePower} | POW: {MaxPower}";

        public string GetHealthBar()
        {
            int bars = (int)Math.Round(Health / 10.0);
            return $"[{new string('█', bars)}{new string(' ', 10 - bars)}] {Health}%";
        }

        public string GetAttackName() => AttackBehavior.GetAttackName();

        public abstract double CalculateTypeBonus(Character defender);
    }
}
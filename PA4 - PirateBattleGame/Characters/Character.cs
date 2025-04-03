using PirateBattleGame.Attacks;
using System;

namespace PirateBattleGame.Characters
{
    public abstract class Character
    {
        public string Name { get; }
        private int MaxPower { get; set; }  
        public int Health { get; private set; }
        private int AttackStrength { get; set; }
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
        }

        public int PerformAttack()
        {
            return AttackBehavior.ExecuteAttack(AttackStrength);
        }

        public void TakeDamage(int damage)
        {
            Health = Math.Max(0, Health - damage);
        }

        public string GetStats() => $"ATK: {AttackStrength} | POW: {MaxPower} | HP: {Health}";
        
        public string GetAttackName() => AttackBehavior.GetAttackName();
        
        public string GetHealthBar()
        {
            int bars = (int)Math.Round(Health / 10.0);
            return $"[{new string('█', bars)}{new string(' ', 10 - bars)}] {Health}%";
        }

        public abstract double CalculateTypeBonus(Character defender);
    }
}
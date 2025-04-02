using PirateBattleGame.Attacks;

namespace PirateBattleGame.Characters
{
    public class WillTurner : Character
    {
        public WillTurner(string name) : base(name, new SwordAttack()) { }

        public override double CalculateTypeBonus(Character defender)
        {
            // Will beats Davy, loses to Jack
            return defender is DavyJones ? 1.2 : 1.0;
        }
    }
}
using PirateBattleGame.Attacks;

namespace PirateBattleGame.Characters
{
    public class JackSparrow : Character
    {
        public JackSparrow(string name) : base(name, new DistractAttack()) { }

        public override double CalculateTypeBonus(Character defender)
        {
            // Jack beats Will, loses to Davy
            return defender is WillTurner ? 1.2 : 1.0;
        }
    }
}
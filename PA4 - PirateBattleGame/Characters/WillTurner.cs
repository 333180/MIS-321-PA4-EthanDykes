using PirateBattleGame.Attacks;

namespace PirateBattleGame.Characters
{
    public class WillTurner : Character
    {
        public WillTurner(string name) : base(name, new SwordAttack()) { }

        public override double CalculateTypeBonus(Character defender)
        {
            return defender is DavyJones ? 1.2 : 
                   defender is JackSparrow ? 0.8 : 1.0;
        }
    }
}
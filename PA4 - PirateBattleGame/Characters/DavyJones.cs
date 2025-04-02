using PirateBattleGame.Attacks;

namespace PirateBattleGame.Characters
{
    public class DavyJones : Character
    {
        public DavyJones(string name) : base(name, new CannonAttack()) { }

        public override double CalculateTypeBonus(Character defender)
        {
            // Davy beats Jack, loses to Will
            return defender is JackSparrow ? 1.2 : 1.0;
        }
    }
}
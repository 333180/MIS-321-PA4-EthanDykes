using PirateBattleGame.Attacks;

namespace PirateBattleGame.Characters
{
    public class DavyJones : Character
    {
        public DavyJones(string name) : base(name, new CannonAttack()) { }

        public override double CalculateTypeBonus(Character defender)
        {
            return defender is JackSparrow ? 1.2 : 
                   defender is WillTurner ? 0.8 : 1.0;
        }
    }
}
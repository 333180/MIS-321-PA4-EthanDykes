using PirateBattleGame.Attacks;

namespace PirateBattleGame.Characters
{
    public class JackSparrow : Character
    {
        public JackSparrow(string name) : base(name, new DistractAttack()) { }

        public override double CalculateTypeBonus(Character defender)
        {
            return defender is WillTurner ? 1.2 : 
                   defender is DavyJones ? 0.8 : 1.0;
        }
    }
}
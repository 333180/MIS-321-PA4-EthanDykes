namespace PirateBattleGame.Attacks
{
    public interface IAttack
    {
        (int damage, bool isCrit, bool isMiss) ExecuteAttack(int basePower);
        string GetAttackName();
    }
}
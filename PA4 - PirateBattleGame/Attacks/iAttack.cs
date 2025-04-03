namespace PirateBattleGame.Attacks
{
    public interface IAttack
    {
        int ExecuteAttack(int basePower);
        string GetAttackName();
    }
}
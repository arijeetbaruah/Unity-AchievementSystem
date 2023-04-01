using System;

public interface ICommand
{
    void Execute();
}

public interface ICombatCommand
{
    void Execute(CharacterDetails target, CharacterStats characterStats, Action callback);
}

public abstract class BaseMagicCommand : ICombatCommand
{
    public abstract void Execute(CharacterDetails target, CharacterStats characterStats, Action callback);
}

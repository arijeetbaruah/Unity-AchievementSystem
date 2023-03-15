using Sirenix.OdinInspector;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField]
    private Stats stats;

    public Stats Stats => stats;

    public int CalculateDamage(int attack, int baseDmg = 0, int attackBonus = 1)
    {
        float numarator = attack * Constants.DmgConst * attackBonus;
        float denominator = stats.defense + Constants.DmgConst;

        float dmg = numarator / denominator;

        return baseDmg + Mathf.CeilToInt(dmg);
    }

    public int CalculateMagicDamage(int mAttack, int baseDmg = 0, int attackBonus = 1)
    {
        float numarator = mAttack * Constants.DmgConst * attackBonus;
        float denominator = stats.magicDefense + Constants.DmgConst;

        float dmg = numarator / denominator;

        return baseDmg + Mathf.CeilToInt(dmg);
    }
}

[System.Serializable]
public struct Stats
{
    [MinValue(1)]
    public int maxHP;
    [MinValue(1)]
    public int maxMana;
    [ProgressBar(0f, 100f)]
    public int attack;
    [ProgressBar(0f, 100f)]
    public int defense;
    [ProgressBar(0f, 100f)]
    public int magicAttack;
    [ProgressBar(0f, 100f)]
    public int magicDefense;
    [ProgressBar(0f, 100f)]
    public int speed;
}

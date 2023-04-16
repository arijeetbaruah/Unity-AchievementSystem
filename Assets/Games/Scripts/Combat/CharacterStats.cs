using Sirenix.OdinInspector;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField]
    private Stats stats;

    public Stats Stats => stats;
}

[System.Serializable]
public struct Stats
{
    //[MinValue(1)]
    public int maxHP;
    //[MinValue(1)]
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
    [ProgressBar(0f, 100f)]
    public int maxCharge;

    public static Stats operator +(Stats A, Stats B)
    {
        Stats result = new Stats();

        result.maxHP = A.maxHP + B.maxHP;
        result.maxMana = A.maxMana + B.maxMana;
        result.attack = A.attack + B.attack;
        result.defense = A.defense + B.defense;
        result.magicAttack = A.magicAttack + B.magicAttack;
        result.magicDefense = A.magicDefense + B.magicDefense;
        result.speed = A.speed + B.speed;
        result.maxCharge = A.maxCharge + B.maxCharge;

        return result;
    }

    public int CalculateDamage(int attack, int baseDmg = 0, int attackBonus = 1)
    {
        float numarator = attack * Constants.DmgConst * attackBonus;
        float denominator = defense + Constants.DmgConst;

        float dmg = numarator / denominator;

        return baseDmg + Mathf.CeilToInt(dmg);
    }

    public int CalculateMagicDamage(int mAttack, int baseDmg = 0, int attackBonus = 1)
    {
        float numarator = mAttack * Constants.DmgConst * attackBonus;
        float denominator = magicDefense + Constants.DmgConst;

        float dmg = numarator / denominator;

        return baseDmg + Mathf.CeilToInt(dmg);
    }
}

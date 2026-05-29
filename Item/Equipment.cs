using UnityEngine;
using System.Collections.Generic;

public class Equipment : Item
{

    public enum EquipType
    {
        Weapon,
        Helmet,
        Armor,
        Leggings,
        Boots,
    }

    protected EquipType equipType;
    protected Character.StatasType statasType;
    protected Dictionary<Character.StatasType, int> statas;

    public Equipment(string name, Skill s)
                : base(name, -1, s)
    {
        statas = new Dictionary<Character.StatasType, int>();

        statas[Character.StatasType.MaxHp] = 0;
        statas[Character.StatasType.MaxMp] = 0;
        statas[Character.StatasType.Atk] = 0;
        statas[Character.StatasType.Mgc] = 0;
        statas[Character.StatasType.Def] = 0;
        statas[Character.StatasType.Spd] = 0;
    }

    public EquipType getEquipType()
    {
        return equipType;
    }

    public int getStatas(Character.StatasType st)
    {
        return statas[st];
    }
}

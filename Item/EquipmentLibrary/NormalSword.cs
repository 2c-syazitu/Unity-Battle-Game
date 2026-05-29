using UnityEngine;

public class NormalSword : Equipment
{

    public NormalSword(BattleSystem sys)
        : base("NormalSword", null)
    {
        equipType = EquipType.Weapon;
        statas[Character.StatasType.Atk] = 10;
    }
}

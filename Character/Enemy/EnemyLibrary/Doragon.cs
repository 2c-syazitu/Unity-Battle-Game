using UnityEngine;

public class Doragon : Enemy
{

    public Doragon(string name, int hp, int mp, float atk, float mgc, float def, float spd)
        : base(name, hp, mp, atk, mgc, def, spd)
    {
        iamgeLink = "EnemyImage/Doragon";
        exp = 100;

        attributeDef[Skill.SkillAttribute.Fire] = 10;
    }

    public override void loadAlg()
    {

    }

}

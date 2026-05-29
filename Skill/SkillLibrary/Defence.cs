using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Defence : Skill
{
    List<BuffEffect> list;
    const string iamgeLink = "BuffImage/Shield";

    public Defence(BattleSystem sys)
                /*               cost,baseDmg,(float)rate,(bool)targetAlive*/
                : base("defence", 0, 0, 1, true, sys, Skill.SkillTarget.Myself,
                 Skill.SkillRange.Single, Skill.SkillCastTurn.Quick)
    {
        list = new List<BuffEffect>();
        list.Add(new(Character.StatasType.Def, Buff.BuffCalculationType.Percent, 1.5f));

        detail = $"def * 1.5";
        // attributeList.Add(SkillAttribute.None);
    }

    public override void activate(Character user, List<Character> targetList)
    {
        for (int i = 0; i < targetList.Count; i++)
        {
            user.setBuff(new Buff(list, 1, iamgeLink, user, detail, this, user));
            sys.setTextLabel($"{user.getName()} is defence.");
        }
    }

}

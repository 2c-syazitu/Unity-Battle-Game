using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Attack : Skill
{

    public Attack(BattleSystem sys)
                /*               cost,baseDmg,(float)rate,(bool)targetAlive*/
                : base("attack", 0, 0, 1, true, sys, Skill.SkillTarget.Enemy,
                Skill.SkillRange.Single, Skill.SkillCastTurn.Quick)
    {
        // attributeList.Add(SkillAttribute.None);
        rateList[Character.StatasType.Atk] = 1;
    }

    public override void activate(Character user, List<Character> targetList)
    {
        for (int i = 0; i < targetList.Count; i++)
        {
            float dmg = CalcDmg.calcDmg(this, user, targetList[i]);
            targetList[i].setDmg(dmg);
            sys.setTextLabel(
                $"{user.getName()} attack to {targetList[i].getName()}!! \n" +
                $"{targetList[i].getName()} damaged {(int)dmg}");
        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Heal : Skill
{

    public Heal(BattleSystem sys)
                : base("heal", 10, 10, 10, true, sys, Skill.SkillTarget.Ally,
                Skill.SkillRange.Single, Skill.SkillCastTurn.Quick)
    {
        // attributeList.Add(SkillAttribute.None);
    }

    public override void activate(Character user, List<Character> targetList)
    {
        base.activate(user, targetList);
        float heal = CalcDmg.calcSkillInt(this, user);
        for (int i = 0; i < targetList.Count; i++)
        {
            targetList[i].setDmg(-(int)heal);
            sys.setTextLabel($"{user.getName()} heal to {targetList[i].getName()}!! \n" +
                             $"{targetList[i].getName()}'s HP is {targetList[i].getNowHp()}");
        }
    }

    public override string getSkillText()
    {
        return "heal target";
    }
}
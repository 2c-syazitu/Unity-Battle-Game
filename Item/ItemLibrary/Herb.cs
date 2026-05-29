using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Herb : Item
{

    public Herb(BattleSystem sys)
                : base("herb", 1, new HerbSkill(sys))
    {

    }

    public override string getItemText()
    {
        return "heal";
    }


    class HerbSkill : Skill
    {

        public HerbSkill(BattleSystem sys)
                    : base("herb", 0, 30, 0, true, sys, Skill.SkillTarget.Ally,
                    Skill.SkillRange.Single, Skill.SkillCastTurn.Quick)
        {
            attributeList.Add(SkillAttribute.None);
        }

        public override void activate(Character user, List<Character> targetList)
        {
            float val = CalcDmg.calcSkillInt(this, user);
            for (int i = 0; i < targetList.Count; i++)
            {
                Debug.Log($"hb29,tli.name{targetList[i].getName()}  user.name{user.getName()}");

                targetList[i].setDmg(-(int)val);
            }
        }
    }
}


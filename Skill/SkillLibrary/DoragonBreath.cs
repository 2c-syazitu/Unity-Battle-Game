using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoragonBreath : Skill
{
    int castTurn;
    const string imageLink = "BuffImage/DoragonBreath";

    public DoragonBreath(BattleSystem sys)
                /*               cost,baseDmg,(float)rate,(bool)targetAlive*/
                : base("doragonBreath", 10, 0, 1, true, sys, Skill.SkillTarget.Enemy,
                 Skill.SkillRange.Area, Skill.SkillCastTurn.Charge)
    {
        castTurn = 0;
        detail = "took a breath";
        attributeList.Add(SkillAttribute.Fire);
    }

    public override void activate(Character user, List<Character> targetList)
    {
        if (castTurn == 0)
        {
            sys.setTextLabel("took a breath");
            /*アイコン用のからのバフ*/
            user.setBuff(new Buff(new List<BuffEffect>(), 1, imageLink, user, detail, this, user));
            castTurn++;
        }
        else if (castTurn == 1)
        {
            base.activate(user, targetList);
            sys.setTextLabel("doragonBreath");

            for (int i = 0; i < targetList.Count; i++)
            {
                float dmg = CalcDmg.calcDmg(this, user, targetList[i]);
                targetList[i].setDmg(dmg);
                Debug.Log($"db16:{user.getName()} breath to {targetList[i].getName()} damaged {(int)dmg}");
            }
            castTurn = 0;
            user.clearExecute();
        }


    }

    public override string getSkillText()
    {
        return "doragonBreath";
    }

}

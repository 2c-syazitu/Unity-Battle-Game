using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Poison : Skill
{
    List<BuffEffect> list;
    const string iamgeLink = "BuffImage/Poison";
    int turn;

    public Poison(BattleSystem sys)
    /*               cost,baseDmg,(float)rate,(bool)targetAlive*/
                : base("poison", 10, 5, 0.2f, true, sys, Skill.SkillTarget.Enemy,
                 Skill.SkillRange.Single, Skill.SkillCastTurn.Quick)
    {
        list = new List<BuffEffect>();
        list.Add(new(Character.StatasType.None, Buff.BuffCalculationType.Flat, 10));
        turn = 2;
        // attributeList.Add(SkillAttribute.None);
    }

    public override void activate(Character user, List<Character> targetList)
    {
        base.activate(user, targetList);
        float val = baseDmg + user.getAtk() * rate;
        detail = $"{(int)val} damage by turn";

        System.Action<Character, float, BattleSystem> act = (target, val, sys) =>
        {
            sys.setTextLabel($"{target.getName()} damaged {(int)val} by poison");
            target.setDmg(val);
        };

        for (int i = 0; i < targetList.Count; i++)
        {
            Character target = targetList[i];
            Buff b = target.getBuffList().FirstOrDefault(buff => buff.sameResource(this, user));
            if (b == null)
            {
                Buff poison = new ExtraBuff(list, turn, iamgeLink, targetList[i],
                                    act, sys, detail, this, user);
                targetList[i].setBuff(poison);
            }
            else
            {
                b.setTurn(turn);
            }
        }

    }

    public override string getSkillText()
    {
        return "damage to the target each turn";
    }

}

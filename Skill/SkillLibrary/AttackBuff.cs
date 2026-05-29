using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AttackBuff : Skill
{
    List<BuffEffect> list;
    const string iamgeLink = "BuffImage/Power";
    int turn;

    public AttackBuff(BattleSystem sys)
                /*               cost,baseDmg,(float)rate,(bool)targetAlive*/
                : base("attackBuff", 10, 5, 0.2f, true, sys, Skill.SkillTarget.Ally,
                 Skill.SkillRange.Single, Skill.SkillCastTurn.Quick)
    {
        turn = 2;
        list = new List<BuffEffect>();
        list.Add(new(Character.StatasType.Atk, Buff.BuffCalculationType.Flat, 10));
    }

    public override void activate(Character user, List<Character> targetList)
    {
        base.activate(user, targetList);
        float buff = baseDmg + user.getAtk() * rate;
        detail = $"atk + {(int)buff}";

        for (int i = 0; i < targetList.Count; i++)
        {
            /*自バフの場合次のターンからバフを開始させる*/
            if (user == targetList[i])
            {
                turn++;
            }
            Character target = targetList[i];
            Buff b = target.getBuffList().FirstOrDefault(buff => buff.sameResource(this, user));
            if (b == null)
            {
                targetList[i].setBuff(
                    new Buff(list, turn, iamgeLink, targetList[i], detail, this, user));
            }
            else
            {
                b.setTurn(turn);
            }

        }
    }

    public override string getSkillText()
    {
        return "upper target atk";
    }
}

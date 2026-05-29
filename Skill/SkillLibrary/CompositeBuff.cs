using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class CompositeBuff : Skill
{
    List<BuffEffect> list;
    const string iamgeLink = "BuffImage/SwordAndShield";
    int turn;

    public CompositeBuff(BattleSystem sys)
                /*               cost,baseDmg,(float)rate,(bool)targetAlive*/
                : base("compositeBuff", 100, 5, 0.2f, true, sys,
                Skill.SkillTarget.Ally, Skill.SkillRange.Single,
                Skill.SkillCastTurn.Quick)
    {
        list = new List<BuffEffect>();
        turn = 3;
        detail = $"atk + {10}\n" +
                 $"def + 10\n" +
                 $"def + 10\n" +
                 $"def + 10";


        list.Add(new(Character.StatasType.Atk, Buff.BuffCalculationType.Percent, 1.1f));
        list.Add(new(Character.StatasType.Def, Buff.BuffCalculationType.Flat, 10));
        list.Add(new(Character.StatasType.Def, Buff.BuffCalculationType.Flat, 10));
        list.Add(new(Character.StatasType.Def, Buff.BuffCalculationType.Flat, 100));

        // attributeList.Add(SkillAttribute.None);
    }

    public override void activate(Character user, List<Character> targetList)
    {
        base.activate(user, targetList);

        for (int i = 0; i < targetList.Count; i++)
        {
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

            string txt = $"{targetList[i].getName()} filled with energy!!\n" +
                         $"{targetList[i].getName()} increased defence power!!";

            sys.setTextLabel(txt);
        }
    }

    public override string getSkillText()
    {
        return "upper target attack and defence";
    }

}

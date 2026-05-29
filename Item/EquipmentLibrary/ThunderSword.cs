using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThunderSword : Equipment
{

    public ThunderSword(BattleSystem sys)
        : base("ThunderSword", new ThunderSwordSkill(sys))
    {
        equipType = EquipType.Weapon;
        statas[Character.StatasType.Atk] = 5;
        statas[Character.StatasType.Mgc] = 5;
    }

    class ThunderSwordSkill : Skill
    {
        public ThunderSwordSkill(BattleSystem sys)
            : base("ThunderSword", 0, 1000, 10, true, sys, Skill.SkillTarget.Enemy,
            Skill.SkillRange.Area, Skill.SkillCastTurn.Quick)
        {
            attributeList.Add(SkillAttribute.Tunder);
        }

        public override void activate(Character user, List<Character> targetList)
        {
            Debug.Log($"ts27{targetList.Count}");
            for (int i = 0; i < targetList.Count; i++)
            {
                float val = CalcDmg.calcDmg(this, user, targetList[i]);
                Debug.Log($"ts30{val}");
                targetList[i].setDmg(val);
            }
        }
    }
}

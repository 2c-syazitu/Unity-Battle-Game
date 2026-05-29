using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class Skill
{

    protected string name;
    protected int cost;
    protected int baseDmg;
    protected float rate;
    protected Dictionary<Character.StatasType, float> rateList;
    protected BattleSystem sys;
    protected SkillTarget skillTarget;
    protected SkillRange range;
    protected SkillCastTurn castTurn;
    protected bool targetAlive;
    protected string detail;
    protected float val;
    protected List<SkillAttribute> attributeList;
    protected Item item;


    public enum SkillTarget
    {
        Myself,
        Ally,
        Enemy,
    }

    public enum SkillRange
    {
        Single,
        Area,
    }

    public enum SkillCastTurn
    {
        Quick,
        Charge,
    }

    public enum SkillAttribute
    {
        Fire,
        Water,
        Ice,
        Ground,
        Wind,
        Tunder,
        Light,
        Darkness,
        None,
    }

    public Skill(string name, int cost, int baseDmg, float rate, bool targetAlive,
                 BattleSystem sys, SkillTarget skillTarget, SkillRange range,
                 SkillCastTurn castTurn)
    {
        this.name = name;
        this.cost = cost;
        this.baseDmg = baseDmg;
        this.rate = rate;
        rateList = new Dictionary<Character.StatasType, float>();
        this.targetAlive = targetAlive;
        this.sys = sys;
        this.skillTarget = skillTarget;
        this.range = range;
        this.castTurn = castTurn;
        attributeList = new List<SkillAttribute>();

        rateList[Character.StatasType.MaxHp] = 0;
        rateList[Character.StatasType.MaxMp] = 0;
        rateList[Character.StatasType.Atk] = 0;
        rateList[Character.StatasType.Mgc] = 0;
        rateList[Character.StatasType.Def] = 0;
        rateList[Character.StatasType.Spd] = 0;
    }

    public virtual void activate(Character user, List<Character> targetList)
    {
        user.useMp(cost);
    }

    public string getName()
    {
        return name;
    }

    public int getCost()
    {
        return cost;
    }

    public int getBaseDmg()
    {
        return baseDmg;
    }

    public float getRate()
    {
        return rate;
    }

    public Dictionary<Character.StatasType, float> getRateList()
    {
        return rateList;
    }

    public bool getTargetAlive()
    {
        return targetAlive;
    }

    public SkillTarget getTarget()
    {
        return skillTarget;
    }

    public SkillRange getRange()
    {
        return range;
    }

    public SkillCastTurn getCastTurn()
    {
        return castTurn;
    }

    public virtual string getSkillText()
    {
        return null;
    }

    public List<SkillAttribute> getAttributeList()
    {
        return attributeList;
    }

    public Item getItem()
    {
        if (item == null)
        {
            return null;
        }
        else
        {
            return item;
        }
    }

    /*アイテムスキルのときだけ動く*/
    public void setItem(Item i)
    {
        item = i;
    }
}

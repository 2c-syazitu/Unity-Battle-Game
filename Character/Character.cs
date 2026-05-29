using UnityEngine;
using System.Collections.Generic;

public class Character
{
    public enum CharacterType
    {
        Player,
        Enemy,
    }

    public enum CharacterAttribute
    {
        Fire,
        Water,
        Ice,
        Ground,
        Wind,
        Light,
        Darkness,
        None,
    }

    public enum StatasType
    {
        MaxHp,
        MaxMp,
        Atk,
        Mgc,
        Def,
        Spd,
        None,
    }

    protected int lv;
    protected string name;
    protected int baseMaxHp;
    protected int maxHp;
    protected int baseMaxMp;
    protected int nowHp;
    protected int maxMp;
    protected int nowMp;
    protected float baseAtk;
    protected float atk;
    protected float baseMgc;
    protected float mgc;
    protected float baseDef;
    protected float def;
    protected Dictionary<Skill.SkillAttribute, int> attributeDef;
    protected float baseSpd;
    protected float spd;
    protected bool isAlive; /*生死の判定*/
    protected List<Skill> skillList;
    protected List<Buff> buffList;
    protected Skill executeSkill;
    protected List<Character> targetList;
    protected List<Item> itemList;
    protected Dictionary<Equipment.EquipType, Equipment> equipList;

    protected BattleSystem sys;
    protected CharacterUI ui;
    protected CharacterType type;





    public Character(string name, int hp, int mp, float atk, float mgc, float def, float spd,
                     CharacterType type, BattleSystem sys)
    {
        this.name = name;
        this.baseMaxHp = hp;
        this.nowHp = hp;
        this.baseMaxMp = mp;
        this.nowMp = mp;
        this.baseAtk = atk;
        this.baseMgc = mgc;
        this.baseDef = def;
        this.baseSpd = spd;
        this.type = type;
        this.sys = sys;
        isAlive = true;
        executeSkill = null;
        targetList = new List<Character>();
        attributeDef = new Dictionary<Skill.SkillAttribute, int>();
        equipList = new Dictionary<Equipment.EquipType, Equipment>();

        skillList = new List<Skill>();
        buffList = new List<Buff>();
        itemList = new List<Item>();


        /*属性体制のセット(-10 ~ 10)*/
        attributeDef[Skill.SkillAttribute.Fire] = 0;
        attributeDef[Skill.SkillAttribute.Water] = 0;
        attributeDef[Skill.SkillAttribute.Ice] = 0;
        attributeDef[Skill.SkillAttribute.Ground] = 0;
        attributeDef[Skill.SkillAttribute.Wind] = 0;
        attributeDef[Skill.SkillAttribute.Tunder] = 0;
        attributeDef[Skill.SkillAttribute.Light] = 0;
        attributeDef[Skill.SkillAttribute.Darkness] = 0;


        /*テスト*/
        skillList.Add(new Heal(sys));
        skillList.Add(new AttackBuff(sys));
        skillList.Add(new DoragonBreath(sys));
        skillList.Add(new CompositeBuff(sys));
        skillList.Add(new Poison(sys));

        itemList.Add(new Herb(sys));

        /*装備のステータスを計算*/
        calcStatas();
    }

    /*ターン開始処理*/
    public virtual void startTurn()
    {

    }

    /*ターン終了処理*/
    public virtual void finishTurn()
    {

    }

    public virtual void showTargetPreview(bool tf)
    {

    }

    /*ゲッター*/
    public virtual CharacterUI getUI()
    {
        return ui;
    }

    public string getName()
    {
        return name;
    }

    public int getMaxHp()
    {
        return maxHp;
    }

    public int getNowHp()
    {
        return nowHp;
    }

    public int getMaxMp()
    {
        return maxMp;
    }

    public int getNowMp()
    {
        return nowMp;
    }

    public float getBaseAtk()
    {
        return baseAtk;
    }

    public float getAtk()
    {
        return atk;
    }

    public float getBaseMgc()
    {
        return baseMgc;
    }

    public float getMgc()
    {
        return mgc;
    }

    public float getBaseSpd()
    {
        return baseSpd;
    }

    public float getDef()
    {
        return def;
    }

    public float getSpd()
    {
        return spd;
    }

    public float getStatas(StatasType type)
    {
        float f = 0;
        if (type == StatasType.Atk)
        {
            f = getAtk();
        }
        else if (type == StatasType.Mgc)
        {
            f = getMgc();
        }
        else if (type == StatasType.Def)
        {
            f = getDef();
        }

        else if (type == StatasType.Spd)
        {
            f = getSpd();
        }
        return f;
    }

    public bool getAlive()
    {
        return isAlive;
    }

    public List<Skill> getSkillList()
    {
        return skillList;
    }

    public List<Buff> getBuffList()
    {
        return buffList;
    }

    public List<Item> getItemList()
    {
        return itemList;
    }

    public CharacterType getType()
    {
        return type;
    }

    public Skill getExecuteSkill()
    {
        return executeSkill;
    }

    public Dictionary<Equipment.EquipType, Equipment> getEquipList()
    {
        return equipList;
    }

    public List<Character> getTargetList()
    {
        Debug.Log($"ch159:{targetList.Count}");
        return targetList;
    }

    public Dictionary<Skill.SkillAttribute, int> getAtrList()
    {
        return attributeDef;
    }

    public bool getInteractableAnime()
    {
        return ui.getInteractableAnime();
    }


    /*セッター*/
    public virtual void setUI(CharacterUI ui)
    {
        this.ui = ui;
        ui.setSys(sys);
    }

    public void setMaxHp(int i)
    {
        maxHp = i;
        calcStatas();
    }

    public virtual void setNowHp(int i, bool tf)
    {
        nowHp = i;
        if (nowHp <= 0)
        {
            /*死んでしまうとはなさけない*/
            nowHp = 0;
            isAlive = false;
        }
        else if (maxHp < nowHp)
        {
            /*最大値を超えない*/
            nowHp = maxHp;
        }
    }

    /*ダメージのセット*/
    public void setDmg(float i)
    {
        if (i > 0)
        {
            this.setNowHp(getNowHp() - (int)i, true);
        }
        else if (i < 0)
        {
            this.setNowHp(getNowHp() - (int)i, false);
        }

    }

    public void setMaxMp(int i)
    {
        maxMp = i;
        calcStatas();
    }

    public void setNowMp(int i)
    {
        nowMp = i;
        if (i <= 0)
        {
            /*0未満にならない*/
            nowMp = 0;
        }
        else if (maxMp < i)
        {
            /*最大値を超えない*/
            nowMp = maxMp;
        }
    }

    public virtual void useMp(int i)
    {
        nowMp -= i;
        if (nowMp <= 0)
        {
            /*0未満にならない*/
            nowMp = 0;
        }
        else if (maxMp < i)
        {
            /*最大値を超えない*/
            nowMp = maxMp;
        }
    }

    public void setAtk(float i)
    {
        baseAtk = i;
        calcStatas();
    }

    public void setMgc(float i)
    {
        baseMgc = i;
        calcStatas();
    }

    public void setDef(float i)
    {
        baseDef = i;
        calcStatas();
    }

    public void setSpd(float i)
    {
        baseSpd = i;
        calcStatas();
    }

    /*ステータスタイプに応じてセット*/
    public void setStatas(StatasType type, int i)
    {
        if (type == StatasType.Atk)
        {
            setAtk(i);
        }
        else if (type == StatasType.Mgc)
        {
            setMgc(i);
        }
        else if (type == StatasType.Def)
        {
            setDef(i);
        }

        else if (type == StatasType.Spd)
        {
            setSpd(i);
        }
    }

    public void changeAlive()
    {
        /*生死を反転*/
        isAlive = !isAlive;
    }

    public void setSkill(Skill s)
    {
        skillList.Add(s);
    }

    public void setExecute(Skill s)
    {
        executeSkill = s;

    }

    public void setEquipment(Equipment e)
    {
        itemList.Add(e);
        equipList[e.getEquipType()] = e;
        calcStatas();
    }

    /*装備のステータスを計算*/
    public void calcStatas()
    {
        /*初期化*/
        maxHp = maxMp = 0;
        atk = mgc = def = spd = 0;

        /*ステータスの和*/
        foreach (Equipment e in equipList.Values)
        {
            maxHp += e.getStatas(StatasType.MaxHp);
            maxMp += e.getStatas(StatasType.MaxMp);
            atk += e.getStatas(StatasType.Atk);
            mgc += e.getStatas(StatasType.Mgc);
            def += e.getStatas(StatasType.Def);
            spd += e.getStatas(StatasType.Spd);
        }

        /*キャラクターのベースのステータスを計算*/
        maxHp += baseMaxHp;
        maxMp += baseMaxMp;
        atk += baseAtk;
        mgc += baseMgc;
        def += baseDef;
        spd += baseSpd;
    }

    public void setTargetList(List<Character> targetList)
    {
        Debug.Log($"ch260:{targetList.Count}");
        this.targetList = new List<Character>(targetList);
    }

    public void clearExecute()
    {
        targetList.Clear();
        executeSkill = null;
    }

    public virtual void setBuff(Buff b)
    {
        buffList.Add(b);
    }

    public void setItem(Item i)
    {
        itemList.Add(i);
    }

    public void delItem(Item i)
    {
        itemList.Remove(i);
    }
}

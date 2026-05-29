using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Item
{

    string name;
    int count;
    Skill skill;

    public Item(string name, int count, Skill skill)
    {
        this.name = name;
        this.count = count;
        this.skill = skill;

        /*アイテムとスキルを紐づける*/
        if (skill != null)
        {
            skill.setItem(this);
        }
    }

    /*アイテムの使用*/
    public void useItem(Character user)
    {
        /*カウント減少
        　なくなればリストから削除*/
        count--;
        if (count == 0)
        {
            user.delItem(this);
        }
    }


    /*ゲッター*/
    public string getName()
    {
        return name;
    }

    public int getCount()
    {
        return count;
    }

    public Skill getSkill()
    {
        return skill;
    }

    public virtual string getItemText()
    {
        return null;
    }

}

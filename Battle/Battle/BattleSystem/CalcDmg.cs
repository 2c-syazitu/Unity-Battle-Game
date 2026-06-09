using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public static class CalcDmg
{
    /*割合バフはキャラクターの攻撃力のみに積算
    **整数バフの加算前に割合計算
    **(元の数値*割合バフ+数値バフ)
    */

    /*スキルが与える軽減前ダメージ*/
    public static float calcSkillInt(Skill s, Character c)
    {
        List<Buff> buffList = c.getBuffList();
        float baseDmg = s.getBaseDmg();
        float dmg = 0;
        Dictionary<Character.StatasType, float> rateList = s.getRateList();

        /*スキルの持つレイトリストを順に参照*/
        foreach (var (statasType, rate) in rateList)
        {
            /*ステータスを代入*/
            float statas = c.getStatas(statasType);
            Debug.Log($"cd35:statasType={statasType} , rateInt={rate} , statas={statas}");
            /*割合バフの合計値の変数*/
            float buffPersent = 1;
            /*実数バフの合計値の変数*/
            float buffFlat = 0;

            /*バフリストを順に参照*/
            for (int i = 0; i < buffList.Count; i++)
            {
                /*バフが持っている効果を取得*/
                List<BuffEffect> effectList = buffList[i].getEffectList();

                for (int j = 0; j < effectList.Count; j++)
                {
                    /*バフの効果が参照ステータスと一致すれば*/
                    if (effectList[j].getBuffType() == statasType)
                    {
                        /*割合バフの計算*/
                        if (effectList[j].getBuffCalculationType() == Buff.BuffCalculationType.Percent)
                        {
                            buffPersent *= effectList[j].getValue();
                        }
                        /*実数バフの計算*/
                        else if (effectList[j].getBuffCalculationType() == Buff.BuffCalculationType.Flat)
                        {
                            buffFlat += effectList[j].getValue();
                        }
                    }
                }
            }
            Debug.Log($"cd60: dmg = {dmg} , buffPersent = {buffPersent} , buffFlat = {buffFlat} , rateInt = {rate}");
            /*ダメージ計算*/
            dmg += (statas * buffPersent + buffFlat) * rate;
            Debug.Log($"cd62: dmg = {dmg}");
        }
        /*基本ダメージの計算*/
        dmg += baseDmg;
        return dmg;
    }

    /*バフを計算しないスキルのダメージ計算*/
    public static float calcIgnoreBuffDmg(Skill s, Character c)
    {
        float dmg = 0;
        float baseDmg = s.getBaseDmg();
        Dictionary<Character.StatasType, float> rateList = s.getRateList();

        foreach (var (statasType, rate) in rateList)
        {
            float statas = c.getStatas(statasType);
            dmg += rate * statas;
        }
        dmg += baseDmg;
        return dmg;
    }

    /*キャラクターの防御力のダメージ軽減率(%)*/
    public static float calcDefRate(Skill skill, Character c)
    {
        float rate;

        float def = c.getDef();
        List<Buff> buffList = c.getBuffList();

        /*防御バフの計算
        **(元の数値*割合バフ+数値バフ)
        */

        /*防御系の割合積算バフの計算*/
        for (int i = 0; i < buffList.Count; i++)
        {
            /*バフの詳細リストを代入*/
            List<BuffEffect> effectList = buffList[i].getEffectList();
            Debug.Log($"cd19:effectList.size,{effectList.Count}");
            for (int j = 0; j < effectList.Count; j++)
            {
                /*バフが防御バフかつ割合積算*/
                if ((effectList[j].getBuffType() == Character.StatasType.Def) &&
                   (effectList[j].getBuffCalculationType() == Buff.BuffCalculationType.Percent))
                {
                    def *= effectList[j].getValue();
                    Debug.Log($"cd29:atk,{def}");
                }
            }
        }

        /*防御系の割合積算バフの計算*/
        for (int i = 0; i < buffList.Count; i++)
        {
            /*バフの詳細リストを代入*/
            List<BuffEffect> effectList = buffList[i].getEffectList();
            Debug.Log($"cd19:effectList.size,{effectList.Count}");
            for (int j = 0; j < effectList.Count; j++)
            {
                /*バフが防御バフかつ割合積算*/
                if ((effectList[j].getBuffType() == Character.StatasType.Def) &&
                   (effectList[j].getBuffCalculationType() == Buff.BuffCalculationType.Flat))
                {
                    def *= effectList[j].getValue();
                    Debug.Log($"cd29:atk,{def}");
                }
            }
        }

        if (c.getDef() >= 0)
        {
            /*lim(def->∞)で0に収束*/
            rate = (float)100 / (100 + def);
        }
        else
        {
            /*lim(def->-∞)で2に収束*/
            rate = (float)2 - (100 / (100 - def));
        }

        /*属性防御を計算*/
        List<Skill.SkillAttribute> list = skill.getAttributeList();
        float atrDef = 0;
        /*スキルに対応する耐性を合算*/
        for (int i = 0; i < list.Count; i++)
        {
            atrDef -= c.getAtrList()[list[i]];
        }

        atrDef = atrDef * 0.03f + 1;

        /*軽減率に属性軽減率をかける*/
        rate *= atrDef;

        return rate;
    }

    /*ダメージの計算*/
    public static float calcDmg(Skill exeSkill, Character user, Character target)
    {
        float dmg = calcSkillInt(exeSkill, user) * calcDefRate(exeSkill, target);
        return dmg;
    }

}

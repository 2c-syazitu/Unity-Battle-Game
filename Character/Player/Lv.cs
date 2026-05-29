using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Lv", menuName = "Scriptable Objects/Lv")]
public class Lv : ScriptableObject
{
    /*レベルアップに必要な経験値*/
    public int[] requiredExp;
    /*レベルに応じた基礎ステータスリスト*/
    public int[] hpListForLv;
    public int[] mpListForLv;
    public int[] aktListForLv;
    public int[] mgcListForLv;
    public int[] defListForLv;
    public int[] spdListForLv;

    /*ステータスタイプをキー、ステータスリストをバリューとしたディクショナリ*/
    public Dictionary<Character.StatasType, int[]> statasList;

    public void OnEnable()
    {
        statasList = new Dictionary<Character.StatasType, int[]>();

        statasList[Character.StatasType.MaxHp] = hpListForLv;
        statasList[Character.StatasType.MaxMp] = mpListForLv;
        statasList[Character.StatasType.Atk] = aktListForLv;
        statasList[Character.StatasType.Mgc] = mgcListForLv;
        statasList[Character.StatasType.Def] = defListForLv;
        statasList[Character.StatasType.Spd] = spdListForLv;
    }
}

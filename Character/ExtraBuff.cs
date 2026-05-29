using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExtraBuff : Buff
{
    private System.Action<Character, float, BattleSystem> activate;
    private BattleSystem sys;

    public ExtraBuff(List<BuffEffect> list, int turn, string imageLink,
                     Character target, System.Action<Character, float, BattleSystem> activate,
                      BattleSystem sys, string detail, Skill s, Character c)
              : base(list, turn, imageLink, target, detail, s, c)
    {
        this.activate = activate;
        this.sys = sys;
    }

    public override bool changeTurn()
    {
        /*ターンを消費するタイミングで実行*/
        for (int i = 0; i < list.Count; i++)
        {
            activate?.Invoke(target, list[i].getValue(), sys);
        }

        return base.changeTurn();
    }
}
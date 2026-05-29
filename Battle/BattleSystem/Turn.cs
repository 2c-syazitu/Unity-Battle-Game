using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Turn
{

    private int round; /*各キャラクターが1ターン消費したら1ラウンド*/
    private int turn; /*各キャラクターの1回の行動*/
    private List<Character> sortedSpd;

    public Turn(List<Character> charaList)
    {
        round = 1;
        turn = 0;
        sortedSpd = sortSpd(charaList);

        for (int i = 0; i < sortedSpd.Count; i++)
        {
            Debug.Log($"tu19;{sortedSpd[i].getName()}");
        }
    }

    public void changeTurn()
    {
        /*ターンの終了処理*/
        getTurnHolder().finishTurn();

        turn++;
        if (turn == sortedSpd.Count)
        {
            turn = 0;
            round++;
            sortSpd(sortedSpd);
        }
        /*死んでるキャラクターはスキップ*/
        if (!getTurnHolder().getAlive())
        {
            changeTurn();
        }

        /*ターンの開始処理*/
        getTurnHolder().startTurn();
    }

    public List<Character> sortSpd(List<Character> list)
    {
        /*素早さでソート*/
        list.Sort((a, b) => b.getSpd().CompareTo(a.getSpd()));
        /*同じ素早さの場合ラウンドごとにランダムに並び替え*/

        /*同じ素早さの要素の仮リスト*/
        List<Character> samaSpdList = new List<Character>();

        int i = 0, j = 1;
        while (i < list.Count - 1)
        {
            /*ある要素とその次が同じ素早さ*/
            if (list[i].getSpd() == list[i + j].getSpd())
            {
                samaSpdList.Add(list[i]);
                /*同じ素早さが続く限りループ*/
                while (list[i].getSpd() == list[i + j].getSpd())
                {
                    samaSpdList.Add(list[i + j]);
                    /*元のリストサイズを超えた時点で終了*/
                    if (list.Count - 1 == i + j)
                    {
                        break;
                    }
                    j++;
                }
                /*ランダムな数字を格納*/
                List<int> radomList = new List<int>();

                /*samaSpdListの要素数分ランダムな数字を生成
                  生成したランダムな数字の要素番号のsamaSpdListの要素を
                  もとのリストに順番に格納
                */
                int k = 0;
                while (samaSpdList.Count != radomList.Count)
                {
                    int randomInt = Random.Range(0, samaSpdList.Count);

                    /*生成した数字は被らないようにする*/
                    if (!radomList.Contains(randomInt))
                    {
                        radomList.Add(randomInt);
                        list[i + k] = samaSpdList[randomInt];
                        k++;
                    }
                }
            }
            i += j;
            j = 1;
        }
        return list;


    }

    public Character getTurnHolder()
    {
        return sortedSpd[turn];
    }

    public int getRound()
    {
        return round;
    }
}

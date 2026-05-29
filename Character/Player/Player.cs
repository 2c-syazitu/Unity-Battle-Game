using UnityEngine;

public class Player : Character
{

    private PlayerUI playerUI => ui as PlayerUI;
    // private int lv;
    private int exp;

    private Lv lvData;

    public Player(string name, int hp, int mp, float atk, float mgc, float def, float spd, BattleSystem sys)
                 : base(name, hp, mp, atk, mgc, def, spd, CharacterType.Player, sys)
    {
        /*レベル、経験値を初期化*/
        lv = 1;
        exp = 0;

        /*経験値の閾値などのデータを格納したスクリプタブルオブジェクト*/
        lvData = sys.getLvData();
    }

    /*ターンの終了処理*/
    public override void startTurn()
    {
        /*バフのターンを消化*/
        for (int i = buffList.Count - 1; -1 < i; i--)
        {
            /*バフのターンがなくなればリスト,UIから削除*/
            if (!buffList[i].changeTurn())
            {
                playerUI.delBuffLabel(buffList[i]);
                buffList.RemoveAt(i);
            }
        }
        base.startTurn();
    }

    /*経験値のセット*/
    public void setExp(float f)
    {
        exp += (int)f;
        /*経験値が閾値を超えた場合レベルを上げる*/
        Debug.Log($"pl44:{exp}");
        while (lv < lvData.requiredExp.Length && exp >= lvData.requiredExp[lv])
        {
            Debug.Log($"pl47:{lv}");
            lv++;
            lvUp();
        }
    }

    /*レベルアップ処理*/
    public void lvUp()
    {
        playerUI.lvUp();
        /*ステータスタイプ、レベルに応じてセット*/
        foreach (var (type, list) in lvData.statasList)
        {
            Debug.Log($"pl60{type}");
            Debug.Log($"pl61{list.Length}");
            setStatas(type, list[lv]);
        }
    }

    public override void showTargetPreview(bool tf)
    {
        playerUI.showTargetPreview(tf);
    }

    public override CharacterUI getUI()
    {
        return playerUI;
    }

    public override void setNowHp(int i, bool tf)
    {
        base.setNowHp(i, tf);
        playerUI.updataHpLabel(tf);
    }

    public override void useMp(int i)
    {
        base.useMp(i);
        playerUI.updataMpLabel();
    }

    public override void setBuff(Buff b)
    {
        base.setBuff(b);
        playerUI.updataBuffLabel(b);
    }

    public override void setUI(CharacterUI ui)
    {
        base.setUI(ui);
        playerUI.setCharacter(this);
    }


}

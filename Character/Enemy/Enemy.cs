using UnityEngine;

public class Enemy : Character

{
    protected string iamgeLink;
    protected EnemyUI enemyUI => base.ui as EnemyUI;
    protected int exp;

    public Enemy(string name, int hp, int mp, float atk, float mgc, float def, float spd, BattleSystem sys)
                 : base(name, hp, mp, atk, mgc, def, spd, CharacterType.Enemy, sys)
    {
        /*エネミーごとに固有の経験値*/
        exp = 0;
    }

    public virtual void loadAlg()
    {

    }

    public override void startTurn()
    {
        /*バフのターンを消化*/
        for (int i = buffList.Count - 1; -1 < i; i--)
        {
            /*バフのターンがなくなればリスト,UIから削除*/
            if (!buffList[i].changeTurn())
            {
                buffList.RemoveAt(i);
            }
        }
        base.startTurn();
    }

    public override void showTargetPreview(bool tf)
    {
        enemyUI.showTargetPreview(tf);
    }

    public override CharacterUI getUI()
    {
        return enemyUI;
    }

    public int getExp()
    {
        return exp;
    }

    public override void setNowHp(int i, bool tf)
    {
        base.setNowHp(i, tf);
        enemyUI.updataHpLabel(tf);
        Debug.Log("en55");
        if (!isAlive)
        {
            Debug.Log($"en58:{exp}");
            sys.setExp(exp);
        }
    }

    public override void setUI(CharacterUI ui)
    {
        base.setUI(ui);
        enemyUI.setImageLabel(iamgeLink);
        enemyUI.setNameLabel(name);
        enemyUI.setCharacter(this);
    }


}

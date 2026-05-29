using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;


public class Buff
{

    public enum BuffCalculationType
    {
        Flat,
        Percent,
    }

    protected int turn;
    protected string imageLink;
    protected GameObject buffObj;
    protected Character target;
    protected List<BuffEffect> list;
    protected string detail;
    protected Skill s;
    protected Character c;

    public BuffDetailWindow detailWindow;

    public Buff(List<BuffEffect> list, int turn, string imageLink,
                 Character target, string detail, Skill s, Character c)
    {
        this.turn = turn;
        this.imageLink = imageLink;
        this.target = target;
        this.list = list;
        this.detail = detail;
        this.s = s;
        this.c = c;


        /*バフアイコン用のオブジェクト生成*/
        buffObj = new GameObject("BuffIcon",
                                typeof(RectTransform),
                                typeof(RawImage),
                                typeof(AspectRatioFitter),
                                typeof(Button));


        /*AspectRatioFitterコンポーネントを取得*/
        AspectRatioFitter arf = buffObj.GetComponent<AspectRatioFitter>();
        /*スペクト比の基準を高さに*/
        arf.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
        /*1:1*/
        arf.aspectRatio = 1.0f;

        /*RawImageコンポーネントを取得*/
        RawImage ri = buffObj.GetComponent<RawImage>();
        /*画像をセット*/
        ri.texture = Resources.Load<Texture>(imageLink);

        /*Buttonコンポーネントを取得*/
        Button btn = buffObj.GetComponent<Button>();
        /*クリック時の挙動をセット*/
        btn.onClick.AddListener(() => onBuffIconClick());

    }

    public void onBuffIconClick()
    {
        target.getUI().showBuffDetail(target);
    }

    /*バフのリソースが同じかどうかの判定
      同じバフを同じキャラクターから付与されるとTrue
      どうでなければFalse
    */
    public bool sameResource(Skill s, Character c)
    {
        if (this.s == s && this.c == c)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public List<BuffEffect> getEffectList()
    {
        return list;
    }

    public GameObject getBuffObj()
    {
        return buffObj;
    }

    /*バフのターン処理
      残りターンがあるならtrue、ないならfalseを返す
    */
    public virtual bool changeTurn()
    {
        turn--;
        if (turn != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public string getImage()
    {
        return imageLink;
    }

    public string getBuffDetail()
    {
        return detail;
    }

    public void setTurn(int i)
    {
        turn = i;
    }

    public int getTurn()
    {
        return turn;
    }
}

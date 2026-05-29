using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CharacterUI : MonoBehaviour
{

    [SerializeField] protected BuffDetailWindow buffDetailWindow;

    protected Character c;
    protected string characterName;
    protected int hp;
    protected int mp;
    protected bool interactableAnime;
    protected BattleSystem sys;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    public void OnEnable()
    {
        interactableAnime = true;
    }

    public virtual void setCharacter(Character target)
    {

    }

    public virtual void updataBuffLabel(Buff b)
    {

    }

    /*バフの詳細を表示*/
    public void showBuffDetail(Character c)
    {
        buffDetailWindow.windowActivate(c);
    }

    public virtual void showTargetPreview(bool tf)
    {

    }

    public string getName()
    {
        return characterName;
    }

    public void setHp(int i)
    {
        hp -= i;
    }

    public void setMp(int i)
    {
        mp -= i;
    }

    public bool getInteractableAnime()
    {
        return interactableAnime;
    }

    public void setSys(BattleSystem sys)
    {
        this.sys = sys;
    }
}

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerUI : CharacterUI
{

    [SerializeField] protected TextMeshProUGUI nameLabel;
    [SerializeField] protected TextMeshProUGUI hpLabel;
    [SerializeField] protected TextMeshProUGUI mpLabel;
    [SerializeField] protected HorizontalLayoutGroup buffLabel;
    [SerializeField] protected GameObject buffButCover;
    [SerializeField] protected GameObject targetPreviewR;
    [SerializeField] protected GameObject targetPreviewL;
    [SerializeField] protected Image hpBer;
    [SerializeField] protected Image mpBer;

    [SerializeField] private Animator animator;


    // Player player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    protected override void Update()
    {
        /*バフボタンの有効化判定*/
        if (buffDetailWindow != null && sys != null)
        {
            /*バフ詳細ウィンドウが表示されていないかつ進行可能状態であればボタンを有効化*/
            if (!buffDetailWindow.gameObject.activeInHierarchy && sys.getInteractable())
            {
                /*カバーの非表示(ボタンの有効化)*/
                buffButCover.SetActive(false);
            }
            else
            {
                /*カバーの表示(ボタンの無効化)*/
                buffButCover.SetActive(true);
            }
        }

    }

    public override void setCharacter(Character c)
    {
        base.setCharacter(c);
        this.c = c;
        nameLabel.text = c.getName();
        hpLabel.text = c.getNowHp().ToString();
        mpLabel.text = c.getNowMp().ToString();
    }

    public override void updataBuffLabel(Buff b)
    {
        base.updataBuffLabel(b);
        /*バフのオブジェクトを取得してラベルにセット*/
        b.getBuffObj().transform.SetParent(buffLabel.transform, false);
    }

    public void delBuffLabel(Buff b)
    {
        Destroy(b.getBuffObj());
    }

    public virtual void showTargetPreview(bool tf)
    {
        targetPreviewR.SetActive(tf);
        targetPreviewL.SetActive(tf);
    }

    public void updataNameLabel()
    {
        /*名前のセット*/
        nameLabel.text = c.getName();
    }

    public void updataHpLabel(bool dmgInt)
    {
        StartCoroutine(DmgAnimation(c.getNowHp(), c.getMaxHp(), c.getAlive(), dmgInt));
    }

    IEnumerator DmgAnimation(int nowHp, int maxHp, bool alive, bool dmgInt)
    {
        if (dmgInt)
        {
            animator.SetTrigger("Damage");
        }

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        /*HPラベルの変更*/
        hpLabel.text = ((int)nowHp).ToString();
        StartCoroutine(BerAnimation(nowHp, maxHp, hpBer));

        if (alive)
        {
            /*プレイヤーが生きてたら白文字*/
            nameLabel.color = Color.white;
        }
        else
        {
            /*プレイヤーが死んでたら赤文字*/
            nameLabel.color = Color.red;
        }
    }

    public void updataMpLabel()
    {
        /*MPのセット*/
        mpLabel.text = c.getNowMp().ToString();
        StartCoroutine(BerAnimation(c.getNowMp(), c.getMaxMp(), mpBer));
    }

    IEnumerator BerAnimation(int nowInt, int maxInt, Image berImage)
    {
        /*hpバーの変更前の状態*/
        float startPercent = berImage.fillAmount;
        /*経過時間の初期化*/
        float elapsed = 0f;
        /*何秒かけて減らすか*/
        float duration = 0.5f;

        /*経過時間が想定の時間を超えるまで*/
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            /*                              変更前の長さ、変更後の長さ、現在の進行割合*/
            berImage.fillAmount = Mathf.Lerp(startPercent, (float)nowInt / maxInt, elapsed / duration);
            yield return null;
        }
        berImage.fillAmount = (float)nowInt / maxInt;
    }

    public void lvUp()
    {
        StartCoroutine(lvUpAnimation());
    }

    IEnumerator lvUpAnimation()
    {
        interactableAnime = false;
        animator.SetTrigger("LvUp");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        interactableAnime = true;
    }
}

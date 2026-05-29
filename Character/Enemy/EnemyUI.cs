using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EnemyUI : CharacterUI
{
    [SerializeField] protected RawImage imegLabel;
    [SerializeField] protected TextMeshProUGUI nameLabel;
    [SerializeField] protected GameObject targetPreview;
    [SerializeField] private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // animator = GetComponentInChildren<Animator>();
        if (animator != null)
        {
            Debug.Log("eUI25");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }



    public override void setCharacter(Character c)
    {
        base.setCharacter(c);
        this.c = c;
    }

    public override void showTargetPreview(bool tf)
    {
        targetPreview.SetActive(tf);
    }

    public void setImageLabel(string texiamgeLinkture)
    {
        imegLabel.texture = Resources.Load<Texture>(texiamgeLinkture);
    }

    public void updaImageLabel(bool tf)
    {
        imegLabel.gameObject.SetActive(tf);
    }

    public void setNameLabel(string str)
    {
        nameLabel.text = str;

    }

    public void updataHpLabel(bool dorh)
    {
        StartCoroutine(DmgAnimation(dorh));
    }

    IEnumerator DmgAnimation(bool dorh)
    {
        interactableAnime = false;
        /*dorh:ダメージかヒールかの判定
          trueでダメージ
          falseでヒール
        */

        /*dorhに対応したアニメーションの再生*/
        if (dorh)
        {
            animator.SetTrigger("Damage");
        }

        /*アニメーションの再生中は待機*/
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(1);

        /*対象が死んだ場合、対応するアニメーションの再生*/
        if (!c.getAlive())
        {
            animator.SetTrigger("Death");
        }

        interactableAnime = true;
    }

    public void updataNameLabel(bool tf)
    {
        nameLabel.gameObject.SetActive(tf);
    }
}

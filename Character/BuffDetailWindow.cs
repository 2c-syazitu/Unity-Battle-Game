using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class BuffDetailWindow : MonoBehaviour
{

    [SerializeField] private Transform detailPanel;
    [SerializeField] private GameObject detailPH;
    [SerializeField] private GameObject window;
    [SerializeField] private GameObject commandButCover;


    List<Buff> buffList;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void windowActivate(Character c)
    {
        /*ウィンドウを表示*/
        window.SetActive(true);
        commandButCover.SetActive(true);

        /*バフをクリックしたら詳細を表示*/
        buffList = c.getBuffList();
        for (int i = 0; i < buffList.Count; i++)
        {
            GameObject detailObj = Instantiate(detailPH, detailPanel);

            /*画像のセット*/
            detailObj.GetComponentInChildren<RawImage>().texture = Resources.Load<Texture>(buffList[i].getImage());

            /*テキストのセット*/
            TextMeshProUGUI tmp = detailObj.transform.Find("BuffDetail").GetComponent<TextMeshProUGUI>();
            tmp.text = buffList[i].getBuffDetail();
            /*tmpの状態を更新*/
            tmp.ForceMeshUpdate();

            /*バフターンのセット*/
            TextMeshProUGUI buffTurn = detailObj.transform.Find("BuffTurn").GetComponent<TextMeshProUGUI>();
            buffTurn.text = buffList[i].getTurn().ToString() + "Turn";

            /*オブジェクトのRectTransformを取得*/
            RectTransform rectParent = detailObj.GetComponent<RectTransform>();

            /*テキスト全体の高さ = 行数 * 一行当たりの高さ*/
            float textSize = tmp.textInfo.lineCount * tmp.fontSize;
            /*親プレハブオブジェクトの高さ*/
            float ph = rectParent.rect.height;

            /*最終的に使用したい高さの計算
              テキストサイズがプレハブの高さを超えた場合テキストサイズを使用
            */
            float h = Mathf.Max(textSize, ph);

            /*親要素の高さをセット*/
            rectParent.sizeDelta = new Vector2(rectParent.sizeDelta.x, h);


        }
    }

    /*閉じるボタンの設定*/
    public void onCloseButClick()
    {
        /*パネルのリセット*/
        foreach (Transform child in detailPanel)
        {
            Destroy(child.gameObject);
        }
        /*ウィンドウの非表示*/
        window.SetActive(false);
        commandButCover.SetActive(false);
    }
}

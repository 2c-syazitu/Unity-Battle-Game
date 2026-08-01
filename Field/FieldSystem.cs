using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class FieldSystem : MonoBehaviour
{
    [SerializeField] private FieldCharacter character;
    [SerializeField] private FieldData fieldData;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject fieldImage;
    [SerializeField] private CreateObj createObj;

    private float timer = 0f;
    private float delayTime = 2.0f;
    private RectTransform charaRect;
    private RectTransform fieldRect;
    private RectTransform fieldImageRect;
    private bool menuActive;
    private GameObject butPanel;
    private GameObject playerUI;
    private GameObject playerSelectPanel;
    private GameObject itemSelectPanel;
    private GameObject skillSelectPanel;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fieldRect = fieldImage.GetComponent<RectTransform>();
        fieldImageRect = fieldImage.GetComponent<RectTransform>();
        charaRect = character.GetComponent<RectTransform>();

        /*画像、キャラクターの座標をセット*/
        fieldImageRect.anchoredPosition = fieldData.iamgePos;
        charaRect.anchoredPosition = fieldData.characterPos;

        butPanel = menu.transform.Find("ButPanel")?.gameObject;
        playerUI = menu.transform.Find("PlayerUI")?.gameObject;
        playerSelectPanel = menu.transform.Find("PlayerSelectPanel")?.gameObject;
        itemSelectPanel = menu.transform.Find("ItemSelectPanel")?.gameObject;
        skillSelectPanel = menu.transform.Find("SkillSelectPanel")?.gameObject;
        menuActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*前回の抽選からの経過時間*/
        timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.E))
        {
            menuActive = !menuActive;
            if (butPanel == null)
            {
                Debug.Log("test");
            }
            menu.SetActive(menuActive);
            playerSelectPanel.SetActive(!menuActive);
            itemSelectPanel.SetActive(!menuActive);
            skillSelectPanel.SetActive(!menuActive);
        }
    }

    public void startBattle()
    {
        /*移動が行われたらバトル突入の抽選
          前回の抽選からの経過時間が遅延を超えれば
        */
        /*タイマーのリセット*/
        timer = 0;
        /*抽選*/
        int randomInt = Random.Range(0, 99);
        if (randomInt >= 80)
        {
            fieldData.characterPos = fieldRect.anchoredPosition;
            fieldData.iamgePos = fieldImageRect.anchoredPosition;
            fieldData.battleCount--;

            /*バトル開始*/
            // SceneManager.LoadScene("BattleScene");
        }
    }

    public bool getInteractable()
    {
        return !menuActive;
    }
}

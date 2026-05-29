using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SelectWindow : MonoBehaviour
{
    [SerializeField] private BattleSystem sys;
    [SerializeField] private Transform targetPanel;
    [SerializeField] private Button selectButPH;
    [SerializeField] private GameObject selectWindowPanel;
    [SerializeField] private TextMeshProUGUI textLabel;
    [SerializeField] private TextMeshProUGUI mpLabel;
    [SerializeField] private GameObject butCover;


    private List<Character> targetList = new List<Character>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }

    /*スキルの対象のボタン作成*/
    public void createChaBut(List<Character> list, Character turnHolder)
    {
        /*引数で対象リストを獲得*/
        for (int i = 0; i < list.Count; i++)
        {
            int index = i;
            Button selectBut = Instantiate(selectButPH, targetPanel);
            selectBut.GetComponentInChildren<TextMeshProUGUI>().text = list[i].getName();
            selectBut.onClick.AddListener(() => OnSelectButClicked(list[index], turnHolder));

            /*ボタンのEventTriggerコンポーネントを取得*/
            EventTrigger trigger = selectBut.gameObject.GetComponent<EventTrigger>();

            /*マウスカーソルがボタンに乗った時の挙動*/
            EventTrigger.Entry entryEnter = new EventTrigger.Entry();
            entryEnter.eventID = EventTriggerType.PointerEnter;
            entryEnter.callback.AddListener((data) =>
            {
                OnHoverEnter(list[index]);
            });

            /*リストに追加*/
            trigger.triggers.Add(entryEnter);

            /*マウスカーソルがボタンから外れた時の挙動*/
            EventTrigger.Entry entryExit = new EventTrigger.Entry();
            entryExit.eventID = EventTriggerType.PointerExit;
            entryExit.callback.AddListener((data) =>
            {
                OnHoverExit(list[index]);
            });

            /*リストに追加*/
            trigger.triggers.Add(entryExit);
        }
    }

    /*スキルボタン作成*/
    public void createSkillBut(Character turnHolder)
    {
        List<Skill> list = turnHolder.getSkillList();
        for (int i = 0; i < list.Count; i++)
        {
            int index = i;
            Button selectBut = Instantiate(selectButPH, targetPanel);
            selectBut.GetComponentInChildren<TextMeshProUGUI>().text = list[i].getName();
            selectBut.onClick.AddListener(() => OnSelectButClicked(list[index]));

            /*ボタンのEventTriggerコンポーネントを取得*/
            EventTrigger trigger = selectBut.gameObject.GetComponent<EventTrigger>();

            /*マウスカーソルがボタンに乗った時の挙動*/
            EventTrigger.Entry entryEnter = new EventTrigger.Entry();
            entryEnter.eventID = EventTriggerType.PointerEnter;
            entryEnter.callback.AddListener((data) =>
            {
                OnHoverEnter(list[index], turnHolder);
            });

            /*リストに追加*/
            trigger.triggers.Add(entryEnter);

            /*マウスカーソルがボタンから外れた時の挙動*/
            EventTrigger.Entry entryExit = new EventTrigger.Entry();
            entryExit.eventID = EventTriggerType.PointerExit;
            entryExit.callback.AddListener((data) =>
            {
                OnHoverExit(list[index], turnHolder);
            });

            /*リストに追加*/
            trigger.triggers.Add(entryExit);
        }

    }

    /*アイテムボタンの作成*/
    public void createItemBut(Character turnHolder)
    {
        List<Item> list = turnHolder.getItemList();
        for (int i = 0; i < list.Count; i++)
        {
            int index = i;
            Button selectBut = Instantiate(selectButPH, targetPanel);
            selectBut.GetComponentInChildren<TextMeshProUGUI>().text = list[i].getName();
            selectBut.onClick.AddListener(() => OnSelectButClicked(list[index]));

            /*ボタンのEventTriggerコンポーネントを取得*/
            EventTrigger trigger = selectBut.gameObject.GetComponent<EventTrigger>();

            /*マウスカーソルがボタンに乗った時の挙動*/
            EventTrigger.Entry entryEnter = new EventTrigger.Entry();
            entryEnter.eventID = EventTriggerType.PointerEnter;
            entryEnter.callback.AddListener((data) =>
            {
                OnHoverEnter(list[index]);
            });

            /*リストに追加*/
            trigger.triggers.Add(entryEnter);

            /*マウスカーソルがボタンから外れた時の挙動*/
            EventTrigger.Entry entryExit = new EventTrigger.Entry();
            entryExit.eventID = EventTriggerType.PointerExit;
            entryExit.callback.AddListener((data) =>
            {
                OnHoverExit();
            });

            /*リストに追加*/
            trigger.triggers.Add(entryExit);
        }

    }

    /*ボタンのリセット*/
    public void delBut()
    {
        foreach (Transform child in targetPanel)
        {
            Destroy(child.gameObject);
        }
        targetList.Clear();
    }

    /*キャラクター対象選択画面での挙動*/
    public void OnSelectButClicked(Character target, Character turnHolder)
    {
        labelReset();
        targetList.Add(target);
        turnHolder.setTargetList(targetList);
        sys.setSelectedTarget(0);
        target.showTargetPreview(false);
    }

    /*キャラクター選択でのマウスカーソルがボタン上に乗った時の挙動*/
    public void OnHoverEnter(Character c)
    {
        c.showTargetPreview(true);
    }

    /*キャラクター選択でのマウスカーソルがボタンから外れた時の挙動*/
    public void OnHoverExit(Character c)
    {
        c.showTargetPreview(false);
    }

    /*スキル選択画面の挙動*/
    public void OnSelectButClicked(Skill skill)
    {
        labelReset();
        sys.setExecuteSkill(skill);
        sys.setSelectedTarget(0);
    }

    /*スキル選択でのマウスカーソルがボタン上に乗った時の挙動*/
    public void OnHoverEnter(Skill s, Character user)
    {
        textLabel.text = s.getSkillText();
        mpLabel.text = $"{s.getCost()}/{user.getNowMp()}";
    }

    /*スキル選択でのマウスカーソルがボタンから外れた時の挙動*/
    public void OnHoverExit(Skill s, Character user)
    {
        textLabel.text = "";
        mpLabel.text = $"*/{user.getNowMp()}";
    }

    /*アイテム選択画面の挙動*/
    public void OnSelectButClicked(Item i)
    {
        if (i.getSkill() != null)
        {
            sys.setExecuteSkill(i.getSkill());
            sys.setSelectedTarget(0);
            labelReset();
        }
        else
        {
            sys.setTextLabel($"{i.getName()} can't use.");
            OnBackButClicked();
        }

    }

    /*アイテム選択でのマウスカーソルがボタン上に乗った時の挙動*/
    public void OnHoverEnter(Item i)
    {
        textLabel.text = i.getItemText();
    }

    /*アイテム選択でのマウスカーソルがボタンから外れた時の挙動*/
    public void OnHoverExit()
    {
        textLabel.text = "";
    }

    /*戻るボタンの挙動*/
    public void OnBackButClicked()
    {
        labelReset();
        selectWindowPanel.SetActive(false);
        sys.setSelectedTarget(2);
    }

    public void labelReset()
    {
        mpLabel.text = "";
        textLabel.text = "";
    }

    /*ゲッター*/
    public List<Character> getTargetList()
    {
        return targetList;
    }


}



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Menue : MonoBehaviour
{
    [SerializeField] private GameObject playerSelectPanel;
    [SerializeField] private GameObject skillSelectPanel;
    [SerializeField] private Button playerSelectButPH;
    [SerializeField] private Button skillSelectButPH;
    [SerializeField] private CreateObj createObj;
    private List<Player> playerList;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        createObj = new CreateObj();
        playerList = createObj.createCharacter.getPlayerList();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnSkillMenuButClick()
    {
        playerSelectPanel.SetActive(true);
        Debug.Log($"me24:{playerList.Count}");
        for (int i = 0; i < playerList.Count; i++)
        {
            int num = i;
            Button playerSelectBut = Instantiate(playerSelectButPH, playerSelectPanel.transform);
            playerSelectBut.GetComponentInChildren<TextMeshProUGUI>().text = playerList[num].getName();
            playerSelectBut.onClick.AddListener(() => OnPlayerSelectButClick(playerList[num]));
        }
    }

    public void OnItemMenuClick()
    {

    }

    public void OnPlayerSelectButClick(Player p)
    {
        skillSelectPanel.SetActive(true);
        List<Skill> skillList = p.getSkillList();
        Debug.Log($"mn53:{skillList.Count}");
        for (int i = 0; i < skillList.Count; i++)
        {
            int num = i;
            Button skillSelectBut = Instantiate(skillSelectButPH, skillSelectPanel.transform);
            skillSelectBut.GetComponentInChildren<TextMeshProUGUI>().text = skillList[num].getName();
            skillSelectBut.onClick.AddListener(() => OnSkillSelectButClick(skillList[num]));
        }
    }

    public void OnSkillSelectButClick(Skill s)
    {

    }
}

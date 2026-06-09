using UnityEngine;

public class CommandBut : MonoBehaviour
{
    [SerializeField] BattleSystem sys;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnAttackButClicked()
    {
        sys.setChoseCommandPhase(1);
        Debug.Log("attackBut was clicked!");
    }

    public void OnSkillButClicked()
    {
        sys.setChoseCommandPhase(2);
        Debug.Log("skill but was clicked");
    }

    public void OnDefButClicked()
    {
        sys.setChoseCommandPhase(3);
        Debug.Log("def but was clicked");
    }

    public void OnItemButClicked()
    {
        sys.setChoseCommandPhase(4);
        Debug.Log("item but was clicked");
    }
}

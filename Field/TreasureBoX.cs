using UnityEngine;

public class TreasureBoX : MonoBehaviour
{
    [SerializeField] private FieldCharacter player;
    [SerializeField] private GameObject interact;
    private Item item;
    private float searchRadius; // プレイヤーの探知範囲
    private bool opened; //箱が空いているか

    void Start()
    {
        searchRadius = 1000.0f;
        opened = false;
    }

    void Update()
    {
        /*箱がまだ空いていなければ*/
        if (!opened)
        {
            /*プレイヤーとの距離が近くなれば*/
            if (calcDistanceToPlayer() <= searchRadius)
            {
                Debug.Log("serach");
                interact.SetActive(true);
                if (Input.GetKeyDown(KeyCode.F))
                {
                    opened = true;
                    player.setItem(item);
                    interact.SetActive(false);
                }
            }
            else
            {
                interact.SetActive(false);
            }
        }

    }

    /*宝箱とプレイヤーの距離を計算*/
    public float calcDistanceToPlayer()
    {
        RectTransform playerRec = player.GetComponent<RectTransform>();
        RectTransform boxRec = this.GetComponent<RectTransform>();

        float distance = Vector2.Distance(playerRec.position, boxRec.position);
        return distance;
    }
}

using UnityEngine;
using System.Collections.Generic;

public class FieldCharacter : MonoBehaviour
{
    [SerializeField] private FieldSystem sys;
    [SerializeField] private GameObject menu;

    private float x;
    private float y;
    private RectTransform charaRect;
    private Rigidbody2D rigidbody;
    private Vector2 moveVec;
    private List<Item> itemList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        charaRect = this.GetComponent<RectTransform>();
        rigidbody = this.GetComponent<Rigidbody2D>();
        itemList = new List<Item>();
    }

    void FixedUpdate()
    {
        rigidbody.linearVelocity = moveVec * 1000;
        if (rigidbody.linearVelocity != Vector2.zero)
        {
            moveVec = new Vector2(0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*操作可能であれば*/
        if (sys.getInteractable())
        {
            /*フィールド上のキャラクターの座標を変更
              そのためのレクトトランスフォームを取得
            */

            Vector2 vec = charaRect.anchoredPosition;
            x = vec.x;
            y = vec.y;

            /*WASDそれぞれの入力を-1,0,1で感知
              移動量の仮決定
            */
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            /*移動後の座標の仮決定*/
            float movedX = x + moveX;
            float movedY = y + moveY;

            if ((moveX, moveY) != (0f, 0f))
            {
                /*移動*/
                moveVec = new Vector2(moveX, moveY);
            }
        }
    }

    public void setItem(Item i)
    {
        itemList.Add(i);
    }
}

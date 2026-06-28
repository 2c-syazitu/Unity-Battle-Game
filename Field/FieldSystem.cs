using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class FieldSystem : MonoBehaviour
{
    [SerializeField] private FieldCharacter character;
    [SerializeField] private GameObject field;
    [SerializeField] private FieldData fieldData;
    [SerializeField] private GameObject fieldImage;

    private float timer = 0f;
    private float delayTime = 2.0f;
    private float fieldW;
    private float fieldH;
    private RectTransform charaRect;
    private RectTransform fieldRect;
    private RectTransform fieldImageRect;
    private float x;
    private float y;
    private int maxDeltaX;
    private int maxDeltaY;
    private float fieldImaW;
    private float fieldImaH;
    private Vector2 imageVec;
    private WalkAbleObj walkAbleObjX;
    private WalkAbleObj walkAbleObjY;

    public enum WalkAbleObj
    {
        Character,
        Camera,
        False,
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fieldRect = field.GetComponent<RectTransform>();
        fieldImageRect = fieldImage.GetComponent<RectTransform>();
        charaRect = character.GetComponent<RectTransform>();

        /*画像、キャラクターの座標をセット*/
        fieldImageRect.anchoredPosition = fieldData.iamgePos;
        charaRect.anchoredPosition = fieldData.characterPos;

        fieldW = fieldRect.rect.width;
        fieldH = fieldRect.rect.height;

        fieldImaW = fieldImageRect.rect.width;
        fieldImaH = fieldImageRect.rect.height;
        maxDeltaX = (int)(fieldImaW / 2 - fieldW / 2);
        maxDeltaY = (int)(fieldImaH / 2 - fieldH / 2);
        // imageVec = fieldImageRect.anchoredPosition;
        Debug.Log($"fs45:{fieldImaW},{fieldImaH},{maxDeltaX},{maxDeltaY}");
    }

    // Update is called once per frame
    void Update()
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
            Debug.Log($"fs64:{moveX},{moveY}");



            /*移動後座標が移動可能であれば*/
            if (isWakabke(moveX, moveY))
            {
                /*移動*/
                move(moveX, moveY);
            }
            /*移動不可であれば*/
            else
            {
                /*移動量をリセット*/
                moveX = moveY = 0;
            }
        }




        /*移動が行われたらバトル突入の抽選
          前回の抽選からの経過時間が遅延を超えれば
        */
        if ((moveX, moveY) != (0f, 0f) && (timer > delayTime))
        {
            Debug.Log($"fs31:{timer}");
            /*タイマーのリセット*/
            timer = 0;
            /*抽選*/
            int randomInt = Random.Range(0, 99);
            Debug.Log($"fs34:{randomInt}");
            if (randomInt >= 80)
            {
                fieldData.characterPos = new Vector2(movedX, movedY);
                fieldData.iamgePos = fieldImageRect.anchoredPosition;
                fieldData.battleCount--;

                /*バトル開始*/
                // SceneManager.LoadScene("BattleScene");
            }
        }

        /*前回の抽選からの経過時間*/
        timer += Time.deltaTime;
    }

    /*引数で与えられた移動後座標が移動可能であればtrueを返す*/
    public bool isWakabke(float moveX, float moveY)
    {
        /*X方向に移動可能なオブジェクト*/
        walkAbleObjX = WalkAbleObj.False;
        /*Y方向に移動可能なオブジェクト*/
        walkAbleObjY = WalkAbleObj.False;
        bool retv;
        imageVec = fieldImageRect.anchoredPosition;

        /*X座標方向*/
        /*カメラが画面端に到達していなければ*/
        if (Mathf.Abs(imageVec.x - moveX) <= maxDeltaX)
        {
            walkAbleObjX = WalkAbleObj.Camera;
            /*キャラクターの座標が中心より端にあれば*/
            if (x * moveX < 0)
            {
                walkAbleObjX = WalkAbleObj.Character;
            }
        }
        /*キャラクターが画面端に到達していなければ*/
        else if (Mathf.Abs(x + moveX) <= (fieldW / 2) - 30)
        {
            walkAbleObjX = WalkAbleObj.Character;
        }

        /*Y座標方向*/
        /*カメラが画面端に到達していなければ*/
        if (Mathf.Abs(imageVec.y - moveY) <= maxDeltaY)
        {
            walkAbleObjY = WalkAbleObj.Camera;
            /*キャラクターの座標が中心より端にあれば*/
            if (y * moveY < 0)
            {
                walkAbleObjY = WalkAbleObj.Character;
            }
        }
        /*キャラクターが画面端に到達していなければ*/
        else if (Mathf.Abs(y + moveY) <= (fieldH / 2) - 30)
        {
            walkAbleObjY = WalkAbleObj.Character;
        }

        /*X,Yともに移動不可であればfalseを返す*/
        if (walkAbleObjX == WalkAbleObj.False && walkAbleObjY == WalkAbleObj.False)
        {
            retv = false;
        }
        else
        {
            retv = true;
        }
        return retv;
    }

    public void move(float moveX, float moveY)
    {
        /*X座標方向*/
        /*カメラの移動*/
        if (walkAbleObjX == WalkAbleObj.Camera)
        {
            fieldImageRect.anchoredPosition -= new Vector2(moveX, 0);
        }
        /*キャラクターの移動*/
        else if (walkAbleObjX == WalkAbleObj.Character)
        {
            charaRect.anchoredPosition += new Vector2(moveX, 0);
        }

        /*Y座標方向*/
        /*カメラの移動*/
        if (walkAbleObjY == WalkAbleObj.Camera)
        {
            fieldImageRect.anchoredPosition -= new Vector2(0, moveY);
        }
        /*キャラクターの移動*/
        else if (walkAbleObjY == WalkAbleObj.Character)
        {
            charaRect.anchoredPosition += new Vector2(0, moveY);
        }
    }
}

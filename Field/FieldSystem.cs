using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class FieldSystem : MonoBehaviour
{
    [SerializeField] private FieldCharacter character;
    [SerializeField] private GameObject field;
    [SerializeField] private FieldData fieldData;

    private float timer = 0f;          // 時間を測るタイマー
    private float delayTime = 2.0f;    // 遅延させたい時間（2秒）
    private float fieldWidth;
    private float fieldHeight;
    private RectTransform charaRect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RectTransform fieldRect = field.GetComponent<RectTransform>();
        charaRect = character.GetComponent<RectTransform>();
        charaRect.anchoredPosition = fieldData.position;
        fieldWidth = fieldRect.rect.width;
        fieldHeight = fieldRect.rect.height;
        Debug.Log($"fs22:{fieldWidth},{fieldHeight}");
    }

    // Update is called once per frame
    void Update()
    {
        /*フィールド上のキャラクターの座標を変更
          そのためのレクトトランスフォームを取得
        */

        Vector2 vec = charaRect.anchoredPosition;
        float x = vec.x;
        float y = vec.y;

        /*WASDそれぞれの入力を-1,0,1で感知*/
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        float movedX = x + moveX;
        float movedZ = y + moveZ;

        if ((moveX, moveZ) != (0, 0))
        {
            Debug.Log($"fs47:{movedX},{movedZ}");
        }

        /*座標の変更*/
        if (Mathf.Abs(movedX) <= (fieldWidth / 2) - 30 && Mathf.Abs(movedZ) <= (fieldHeight / 2) - 30)
        {
            charaRect.anchoredPosition += new Vector2(moveX, moveZ);
        }


        /*移動が行われたらバトル突入の抽選
          前回の抽選からの経過時間が遅延を超えれば
        */
        if ((moveX, moveZ) != (0f, 0f) && (timer > delayTime))
        {
            Debug.Log($"fs31:{timer}");
            /*タイマーのリセット*/
            timer = 0;
            /*抽選*/
            int randomInt = Random.Range(0, 99);
            Debug.Log($"fs34:{randomInt}");
            if (randomInt >= 80)
            {
                fieldData.position = new Vector2(movedX, movedZ);
                fieldData.battleCount--;

                /*バトル開始*/
                SceneManager.LoadScene("BattleScene");
            }
        }

        /*前回の抽選からの経過時間*/
        timer += Time.deltaTime;
    }
}

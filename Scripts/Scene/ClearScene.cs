using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearScene : MonoBehaviour
{
    // リザルトで使用
    public Text screenText;         // 点滅する文字
    private float speed_;           // 点滅スピード
    private float alphaTime_;       // 文字のアルファ値を変えて点滅
    public GameObject stage;        // クリアシーンの森ステージ
    public GameObject resultStage;  // リザルト中の自動ステージ
    public GameObject rezultCanvas; // リザルトを表示しているキャンバス

    // 各テキスト　0正気度　1経過時間　2敵に遭遇　3箱に隠れた回数
    private Text[] textObj;

    // アクティブ状態変更
    public CharacterCtl CCtl;

    void Start()
    {
        Cursor.visible = true;                                    // マウスカーソルの表示
        Cursor.lockState = CursorLockMode.None;                   // マウスカーソルの場所の固定解除

        textObj = new Text[4];// 0正気度　1経過時間　2敵に遭遇　3箱に隠れた回数
        for (int i = 0; i < 4; i++)
        {
            // resultCanvasの子を上から下に見ていく
            textObj[i] = rezultCanvas.transform.GetChild(i).GetComponent<Text>();
        }

        // 正気度を表示 
        textObj[0].text = "Your San[" + (int)SanitMng.sanit_ + "]";

        // ゲームプレイ時間を表示
        if ((int)GameScene.seconds<=9)
        {
            textObj[1].text = "Play Time[" + (int)GameScene.minute +
                                      ":0" + (int)GameScene.seconds + "]";
        }
        else
        {
            textObj[1].text = "Play Time[" + (int)GameScene.minute +
                                       ":" + (int)GameScene.seconds + "]";
        }

        // 敵に遭遇した回数
        textObj[2].text = "Enemy Encount[" + tLightRange.hitNum + "]";

        // 箱に隠れた回数
        textObj[3].text = "Hide Count[" + HideControl.hideNum + "]";


        CCtl.enabled = false;
        speed_ = 1.0f;

        stage.SetActive(false);
        resultStage.SetActive(true);
    }

    void Update()
    {

        if (rezultCanvas != null)
        {
            if (Input.GetMouseButtonDown(0))             // マウスの左クリックをしたとき
            {
                speed_ = 5.0f;// 点滅スピードを速くする
                StartCoroutine("Coroutine");// 森ステージを表示するためのCoroutine
            }
            screenText.color = GetAlphaColor(screenText.color);
        }
    }

    Color GetAlphaColor(Color color)
    {
        // 文字の点滅

        alphaTime_ += Time.deltaTime * 5.0f * speed_;
        color.a = Mathf.Sin(alphaTime_) * 0.5f + 0.5f;

        return color;
    }


    private IEnumerator Coroutine()
    {
        // 画面クリックから1.5秒後に
        // リザルトを消してステージを表示
        yield return new WaitForSecondsRealtime(1.5f);
        Destroy(resultStage);
        Destroy(rezultCanvas);
        stage.SetActive(true);
        CCtl.enabled = true;
    }

}

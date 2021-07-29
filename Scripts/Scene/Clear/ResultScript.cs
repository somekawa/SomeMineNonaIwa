using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultScript : MonoBehaviour
{
    public enum textType
    {
        TAP,        // スクリーン
        SANIT,      // 正気度
        TIME,       // 経過時間
        ENCOUNT,    // 敵に遭遇
        HIDE_CNT,   // 箱に隠れた回数
        FREE,       // 削除用（FREE)
        MAX
    }

    public GameObject rezultTexts; // リザルトを表示するキャンバス
    private Text[] textObj;         // resultCanvasにあるTextを入れる
    private string[] textString_;   // 表示する文字を保存

    private float speed_ = 1.0f;           // 点滅スピード
    private float alphaTime_ = 0.0f;       // 文字のアルファ値
    private int removeNum_ = 0;         // TextCoroutineを呼び出した回数

    public GameObject stages;
    private GameObject[] stage_;
    public GameObject borad;
    public CharacterCtl CCtl;       // アクティブ状態変更

    void Start()
    {
        Cursor.visible = true;                                    // マウスカーソルの表示
        Cursor.lockState = CursorLockMode.None;                   // マウスカーソルの場所の固定解除

        StartText();        // テキスト系初期化
        
        // 0番ResultStage　1番CleaStage
        stage_ = new GameObject[stages.transform.childCount];
        stage_[0] = stages.transform.GetChild(0).gameObject;
        stage_[1] = stages.transform.GetChild(1).gameObject;
        stage_[0].SetActive(true);
        stage_[1].SetActive(false);
        CCtl.enabled = false;
        borad.SetActive(false);
    }

    private void StartText()
    {
        textObj = new Text[(int)textType.MAX];
        textString_ = new string[(int)textType.MAX]{
             "Tap Screen",
             "Your San[" + (int)SanitMng.sanit_ + "]",
             "Play Time[" + (int)GameScene.minute + ":" + (int)GameScene.seconds + "]",
             "Enemy Encount[" + tLightRange.hitNum + "]",
             "Hide Count[" + HideControl.hideNum + "]",
             "                     ",
        };

        for (int i = 0; i < (int)textType.MAX; i++)
        {
            // resultCanvasの子になっている全てのテキストを上から下に見ていく
            textObj[i] = rezultTexts.transform.GetChild(i).GetComponent<Text>();

            // 秒の2桁目がなかった場合Textに文字を代入
            if ((int)GameScene.seconds <= 9)
            {
                textString_[(int)textType.TIME] = "Play Time[" + (int)GameScene.minute
                                                    + ":0" + (int)GameScene.seconds + "]";
            }
            textObj[i].text = textString_[i];

            Debug.Log(i + "番目" + textString_[i].Length);
        }

        // 点滅させるため
        textObj[(int)textType.TAP].color = GetAlphaColor(textObj[(int)textType.TAP].color);
    }

    void Update()
    {
        // TextCoroutineが呼ばれてない時なら入るようにする
        if (removeNum_ <= 0)
        {
            if (Input.GetMouseButtonDown(0))  　// マウスの左クリックをしたとき
            {
                StartCoroutine("TextCoroutine");
            }
        }

        textObj[(int)textType.TAP].color = GetAlphaColor(textObj[(int)textType.TAP].color);
    }

    Color GetAlphaColor(Color color)
    {
        // 文字の点滅
        alphaTime_ += Time.deltaTime * 5.0f * speed_;
        color.a = Mathf.Sin(alphaTime_) * 0.5f + 0.5f;
       // Debug.Log("alphaTime_" + alphaTime_);
        return color;
    }

    private IEnumerator Coroutine()
    {
        // 画面クリックから1.5秒後にリザルトを消してステージを表示
        yield return new WaitForSecondsRealtime(1.5f);

        // リザルトで使用したものを削除
        Destroy(rezultTexts);
        Destroy(stage_[0]);
        // リザルト後に使用するものをアクティブに
        stage_[1].SetActive(true);
        borad.SetActive(true);
        CCtl.enabled = true;

        this.enabled = false;

    }

    private IEnumerator TextCoroutine()
    {
        // 画面クリックから1.5秒後に
        // リザルトを消してステージを表示
        yield return new WaitForSecondsRealtime(0.1f);

        removeNum_++;
        speed_ = removeNum_ - 3;
        for (int i = 1; i < (int)textType.MAX; i++)
        {
            if (textString_[i].Length <= 0)
            {
                Debug.Log("removeNum_" + removeNum_);
                // 文字を削除しきったstringだけ入らないようにする
                continue;
            }

            // 入ってくるたびに1文字目のみ削除
            textObj[i].text = textString_[i].Remove(0, 1);

            // 1文字目を削除したstringを保存
            textString_[i] = textObj[i].text.ToString();

            if (i == ((int)textType.MAX-1))
            {
                // 1周したら最初から入りなおす
                StartCoroutine("TextCoroutine");
            }
        }

        if (textString_[(int)textType.FREE].Length <= removeNum_)
        {
            // FREE=最大文字数がremoveNum_以下になったら
            // 森ステージを表示するためのCoroutinを呼び出す
            StartCoroutine("Coroutine");
        }
    }
}

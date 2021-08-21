using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PracticMission : MonoBehaviour
{
    public enum practic
    {
        HIDE,       // 0.隠れさせて敵の動きを待つ
        LOOK,       // 1.出た後に角から敵がいる場所をチラ見
        SEARCH_KEY, // 2.鍵を探してもらう
        RADIO,  // 3.ラジオを再生
        DOOR,       // 4.ドアに接触して次のシーンに
        MAX
    }
    private practic checkPractic_;

    public GameObject slenderMan;   // 敵が表示されるタイミングを決める

    public GameObject practicUIs;   // 実践ミッション表示用
    public GameObject escapeKey;    // 鍵を取得したかのミッション用
    public Image monochromeUI;      // ラジオを再生したかのミッション用
    public GameObject radio;

    /*プレイヤー関連*/
    public GameObject player;
    private playerController playerCtl_;    // 覗き見のフラグを見る
    private HideControl hideCtl_;    // 隠れたときのフラグを見るため

    private GameScene gameScene_;

    private Text moveText;       // どのテキストであるか
    private Image textBackImage; // どの背景画像か

    private string[] textString;// 表示したいコメントを格納
    private float alphaNum_ = 0.5f;        // 画像の透明度
    private bool pCheckFlag_ = false;

    void Start()
    {
        radio.SetActive(false);
        practicUIs.SetActive(true);
        playerCtl_ = player.GetComponent<playerController>();
        hideCtl_ = player.GetComponent<HideControl>();
        gameScene_ = FindObjectOfType<GameScene>();

        checkPractic_ = practic.HIDE;

        textString = new string[(int)practic.MAX]
            { "箱に隠れてください", "黄色ポイントから\n敵の位置を確認しましょう","鍵を探しましょう",
                "ラジオで敵を誘導しましょう","ドアまで移動して脱出しましょう",};

        // 各の情報を初期化
        textBackImage = practicUIs.transform.GetChild(0).GetComponent<Image>();
        moveText = practicUIs.transform.GetChild(1).GetComponent<Text>();
        for (int i = 0; i < (int)practic.MAX; i++)
        {
            moveText.text = textString[i];
        }
        moveText.text = textString[(int)checkPractic_];
    }

    void Update()
    {
        if (gameScene_.GetPauseFlag())
        {
            return;            // pause中は何の処理もできないようにする
        }

        if (pCheckFlag_ == true)
        {
            ChoicePractic();
        }
        else
        {
            // 1.隠れるミッション
            if (checkPractic_ == practic.HIDE)
            {
                if (hideCtl_.GetHideFlg() == true)
                {
                    Debug.Log("隠れました");
                    // 隠れたら敵を出現させポイントまで移動させる
                    slenderMan.SetActive(true);
                    pCheckFlag_ = true;
                }
            }

            // 2.チラ見ミッション
            if (checkPractic_ == practic.LOOK)
            {
                // Tで傾き（角から覗いてる感じになる）
                if (playerCtl_.GetNowLean() == true)
                {
                    Debug.Log("傾きを確認しました");
                    pCheckFlag_ = true;
                }
            }

            if (checkPractic_ == practic.SEARCH_KEY)
            {
                if (escapeKey == null)
                {
                    radio.SetActive(true);
                    pCheckFlag_ = true;
                }
            }

            // 3.鍵を探させる 鍵のflagはcollision側でtrueにする
            if (checkPractic_ == practic.RADIO)
            {
                // 4.敵を誘導するミッション
                if (0.0f < monochromeUI.fillAmount)
                {
                    pCheckFlag_ = true;
                }
            }
        }
    }

    private void ChoicePractic()
    {
        //Debug.Log((int)missionNum + "番目のミッションを達成しました。");
        if (alphaNum_ <= 0.0f)
        {
            ////// アルファ値が0以下になったら非表示に
            ResetAlphaPractic(0.5f, false);
            checkPractic_ += 1; // 次のテキストに変更します
            moveText.text = textString[(int)checkPractic_];
            Debug.Log(textString[(int)checkPractic_]);
        }
        else
        {
            // Debug.Log("alpha値を減少させます");
            // 達成された表示ミッションを徐々に消す
            EraseAlphaPractic(0.5f);
        }
    }

    private void ResetAlphaPractic( float alpha, bool flag)
    {
        // アルファ値が0以下になったら非表示にするための処理
        alphaNum_ = alpha;
        textBackImage.color = new Color(255.0f, 255.0f, 255.0f, alphaNum_);
        moveText.color = new Color(0.0f, 0.0f, 0.0f, alphaNum_*2);
        pCheckFlag_ = flag;
    }

    // 選ばれたミッション、画像のアルファ値、背景の（r,g,b）bの値
    private void EraseAlphaPractic( float alpha)
    {
        //// クリアしたミッションを徐々に消す処理
        alphaNum_ -= alpha* Time.deltaTime;
        textBackImage.color = new Color(255, 255, 0.0f, alphaNum_); //imageColor;
        moveText.color = new Color(0.0f, 0.0f, 0.0f, alphaNum_ * 2);
    }

    public int GetMissionNum()
    {
        // 鍵を出現させるためのフラグを渡す
        return (int)checkPractic_;
    }

    public void SetMissionNum(int check)
    {
        checkPractic_ = (practic)check;
    }
}

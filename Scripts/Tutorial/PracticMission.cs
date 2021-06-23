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
        INDUCTION,  // 3.ビンを投げて敵を誘導
        DOOR,       // 4.ドアに接触して次のシーンに
        MAX
    }
    private practic checkPractic_;


    public GameObject slenderMan;   // 敵が表示されるタイミングを決める

    public GameObject practicText;  // ミッション表示用
    public GameObject practicImage; // テキストの背景


    /*プレイヤー関連*/
    public GameObject player;
    // 覗き見のフラグを見る
    private playerController playerCtl_;
    // 隠れたときのフラグを見るため
    private HideControl hideCtl_;
    // 投げるアイテムを取得したかフラグを見る
    private ItemTrhow trhow_;
    // 投げるアイテムを持っている間のフラグ
    private bool haveFlag_;

    public PauseScript pause;

    struct status
    {
        public Text moveText;       // どのテキストであるか
        public Image textBackImage; // どの背景画像か
    }
    private status[] status_;
    private string[] textString;// 表示したいコメントを格納
    private bool startFlag_;// 位置の変更ができたらtrueに

    void Start()
    {
        playerCtl_ = player.GetComponent<playerController>();
        hideCtl_ = player.GetComponent<HideControl>();
        trhow_ = player.GetComponent<ItemTrhow>();

        checkPractic_ = practic.HIDE;

        textString = new string[(int)practic.MAX]
            { "隠れてください", "敵の位置を確認しましょう","鍵を探しましょう",
                "敵を誘導しましょう","ドアまで移動して脱出しましょう",};

        startFlag_ = false;
        haveFlag_ = false;
        status_ = new status[(int)practic.MAX];
        for (int i = 0; i < (int)practic.MAX; i++)
        {
            // 各電池の情報を初期化
            status_[i] = new status()
            {
                moveText = practicText.GetComponent<Text>(),
                textBackImage = practicImage.GetComponent<Image>(),
            };
            status_[i].moveText.text = textString[i];
        }
        status_[(int)practic.HIDE].moveText.text = textString[(int)practic.HIDE];

    }

    void Update()
    {
        if (pause.GetPauseFlag() == true)
        {
            return;            // pause中は何の処理もできないようにする
        }
        if (startFlag_ == false)
        {
            // テキストは子で付いてるからImageをアクティブにする
            practicImage.SetActive(true);
            startFlag_ = true;
        }

        // 1.隠れるミッション
        if (hideCtl_.GetHideFlg() == true)
        {
            if(checkPractic_==practic.HIDE)
            {
                Debug.Log("隠れました");
                // 隠れたら敵を出現させポイントまで移動させる
                slenderMan.SetActive(true);

                // 次のテキストに入れ替え
                status_[(int)practic.LOOK].moveText.text = textString[(int)practic.LOOK];
                checkPractic_ = practic.LOOK;// 次のミッションを入れる

            }
        }

        // 2.チラ見ミッション
        if (checkPractic_ == practic.LOOK)
        {
            // Tで傾き（角から覗いてる感じになる）
            if (playerCtl_.GetNowLean() == true)
            {
                Debug.Log("傾きを確認しました");
                checkPractic_ = practic.SEARCH_KEY;
                // テキストの入替え
                status_[(int)practic.SEARCH_KEY].moveText.text = textString[(int)practic.SEARCH_KEY];
            }
        }

        // 3.鍵を探させる 鍵のflagはcollision側でtrueにする
        if (checkPractic_ == practic.INDUCTION)
        {
            // 4.敵を誘導するミッション
            if (trhow_.GetTrhowItemFlg() == true)
            {
                // 投げた瞬間のフラグがないからボトルを所持したときからカウントをする
                haveFlag_ = true;
            }
            // 投げるとtrhow.GetTrhowItemFlg()がfalseになるため外に出す
            if (haveFlag_ == false)
            {
                status_[(int)practic.INDUCTION].moveText.text = textString[(int)practic.INDUCTION];
            }
            else
            {
                if (Input.GetMouseButtonDown(1))  // マウスの右クリックをしたとき
                {
                    Debug.Log("右クリックをしました");
                    status_[(int)practic.DOOR].moveText.text = textString[(int)practic.DOOR];
                    checkPractic_ = practic.DOOR;
                }
            }
        }
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

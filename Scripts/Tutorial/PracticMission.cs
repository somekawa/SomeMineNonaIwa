using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PracticMission : MonoBehaviour
{
    public enum practic
    {
        HIDE,// 0.隠れさせて敵の動きを待つ
        LOOK,// 1.出た後に角から敵がいる場所をチラ見
        SEARCH_KEY,// 2.鍵を探してもらう
        // 電池は充電が少なくなったときように一応おいてるがミッションではない
        INDUCTION,// 3.ビンを投げて敵を誘導
        DOOR,
        MAX
    }
    private practic checkItem_;
    //private bool[] missionFlag;

   

    public HideControl hideCtl;

    public GameObject player;
    public GameObject slenderMan;
    public GameObject tutorialMission;

    public ItemTrhow trhow;
    private bool haveFlag_;

    private Vector3 startPos_;// 開始位置を変更する
    private bool startFlag_;// 位置の変更ができたらtrueに
    private bool slenderActFlag_;
    public playerController playerCtl;


    public GameObject practicText;// ミッション表示用
    public GameObject practicImage;
    private Text text;
    private string[] textString;
    struct status
    {
        public Text moveText;       // どのテキストであるか
        public Image textBackImage; // どの背景画像か
        public bool missionFlag;      // 指定の行動をとったかどうか
        public bool activeFlag;     // 指示を終わらせたかどうか
    }
    private status[] status_;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnEnable()
    {



        checkItem_ = practic.HIDE;

        textString = new string[(int)practic.MAX]
            { "隠れてください", "敵の位置を確認しましょう","鍵を探しましょう",
                "敵を誘導しましょう","ドアまで移動して脱出しましょう",};
        //missionFlag = new bool[(int)practic.MAX];
        startFlag_ = false;
        slenderActFlag_ = false;
        haveFlag_ = false;
        status_ = new status[(int)practic.MAX];
        for (int i = 0; i < (int)practic.MAX; i++)
        {
            // 各電池の情報を初期化
            status_[i] = new status()
            {
                moveText = practicText.GetComponent<Text>(),
                textBackImage = practicImage.GetComponent<Image>(),
                missionFlag = false,
            };
            // nouNum_ = i;
            //  status_[i].textBackImage.color = new Color(255, 255, 255, alphaNum_);
            // image_object[i].GetComponent<Image>().color = new Color(255, 255, 255, alphaNum_);
            //  text_object[i].GetComponent<Text>().color = new Color(0, 0, 0, alphaNum_ * 2);
            status_[i].moveText.text = textString[i];
            //practicText.SetActive(false);
        }
        // practicImage.SetActive(false);// 呼ばれた瞬間は非表示にする
        status_[(int)practic.HIDE].moveText.text = textString[(int)practic.HIDE];
    }

    // Update is called once per frame
    void Update()
    {
        if (startFlag_ == false)
        {
           // tutorialMission.SetActive(false);
            practicImage.SetActive(true);
            startFlag_ = true;
            Debug.Log(slenderActFlag_);
        }

        // 1.隠れるミッション
        if (hideCtl.GetHideFlg() == true)
        {
            if (status_[(int)practic.HIDE].missionFlag == false)
            {
                Debug.Log("隠れました");
                checkItem_ = practic.LOOK;// 次のミッションを入れる
                // 隠れたら敵を出現させポイントまで移動させる
                 slenderActFlag_ = true;
                slenderMan.SetActive(true);

                // 次のテキストに入れ替え
                status_[(int)practic.LOOK].moveText.text = textString[(int)practic.LOOK];
                status_[(int)practic.HIDE].missionFlag = true;

            }
        }

        // 2.チラ見ミッション
        if (status_[(int)practic.HIDE].missionFlag == true
            && status_[(int)practic.LOOK].missionFlag == false)
        {
            // Tで傾き（角から覗いてる感じになる）
            if (playerCtl.GetNowLean()==true)
            {
                Debug.Log("傾きを確認しました");
                checkItem_ = practic.SEARCH_KEY;
                // テキストの入替え
                status_[(int)practic.LOOK].missionFlag = true;
                status_[(int)practic.SEARCH_KEY].moveText.text = textString[(int)practic.SEARCH_KEY];
            }
        }
        // 3.鍵を探させる 鍵のflagはcollision側でtrueにする
        if(status_[(int)practic.SEARCH_KEY].missionFlag ==true
            && status_[(int)practic.INDUCTION].missionFlag == false)
        {
            // 4.敵を誘導するミッション
            if (trhow.GetTrhowItemFlg() == true)
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
                    status_[(int)practic.INDUCTION].missionFlag = true;
                    checkItem_ = practic.DOOR;
                }
            }
        }

    }


    public int GetMissionNum()
    {
        // 鍵を出現させるためのフラグを渡す
        return (int)checkItem_;
    }

    public void SetMissionFlag(bool flag)
    {
         status_[(int)practic.SEARCH_KEY].missionFlag=flag;
    }
}

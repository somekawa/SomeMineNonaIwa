using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialScript : MonoBehaviour
{
    // ミッションは4つずつ表示
    public enum mission
    {
        ONE,    // 1つ目
        TWO,    // 2つ目
        THREE,  // 3つ目
        FOUR,   // 4つ目
        MAX,
    }

    public enum round
    {
        FIRST,      // 1巡目
        SECONDE,    // 2巡目
        THIRD,      // 3巡目
        MAX,
    }
    public round missionRound;


    // アイテムを拾えたか（Eキー）のチェック　現在はバッテリーのみ
    public PlayerCollision playerCollision;
    private bool getUpFlag_;
    public GameObject player;

    // クイックターン、スローペースのフラグを見るため
    public playerController playerController;

    public PlayerCollision playerCol;


    // ビンを拾ったかのフラグを見る　
    public ItemTrhow trhow;
    private bool haveFlag_;     // 瓶を持ったらtrue　持っていないときfalse

    // 隠れれるかのチェックをするため　出るは次のtrue後のFキー押下でチェック
    public HideControl hideCtl;

    // ラウンドごとのミッション内容を格納[round,mission]
    private string[,] textString;

    struct status
    {
        public Text moveText;       // どのテキストであるか
        public Image textBackImage; // どのイメージか
        public bool checkFlag;      // 指定の行動をとったかどうか
        public bool activeFlag;     // 指示を終わらせたかどうか
    }
    private status[] status_;
    private int nouNum_;        // 何番目のミッションが選ばれているか

    [SerializeField] GameObject[] text_object;
    [SerializeField] GameObject[] image_object;

    private bool completeFlag_;     // ミッションを全部終わらせたらtrueに

    private float alphaNum_;        // 画像の透明度

    private bool[] roundFlag_;      // 終了したラウンドをチェック　終了=true

    void Start()
    {
        getUpFlag_ = false;
        completeFlag_ = false;
        haveFlag_ = false;
        missionRound = round.FIRST;
        textString = new string[(int)round.MAX, (int)mission.MAX]{ 
        { "前【Wキー】", "後ろ【Sキー】", "右【Dキー】", "左【Aキー】" },
        { "ライトON/OFF\n【左クリック】", "アイテムを拾う\n【Eキー】", "誘導アイテム使用\n【右クリック】", "スロースピード\n【移動+Enterキー】" }        ,
        { "箱の中に隠れる\n【Fキー】", "メニューの表示\n【Tabキー】", "クイックターン\n【Sキー連続押し】", "ゲームスタート\n【ドアに接触】" } };


        roundFlag_ = new bool[(int)round.MAX];
        for (int i = 0; i < (int)round.MAX; i++)
        {
            roundFlag_[i] = false;
        }
        ResetCommon();

    }

    private void ResetCommon() 
    {
        // 共通部分のリセット及び更新用
        Debug.Log(missionRound+"目です。共通部分を初期化します");
        nouNum_ = (int)mission.MAX;
        alphaNum_ = 0.5f;
        status_ = new status[(int)mission.MAX];

        for (int i = 0; i < (int)mission.MAX; i++)
        {
            // 各電池の情報を初期化
            status_[i] = new status()
            {
                moveText = text_object[i].GetComponent<Text>(),
                textBackImage = image_object[i].GetComponent<Image>(),
                activeFlag = true,
                checkFlag = false,
            };
            nouNum_ = i;
            //status_[i].textBackImage.color=new Color(255, 255, 255, alphaNum_);
            image_object[i].GetComponent<Image>().color = new Color(255, 255, 255, alphaNum_);
            image_object[i].SetActive(true);
            status_[i].moveText.text = textString[(int)missionRound, i];
        }
    }

    void Update()
    {
        if(missionRound == round.FIRST)
        {
            FirstMissions();
            Debug.Log("1巡目です");
        }
        else if(missionRound == round.SECONDE)
        {
            SecondeMissions();
            Debug.Log("2巡目です");
        }
        else if (missionRound == round.THIRD)
        {
            ThirdMissions();
            Debug.Log("3巡目です");
        }
    }


    void FirstMissions()
    {
        if (status_[nouNum_].checkFlag == true)
        {
            Choice((mission)nouNum_);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                status_[(int)mission.ONE].checkFlag = true;
                nouNum_ = (int)mission.ONE;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                status_[(int)mission.TWO].checkFlag = true;
                nouNum_ = (int)mission.TWO;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                status_[(int)mission.THREE].checkFlag = true;
                nouNum_ = (int)mission.THREE;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                status_[(int)mission.FOUR].checkFlag = true;
                nouNum_ = (int)mission.FOUR;
            }
        }

        RoundCheck();
    }

    void SecondeMissions()
    {
        if (status_[nouNum_].checkFlag == true)
        {
            Choice((mission)nouNum_);
        }
        else
        {
            // ライトonoffチェック
            if (Input.GetMouseButtonDown(0))             // マウスの左クリックをしたとき
            {
                nouNum_ = (int)mission.ONE;
                status_[(int)mission.ONE].checkFlag = true;
            }

            //// アイテムを拾うミッション　どのアイテムでも良い
            if (playerCol.SetItemFlag() == true)
            {
                nouNum_ = (int)mission.TWO;
                status_[(int)mission.TWO].checkFlag = true;
            }

            // 誘導アイテム使用ミッション
            if (trhow.GetTrhowItemFlg() == true)
            {
                // 投げた瞬間のフラグがないからボトルを所持したときからカウントをする
                haveFlag_ = true;
            }
            // 投げるとtrhow.GetTrhowItemFlg()がfalseになるため外に出す
            if (haveFlag_ == true)
            {
                Debug.Log("右クリックをしました");
                if (Input.GetMouseButtonDown(1))  // マウスの右クリックをしたとき
                {

                    nouNum_ = (int)mission.THREE;
                    status_[(int)mission.THREE].checkFlag = true;
                    haveFlag_ = false;
                }
            }

            // 遅い歩き
            if (playerController.GetSlowWalkFlg() == true)
            {
                nouNum_ = (int)mission.FOUR;
                status_[(int)mission.FOUR].checkFlag = true;
            }
        }

        RoundCheck();
    }

    void ThirdMissions()
    {
        if (status_[nouNum_].checkFlag == true)
        {
            Choice((mission)nouNum_);
        }
        else
        {
            // 隠れることができたか
            if (hideCtl.GetHideFlg() == true)
            {
                status_[(int)mission.ONE].moveText.text = "箱から出る\n【Fキー】";
                if (Input.GetKeyDown(KeyCode.F))
                {
                    nouNum_ = (int)mission.ONE;
                    // 箱から出たらミッションのフラグをtrueにする
                    status_[(int)mission.ONE].checkFlag = true;
                }
            }

            // メニュー画面の表示
            if (Input.GetKeyUp(KeyCode.Tab))
            {
                nouNum_ = (int)mission.TWO;
                status_[(int)mission.TWO].checkFlag = true;
            }

            // クイックターンができたかどうか
            if (playerController.GetTurnCheckFlag() == true)
            {
                nouNum_ = (int)mission.THREE;
               status_[(int)mission.THREE].checkFlag = true;
            }
            
            // 4番目のミッションはドアに接触のためfalseにしておく
            status_[(int)mission.FOUR].activeFlag = false;
        }

        RoundCheck();
    }

    private void RoundCheck()
    {        
        if (roundFlag_[(int)missionRound] == false)
        {
         // 通過したラウンドはflagをtrueにする
            roundFlag_[(int)missionRound] = true;

            if (missionRound != round.FIRST)
            {
                // 1巡目以外の時だけ呼ぶ
                ResetCommon();
            }
        }
        else
        {
            // ラウンド内で全てのミッションを終わらせる(false)と次のラウンドに
            if (status_[(int)mission.ONE].activeFlag == false
            && status_[(int)mission.TWO].activeFlag == false
            && status_[(int)mission.THREE].activeFlag == false
            && status_[(int)mission.FOUR].activeFlag == false)
            {
                if (missionRound == round.THIRD)
                {
                    // 3巡目が最後のためシーン遷移の準備をする
                    completeFlag_ = true;
                }
                // 最後のラウンドではないため1ラウンドプラスする
                missionRound++;

            }
        }
    }

    private void Choice(mission move_)
    {
        if (alphaNum_ <= 0.0f)
        {
            Debug.Log("alpha値が最小のため非表示にします。");
            // アルファ値が0以下になったら非表示に
            alphaNum_ = 0.0f;
            image_object[(int)move_].GetComponent<Image>().color = new Color(255, 255, 0, alphaNum_);
            image_object[(int)move_].SetActive(false);// 非表示に
            status_[(int)move_].activeFlag = false;
            // 別のキーが押されても良いようにアルファ値を初期値に戻す
            status_[(int)move_].checkFlag = false;
            alphaNum_ = 0.5f;
        }
        else
        {
            Debug.Log("alpha値を減少させます");
            // 達成された表示ミッションを徐々に消す
            // どれかのキーが押されたらミッション達成＝色のアルファ値を下げる
            alphaNum_ -= 0.005f;
            image_object[(int)move_].GetComponent<Image>().color = new Color(255, 255, 0, alphaNum_);
            text_object[(int)move_].GetComponent<Text>().color = new Color(0, 0, 0, 1.0f - alphaNum_);
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    // 全てのミッションが終わっていたら
    //    if (completeFlag_ == true)
    //    {
    //        // ドアと接触したらクリアシーンに移る
    //        if (other.gameObject.tag == "Door")
    //        {
    //            Debug.Log("ドアに接触");
    //            SceneManager.LoadScene("MainScene");
    //        }
    //    }
    //}

    //private void OnTriggerStay(Collider other)
    //{
    //    string[] item = new string[4] { "Battery", "InductionItem", "BarrierItem", "EscapeItem" };
    //    // ドアと接触したらクリアシーンに移る
    //    if (other.gameObject.tag == "Battery"
    //    || other.gameObject.tag == "InductionItem"
    //    || other.gameObject.tag == "BarrierItem"
    //    || other.gameObject.tag == "EscapeItem")
    //    {
    //        if (Input.GetKeyUp(KeyCode.E))
    //        {
    //            getUpFlag_ = true;
    //        }
    //    }
    //}

    public void SetItemFlag(bool flag)
    {
         getUpFlag_=flag;
    }

}


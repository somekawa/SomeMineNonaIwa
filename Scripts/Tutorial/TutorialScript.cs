using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        FIRST,      // 1巡目=0
        SECONDE,    // 2巡目=1
        THIRD,      // 3巡目=2
        MAX,
    }
    public round missionRound;

   // public GameObject PracticMission;

    // クイックターン、スローペースのフラグを見るため
    public playerController playerCtl;

    // チュートリアル用のCollisionを使ってアイテムを拾ったか確認
    private bool getUpFlag_;

    // ビンを拾ったかのフラグを見る　
    public ItemTrhow trhow;
    private bool haveFlag_;     // 瓶を持ったらtrue　持っていないときfalse

    // 隠れれるかのチェックをするため　出るは次のtrue後のFキー押下でチェック
    public HideControl hideCtl;
    private bool hideCheckFlag_;


    struct status
    {
        public Text moveText;       // どのテキストであるか
        public Image textBackImage; // どの背景画像か
        public bool checkFlag;      // 指定の行動をとったかどうか
        public bool activeFlag;     // 指示を終わらせたかどうか
    }
    private status[] status_;
    private int nouNum_;        // 何番目のミッションが選ばれているか
    
    // ラウンドごとのミッション内容を格納[round,mission]
    private string[,] textString;

    [SerializeField] GameObject[] text_object;
    [SerializeField] GameObject[] image_object;

    private bool completeFlag_;     // ミッションを全部終わらせたらtrueに

    private float alphaNum_;        // 画像の透明度

    private bool[] roundFlag_;      // 終了したラウンドをチェック　終了=true

    private bool doorColFlag_;


    private bool testEnd_;
    void Start()
    {


        //  this.GetComponent<PracticMission>().enabled = false;
        //  PracticMission.SetActive(false);
        testEnd_ = false;
        getUpFlag_ = false;
        completeFlag_ = false;
        haveFlag_ = false;
        hideCheckFlag_ = false;
        doorColFlag_ = false;
        missionRound = round.FIRST;
        textString = new string[(int)round.MAX, (int)mission.MAX]{ 
        { "前【Wキー】", "後ろ【Sキー】", "右【Dキー】", "左【Aキー】" },
        { "ライトON/OFF\n【左クリック】", "アイテムを拾う\n【Eキー】", "誘導アイテム使用\n【右クリック】", "スロースピード\n【移動+Enterキー】" },
        { "箱の中に隠れる\n【Fキー】", "メニューの表示\n【Tabキー】", "クイックターン\n【Sキー連続押し】", "Next\n【ドアに接触】" } };

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
            status_[i].textBackImage.color=new Color(255, 255, 255, alphaNum_);
            // image_object[i].GetComponent<Image>().color = new Color(255, 255, 255, alphaNum_);
            image_object[i].SetActive(true);
            text_object[i].SetActive(true);
            text_object[i].GetComponent<Text>().color = new Color(0, 0, 0, alphaNum_*2);
            status_[i].moveText.text = textString[(int)missionRound, i];
        }
    }

    void Update()
    {
        //if (missionRound == round.MAX)
        //{
        //    if (testEnd_ == false)
        //    {
        //        testEnd_ = true;
        //        for (int i = 0; i < (int)mission.MAX; i++)
        //        {
        //            image_object[i].SetActive(false);
        //            GetComponent<TutorialCollision>().enabled = false;
        //        }
        //    }
        //    return;
        //    //  全てのミッションが終わってるからもう入らないようにする
        //}


        if (missionRound == round.FIRST)
        {
            FirstMissions();
            Debug.Log((int)missionRound+"巡目です");
        }
        else if(missionRound == round.SECONDE)
        {
            SecondeMissions();
            Debug.Log((int)missionRound + "巡目です");
        }
        else if (missionRound == round.THIRD)
        {
            ThirdMissions();
            Debug.Log((int)missionRound + "巡目です");
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
            if (status_[(int)mission.ONE].activeFlag == true)
            {
                //if (Input.GetMouseButtonDown(1))  // マウスの右クリックをしたとき
                //{
                //    Debug.Log("右クリックをしました");

                //    nouNum_ = (int)mission.ONE;
                //    status_[(int)mission.ONE].checkFlag = true;
                //    // haveFlag_ = false;
                //}
                // ライトonoffチェック
                if (Input.GetMouseButtonDown(0))             // マウスの左クリックをしたとき
                {
                    nouNum_ = (int)mission.ONE;
                    status_[(int)mission.ONE].checkFlag = true;
                }
            }

            if (status_[(int)mission.TWO].activeFlag == true)
            {
                // アイテムを拾うミッション　どのアイテムでも良い
                if (getUpFlag_ == true)
                {
                    nouNum_ = (int)mission.TWO;
                    status_[(int)mission.TWO].checkFlag = true;
                }
            }

            // 誘導アイテム使用ミッション
            if (trhow.GetTrhowItemFlg() == true)
            {
               
                haveFlag_ = true;
            }
            // 投げるとtrhow.GetTrhowItemFlg()がfalseになるため外に出す
            if (haveFlag_ == true)
            {
                if (status_[(int)mission.THREE].activeFlag == true)
                {
                    if (Input.GetMouseButtonDown(1))  // マウスの右クリックをしたとき
                    {
                        Debug.Log("右クリックをしました");

                        nouNum_ = (int)mission.THREE;
                        status_[(int)mission.THREE].checkFlag = true;
                        haveFlag_ = false;
                    }
                }
            }

            if (status_[(int)mission.FOUR].activeFlag == true)
            {
                // 遅い歩き
                if (playerCtl.GetSlowWalkFlg() == true)
                {
                    nouNum_ = (int)mission.FOUR;
                    status_[(int)mission.FOUR].checkFlag = true;
                }
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
                status_[(int)mission.ONE].checkFlag = true;
                nouNum_ = (int)mission.ONE;
            }

            // メニュー画面の表示
            if (Input.GetKeyUp(KeyCode.Tab))
            {
                nouNum_ = (int)mission.TWO;
                status_[(int)mission.TWO].checkFlag = true;
            }

            if (status_[(int)mission.THREE].activeFlag == true)
            {
                // クイックターンができたかどうか
                if (playerCtl.GetTurnCheckFlag() == true)
                {
                    nouNum_ = (int)mission.THREE;
                    status_[(int)mission.THREE].checkFlag = true;
                }
            }

            // ドアに触れたかどうか
            if (doorColFlag_ == true)
            {
                nouNum_ = (int)mission.FOUR;
                // 4番目のミッションはドアに接触のためfalseにしておく
                status_[(int)mission.FOUR].checkFlag = true;

            }
        }

        RoundCheck();
    }


    private void Choice(mission move_)
    {
        if (hideCtl.GetHideFlg() == true)
        {
           // 隠れたとき用　"隠れる"から"出る"に変更するため
            if (hideCheckFlag_ == true)
            {
               // 表示したい文字を出す
                status_[(int)move_].moveText.text = "箱から出る\n【Fキー】";
                alphaNum_ = 0.5f;
                image_object[(int)move_].GetComponent<Image>().color = new Color(255, 255, 255, alphaNum_);
                text_object[(int)move_].GetComponent<Text>().color = new Color(0, 0, 0, alphaNum_ * 2);
                // 箱から出たら他と同じ消えていく処理に入る
            }
            else
            {
                // 他と同じように表示を消してから
                if (alphaNum_ <= 0.0f)
                {
                    // 箱から出るミッションを表示
                    hideCheckFlag_ = true;
                    alphaNum_ = 0.0f;
                    Debug.Log("隠れるミッションを出るミッションに変更します");
                }
                else
                {
                    alphaNum_ -= 0.005f;
                    image_object[(int)move_].GetComponent<Image>().color = new Color(255, 255, 0, alphaNum_);
                    text_object[(int)move_].GetComponent<Text>().color = new Color(0, 0, 0, alphaNum_ * 2);
                    Debug.Log("隠れるミッションを消します");
                }
            }
        }
        else
        {
            if (alphaNum_ <= 0.0f)
            {
                Debug.Log("alpha値が最小のため非表示にします。");
                // アルファ値が0以下になったら非表示に
                alphaNum_ = 0.0f;
                image_object[(int)move_].GetComponent<Image>().color = new Color(255, 255, 0, alphaNum_);
                text_object[(int)move_].GetComponent<Text>().color = new Color(0, 0, 0, alphaNum_);
                image_object[(int)move_].SetActive(false);// 非表示に
                status_[(int)move_].activeFlag = false;
                status_[(int)move_].checkFlag = false;
                // 次のラウンドで表示されるようにアルファ値を初期値に戻す
                alphaNum_ = 0.5f;
                if ((alphaNum_ == 0.5f) && (doorColFlag_ == true))
                {
                    // ドアに触れた＝基本ミッション終了
                    // 実践ミッションに移るために削除する
                    Destroy(this.gameObject);
                }
            }
            else
            {
                Debug.Log("alpha値を減少させます");
                // 達成された表示ミッションを徐々に消す
                alphaNum_ -= 0.005f;
                image_object[(int)move_].GetComponent<Image>().color = new Color(255, 255, 0, alphaNum_);
                text_object[(int)move_].GetComponent<Text>().color = new Color(0, 0, 0,  alphaNum_*2);
            }
        }

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
            if (missionRound != round.THIRD)
            {                // ラウンド内で全てのミッションを終わらせる(false)と次のラウンドに
                if (status_[(int)mission.ONE].activeFlag == false
                && status_[(int)mission.TWO].activeFlag == false
                && status_[(int)mission.THREE].activeFlag == false
                && status_[(int)mission.FOUR].activeFlag == false)
                {
                    // 最後のラウンドではないため1ラウンドプラスする
                    missionRound++;
                    Debug.Log("ミッションのラウンドをプラスします");
                }
            }
            else
            {
                if (status_[(int)mission.ONE].activeFlag == false
                 && status_[(int)mission.TWO].activeFlag == false
                 && status_[(int)mission.THREE].activeFlag == false)
                {
                    completeFlag_ = true;
                }
                if (status_[(int)mission.FOUR].activeFlag == false)
                {
                   // ドアに触れたらプラスしてUpdateに入らないようにする
                    missionRound++;
                }
            }
        }
    }

    public bool GetCompleteFlag()
    {
        // 全てのミッションを終わらせたらシーンを移す準備をする
        return completeFlag_;
    }

    public int GetMissionRound()
    {
        return (int)missionRound;
    }

    public void SetDoorColFlag(bool flag)
    {
        doorColFlag_ = flag;
    }

    public void SetItemFlag(bool flag)
    {
         getUpFlag_ = flag;
    }

}


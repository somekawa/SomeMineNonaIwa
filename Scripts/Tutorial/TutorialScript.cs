using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// チュートリアルでほしい物
// wasd移動　アイテム拾う（Eキー）　誘導アイテム使用（右クリック）　
//★ 誘導アイテム使用時に敵出現　防御アイテム自動使用の瞬間
// クイックターン（Sキー2回押し）　ライトONOFF（左クリック）
// 視線移動（マウス動かす）　ドアに接触（ゲームスタート）
// 遅い歩き（wasdのどれか+Enterキー）
// 隠れる
public class TutorialScript : MonoBehaviour
{
    // ミッションは4つずつ
    public enum mission
    {
        ONE,
        TWO,
        THREE,
        FOUR,
        MAX,
    }

    public enum round
    {
        FIRST,
        SECONDE,
        THIRD,
        MAX,
    }
    public round missionRound;

    private bool completeFlag_;// ミッションを是部終わらせたらtrueに

    public PlayerCollision playerCollision;// バッテリーを拾えたかのフラグを見る
    public playerController playerController;// クイックターン　スローペースのフラグを見る
    public ItemTrhow trhow;// ビンを拾ったかのフラグを見る
    public HideControl hideCtl;

    struct status
    {
        public Text moveText;  // どの画像であるか
        public Image textBackImage;
        public bool checkFlag;
        public bool activeFlag;
    }
    private status[] status_;

    [SerializeField] GameObject[] text_object;
    [SerializeField] GameObject[] image_object;
    private float alphaNum_;// 画像の透明度
    private int nouNum_;// 何番目のアクションが選ばれているか
    private bool haveFlag_;// 瓶を持ったらtrue　持っていないときfalse
    private string[,] textString;// ラウンドごとのミッション内容を格納[round,mission]
    
    private bool[] roundFlag_;// 終了したラウンドはtrue

    void Start()
    {

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

    private void ResetCommon()        // 共通部分のリセット及び更新用
    {
        Debug.Log("共通部分を初期化します");
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
            image_object[i].GetComponent<Image>().color = new Color(255, 255, 255, alphaNum_);
            image_object[i].SetActive(true);
            status_[i].moveText.text = textString[(int)missionRound, i];
        }
    }


    // Update is called once per frame
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
        //if (status_[(int)mission.ONE].activeFlag == false
        //&& status_[(int)mission.TWO].activeFlag == false
        //&& status_[(int)mission.THREE].activeFlag == false
        //&& status_[(int)mission.FOUR].activeFlag == false)
        //{
        //    missionRound = round.SECONDE;
        //}
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

            if (playerCollision.SetBatteryFlag() == true)
            {
                nouNum_ = (int)mission.TWO;
                status_[(int)mission.TWO].checkFlag = true;
            }

            if (trhow.GetTrhowItemFlg() == true)
            {
                // 投げた瞬間のフラグがないからボトルを所持したときからカウントをする
                haveFlag_ = true;
            }
            if (haveFlag_ == true)
            {
                if (Input.GetMouseButtonDown(1))             // マウスの左クリックをしたとき
                {
                    Debug.Log("右クリックをしました");

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

        Debug.Log("アイテムアクション確認中");
        RoundCheck();


    }

    void ThirdMissions()
    {
        //if (roundFlag_[(int)round.THIRD] == false)
        //{
        //    roundFlag_[(int)round.THIRD] = true;
        //    ResetCommon();
        //}

        if (status_[nouNum_].checkFlag == true)
        {
            Choice((mission)nouNum_);
        }
        else
        {
            // 隠れることができたか
            if (hideCtl.GetHideFlg() == true)
            {
                nouNum_ = (int)mission.ONE;
                text_object[(int)mission.ONE].GetComponent<Text>().text = "箱から出る\n【Fキー】";
                if (Input.GetKeyDown(KeyCode.F))
                {
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
                Debug.Log(playerController.GetTurnCheckFlag());
                nouNum_ = (int)mission.THREE;
               status_[(int)mission.THREE].checkFlag = true;
            }
            status_[(int)mission.FOUR].activeFlag = false;

        }
        RoundCheck();

    }

    private void RoundCheck()
    {
        if (roundFlag_[(int)missionRound] == false)
        {
            roundFlag_[(int)missionRound] = true;

            if (missionRound != round.FIRST)
            {
                // 1巡目以外の時だけ呼ぶ
                ResetCommon();
            }
        }
        else
        {
            if (status_[(int)mission.ONE].activeFlag == false
             && status_[(int)mission.TWO].activeFlag == false
            && status_[(int)mission.THREE].activeFlag == false
            && status_[(int)mission.FOUR].activeFlag == false)
            {
               if (missionRound ==round.THIRD)
                {
                    completeFlag_ = true;
                }
                missionRound =missionRound+1;

            }
        }


    }

    private void Choice(mission move_)
    {
        if (alphaNum_ <= 0.0f)
        {
            // アルファ値が0以下になったら非表示に
            alphaNum_ = 0.0f;
            image_object[(int)move_].GetComponent<Image>().color = new Color(255, 255, 0, alphaNum_);
            image_object[(int)move_].SetActive(false);
            status_[(int)move_].activeFlag = false;
            // 別のキーが押されても良いようにアルファ値を初期値に戻す
            status_[(int)move_].checkFlag = false;
            alphaNum_ = 0.5f;
        }
        else
        {
            // 達成された表示ミッションを徐々に消す
            // どれかのキーが押されたらミッション達成＝色のアルファ値を下げる
            alphaNum_ -= 0.005f;
            image_object[(int)move_].GetComponent<Image>().color = new Color(255, 255, 0, alphaNum_);
            text_object[(int)move_].GetComponent<Text>().color = new Color(0, 0, 0, 1.0f - alphaNum_);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 全てのミッションが終わっていたら
        if (completeFlag_ == true)
        {
            // ドアと接触したらクリアシーンに移る
            if (other.gameObject.tag == "Door")
            {
                Debug.Log("ドアに接触");
                SceneManager.LoadScene("MainScene");
            }
        }
    }
}


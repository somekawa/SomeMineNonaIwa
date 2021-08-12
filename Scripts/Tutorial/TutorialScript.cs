using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    // ミッションは4つずつ表示
    public enum mission
    {
        NON,    //***********(0)// 表示がずれるため下に1つ移動
        ONE,    // 1つ目(1)
        TWO,    // 2つ目(2)
        THREE,  // 3つ目(3)
        FOUR,   // 4つ目(4)
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

    /*プレイヤー関連*/
    public GameObject player;
    // クイックターン、スローペースのフラグを見るため
    private playerController playerCtl_;
    // 隠れれるかのチェックをするため　出るは次のtrue後のFキー押下でチェック
    private HideControl hideCtl_;
    private bool hideCheckFlag_ = false;


    // fillAmountが1になったらラジオを使った処理のため
    public Image monochromeUI;

    public PauseScript pause;

    // チュートリアル用のCollisionを使ってアイテムを拾ったか確認
    public PlayerCollision pCollision;

    struct statusMission
    {
        public Text moveText;       // どのテキストであるか
        public Image textBackImage; // どの背景画像か
        public bool checkFlag;      // 指定の行動をとったかどうか
        public bool activeFlag;     // 指示を終わらせたかどうか
    }
    private statusMission[] status_;
    private int nowNum_;        // 何番目のミッションが選ばれているか
    
    // ラウンドごとのミッション内容を格納[round,mission]
    private string[,] textString;

    [SerializeField] GameObject textBacks;// 表示するImageの親
    [SerializeField] GameObject texts;  // 表示するTextの親
    private float alphaNum_;        // 画像の透明度

    private bool completeFlag_ = false;     // ミッションを全部終わらせたらtrueに


    private bool[] roundFlag_;      // 終了したラウンドをチェック　終了=true

    private bool doorColFlag_ = false;
    private bool turnFlag_ = false;

    void Start()
    {
        playerCtl_ = player.GetComponent<playerController>();
        hideCtl_ = player.GetComponent<HideControl>();

        missionRound = round.FIRST;
        textString = new string[(int)round.MAX, (int)mission.MAX]{ 
        {"", "前【Wキー】", "後ろ【Sキー】", "右【Dキー】", "左【Aキー】" },
        {"", "ライトON/OFF\n【左クリック】", "アイテムを拾う\n【Eキー】", "ラジオ再生\n【Eキー】", "スロースピード\n【移動+左Shiftキー】" },
        {"", "箱の中に隠れる\n【Eキー】", "メニューの表示\n【Tabキー】", "クイックターン\n【Sキー連続押し】", "Next\n【ドアに接触】"} };

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
        alphaNum_ = 0.5f;
        status_ = new statusMission[(int)mission.MAX];
        nowNum_ = (int)mission.MAX;
        for (int i = 1; i < (int)mission.MAX; i++)
        {    // 各電池の情報を初期化
            status_[i] = new statusMission()
            {
                // 子を上から下に見ていく
                moveText = texts.transform.GetChild(i).GetComponent<Text>(),
                textBackImage = textBacks.transform.GetChild(i).GetComponent<Image>(),
                activeFlag = true,
                checkFlag = false,
            };
            nowNum_ = (int)mission.NON;
            //  表示
            status_[i].textBackImage.enabled = true;
            status_[i].moveText.enabled = true;
            // 色とアルファ値をリセット
            status_[i].textBackImage.color = new Color(255, 255, 255, alphaNum_);
            status_[i].moveText.color = new Color(0, 0, 0, alphaNum_ * 2);
            // 指定ラウンドのミッションテキストを代入
            status_[i].moveText.text = textString[(int)missionRound, i];
            Debug.Log(nowNum_ + "目のミッションをします");
        }
    }

    void Update()
    {
        if (pause.GetPauseFlag() == true)
        {
            return;            // pause中は何の処理もできないようにする
        }

        if (missionRound == round.FIRST)
        {
            FirstMissions();
          //  Debug.Log((int)missionRound+"巡目です");
        }
        else if(missionRound == round.SECONDE)
        {
            SecondeMissions();
           // Debug.Log((int)missionRound + "巡目です");
        }
        else if (missionRound == round.THIRD)
        {
            ThirdMissions();
           // Debug.Log((int)missionRound + "巡目です");
        }
    }

    void FirstMissions()
    {
        if (status_[nowNum_].checkFlag == true)
        {
            Choice((mission)nowNum_);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                nowNum_ = (int)mission.ONE;
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                nowNum_ = (int)mission.TWO;
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                nowNum_ = (int)mission.THREE;
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                nowNum_ = (int)mission.FOUR;
            }

            // NON以外が入る＝行動をした
            if (nowNum_ != (int)mission.NON
                && status_[nowNum_].activeFlag == true)
            {
                status_[nowNum_].checkFlag = true;
            }
        }

        RoundCheck();
    }

    void SecondeMissions()
    {
        if (status_[nowNum_].checkFlag == true)
        {
            Choice((mission)nowNum_);
        }
        else
        {
            // ライトonoffチェック
            if (Input.GetMouseButtonDown(0))       // マウスの左クリックをしたとき
            {
                nowNum_ = (int)mission.ONE;
            }

            // アイテムを拾うミッション　2つめ表示のバリアでクリア
            if (pCollision.GetItemNum() == PlayerCollision.item.BARRIER)
            {
                nowNum_ = (int)mission.TWO;
            }

            // fillAmounが0以上＝ラジオを使用した
            if (0.0f < monochromeUI.fillAmount)
            {
                nowNum_ = (int)mission.THREE;
            }

            // 遅い歩き
            if (playerCtl_.GetSlowWalkFlg() == true)
            {
                if ((Input.GetKeyDown(KeyCode.W)) || (Input.GetKeyDown(KeyCode.A))
                    || (Input.GetKeyDown(KeyCode.S)) || (Input.GetKeyDown(KeyCode.D)))
                {
                    nowNum_ = (int)mission.FOUR;
                }
            }

            // NON以外が入る＝行動をした
            if (nowNum_ != (int)mission.NON
                && status_[nowNum_].activeFlag == true)
            {
                status_[nowNum_].checkFlag = true;
            }
        }
        RoundCheck();
    }

    void ThirdMissions()
    {
        Debug.Log(nowNum_ + "番目のミッション activeFlag" + status_[nowNum_].activeFlag);
        if (status_[nowNum_].checkFlag == true)
        {
            Choice((mission)nowNum_);
        }
        else
        {


            // 隠れることができたか
            if (hideCtl_.GetHideFlg() == true)
            {
                nowNum_ = (int)mission.ONE;
            }

            // メニュー画面の表示
            if (Input.GetKeyUp(KeyCode.Tab))
            {
                nowNum_ = (int)mission.TWO;
            }
           
            // クイックターンができたかどうか
            if (playerCtl_.GetTurnCheckFlag() == true
                && turnFlag_ == false)
            {
                turnFlag_ = true;
                nowNum_ = (int)mission.THREE;
            }


            // ドアに触れたかどうか
            if (doorColFlag_ == true)
            {
                nowNum_ = (int)mission.FOUR;
            }

            // NON以外が入る＝行動をした
            if (nowNum_ != (int)mission.NON
                && status_[nowNum_].activeFlag == true)
            {
                Debug.Log(nowNum_+"番目のミッションを削除します"); 
                status_[nowNum_].checkFlag = true;
            }

        }
        RoundCheck();
    }

    private void Choice(mission move_)
    {
       Debug.Log((int)move_+"目のミッションを達成しました。");
        if (hideCtl_.GetHideFlg() == true)
        {
            if (hideCheckFlag_ == true)
            {            
                // 隠れたとき用　"隠れる"から"出る"に変更するため
                status_[(int)move_].moveText.text = "箱から出る\n【Eキー】";

                alphaNum_ = 0.5f;               // アルファ値リセット
                status_[(int)move_].textBackImage.color = new Color(255.0f, 255.0f, 255.0f, alphaNum_);
                status_[(int)move_].moveText.color = new Color(0.0f, 0.0f, 0.0f, alphaNum_ * 2);

                // 箱から出たら他と同じ消えていく処理に入る
            }
            else
            {
                // 隠れた時の処理
                if (alphaNum_ <= 0.0f)
                {
                    hideCheckFlag_ = true;    // 箱から出るミッションを表示
                    alphaNum_ = 0.0f;
                    Debug.Log("隠れるミッションを出るミッションに変更します");
                }
                else
                {
                    // 徐々に薄くする
                    EraseAlpha((int)move_, 0.005f);
                    Debug.Log("隠れるミッションを消します");
                }
            }
        }
        else
        {
          //  if(missionRound==round.THIRD&&move_!=mission.ONE)
            if (alphaNum_ <= 0.0f)
            {
                //////Debug.Log("alpha値が最小のため非表示にします。");
                ////// アルファ値が0以下になったら非表示に

                ResetAlpha((int)move_, 0.5f, false);
                // 次のラウンドで表示されるようにアルファ値を初期値に戻す
                if ((alphaNum_ == 0.5f) && (doorColFlag_ == true))
                {
                    // ドアに触れた＝基本ミッション終了
                    // 実践ミッションに移るために削除する
                    Destroy(this.gameObject);
                }
            }
            else
            {
                // Debug.Log("alpha値を減少させます");
                // 達成された表示ミッションを徐々に消す
                EraseAlpha((int)move_, 0.005f);

                if (doorColFlag_ == true)
                {
                    // SEの音を鳴らす
                    SoundScript.GetInstance().PlaySound(10);
                }
            }
        }
    }

    private void ResetAlpha(int num, float alpha, bool flag)
    {
        // アルファ値が0以下になったら非表示にするための処理
        alphaNum_ = alpha;
        status_[num].textBackImage.color = new Color(255.0f, 255.0f, 0.0f, alphaNum_);
        status_[num].moveText.color = new Color(0.0f, 0.0f, 0.0f, alphaNum_);
        status_[num].textBackImage.enabled = flag;
        status_[num].moveText.enabled = flag;
        status_[num].activeFlag = flag;
        status_[num].checkFlag = flag;
        nowNum_ = (int)mission.NON;
    }

    // 選ばれたミッション、画像のアルファ値
    private void EraseAlpha(int num, float alpha)
    {
        //eraseM.EraseAlphaPractic(status_[num].moveText, status_[num].textBackImage, 0.005f);
        //// クリアしたミッションを徐々に消す処理
        alphaNum_ -= alpha;
        status_[num].textBackImage.color = new Color(255.0f, 255.0f, 0.0f, alphaNum_); //imageColor;
        status_[num].moveText.color = new Color(0.0f, 0.0f, 0.0f, alphaNum_ * 2);
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
            {
                //    ラウンド内で全てのミッションを終わらせる(false)と次のラウンドに
                if (status_[(int)mission.ONE].activeFlag == false
                && status_[(int)mission.TWO].activeFlag == false
                && status_[(int)mission.THREE].activeFlag == false
                && status_[(int)mission.FOUR].activeFlag == false)
                {
                    //  最後のラウンドではないため1ラウンドプラスする
                    missionRound++;
                    Debug.Log("missionRound"+ missionRound);
                    // SEの音を鳴らす
                    SoundScript.GetInstance().PlaySound(11);

                    //  Debug.Log("ミッションのラウンドをプラスします");
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

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SystemMessage : MonoBehaviour
{
    public enum action
    {
        NON,
        BATTERY,    // 電池を取得した
        BARRIER_GET,// バリアを取得した
        KEY,        // 鍵を取得した
        RADIO_USE,  // ラジオを使えます
        DOOR,       // ドアに接触
        BOX_IN,     // 隠れる
        BOX_OUT,    // 隠れてる状態から出る
        LEAN,       // 傾きから戻す
        MAX
    }
    private action action_;

    public HideControl hideCtl;         // 隠れているかのチェック
    public PlayerCollision collision;   // どのアイテムを取得したかの確認
    public GameObject mainCamera;       // player側のカメラ

    public Image textBack;              // 文字の背景
    public Text textMessage;            // 表示する文字
    public GameObject leanMessage;      // レーン用の文字

    private int useKeyNum_;     // 鍵の所持数を保存
   
    /*隠れる関連*/
    public GameObject hideBoxes;// Boxの親を入れる
    private HideBox[] testBox_;// 子どもを代入
    private int boxNum_;// どの子どもの箱に入ったかを代入
    private int boxMaxCnt_;// 箱の個数

    public RadioVoiceAudio radioAudio_; // ラジオを使える時

    private float alpha_ = 1.0f;    // テキストのアルファ値を変更
    private string[] message_;      // 行動表示の文字を保存
    private bool textActiveFlag_ = false;   // テキストを表示しているか
    private bool itemFindFlag_ = false;     // アイテムを見つけたか

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "TutorialScene")
        {
            boxMaxCnt_ = hideBoxes.transform.childCount;
        }
        else
        {
            boxMaxCnt_ = hideBoxes.transform.childCount-1;
        }
        //// 隠れる箱を探す
        testBox_ = new HideBox[boxMaxCnt_];
        for (int i = 0; i < boxMaxCnt_; i++)
        {
            testBox_[i] = hideBoxes.transform.GetChild(i).GetComponent<HideBox>();
        }
        Debug.Log("箱の個数" + boxMaxCnt_);


        action_ = action.NON;
        message_ = new string[(int)action.MAX]{"","ライトを充電しました",
            "防御アイテムを拾いました" , "鍵を拾いました",
            "【E】ラジオを使う", "【E】鍵を使う",
            "【E】隠れる","【E】出る","【T】通路を覗き込む"};

        textMessage.text = message_[(int)action.NON];
        textBack.enabled = false;
        useKeyNum_ = collision.GetUseKeyCnt();
    }

    void Update()
    {
        Debug.Log("今のアクション" + action_);
        if (textActiveFlag_ == false)
        {
            // メッセージが表示されていないとき
            ItemAction();
        }
        else
        {
            //Debug.Log("SystemMessageのfind_" + find_);
            if (itemFindFlag_ == true)
            {
                FindMessage();
            }
            else if (action.BOX_IN <= action_)
            {
                // ラジオ以降（〇〇できる系）
                // 行動を起こすまで表示
                ActionMessage(action_);
            }
            else if (action.RADIO_USE == action_)
            {
                RadioMessage(action_);
            }
            else if (action.DOOR == action_)
            {
                UseKeyMessage();
            }

            if (action_ <= action.KEY && itemFindFlag_ == false)
            {
                ItemGetMessage(action_);
            }
        }
    }

    private void ItemAction()
    {     
        // NON以外＝何らかの行動をしているからテキストを表示
        if (action_ != action.NON)
        {
            textActiveFlag_ = true;
            return;
        }

        Debug.Log("どのアイテムかをチェックする");

        if (collision.GetFindItem() != PlayerCollision.item.NON)
        {
            for (int i = 0; i < (int)PlayerCollision.item.MAX; i++)
        {
                // アイテム接触系
                if (collision.GetFindItem() == (PlayerCollision.item)i)
                {
                    action_ = (action)collision.GetFindItem();
                    itemFindFlag_ = true;
                    Debug.Log(action_ + "と接触しています。メッセージを表示しますitemFindFlag_" + itemFindFlag_);
                    return;
                }
            }
        }

        // ラジオを使える範囲にいる時
        if (radioAudio_.GetRadioAround() == true)
        {
            action_ = action.RADIO_USE;
            return;
        }

        for (int i = 0; i < boxMaxCnt_; i++)
        {
            // ボックスを使える範囲にいる時
            if (testBox_[i].InFlagCheck() == true)
            {
                action_ = action.BOX_IN;
                boxNum_ = i;
                return;
            }
        }

        if (collision.GetDoorColFlag() == true)
        {
            action_ = action.DOOR;
            return;
        }
    }

    private void FindMessage()
    {
        if (collision.GetItemNum() == (PlayerCollision.item)action_)
        {
            Debug.Log("アイテムを拾えますからアイテムを拾った");
            itemFindFlag_ = false;
            action_ = (action)collision.GetItemNum();
            ItemGetMessage(action_);
            return;
        }

        if (action_ == action.BATTERY)
        {
            textMessage.text = "【E】電池を拾う";
            Debug.Log("FindMessage電池を拾うのなか");
        }
        else if (action_ == action.BARRIER_GET)
        {
            if (Barrier.FindObjectOfType<Barrier>().GetBarrierItemFlg())
            {
                textMessage.text = "防御アイテムは\nすでに持っています";
            }
            else
            {
                textMessage.text = "【E】防御アイテムを拾う";
            }
            Debug.Log("FindMessage防御アイテムを拾うのなか");
        }
        else if (action_ == action.KEY)
        {
            textMessage.text = "【E】鍵を拾う";
            Debug.Log("FindMessage鍵を拾うのなか");
        }

        // アイテムを取得した時のメッセージ表示
        textBack.enabled = true;
        textMessage.enabled = true;

        // 拾わずに離れた場合
        if (collision.GetFindItem() == PlayerCollision.item.NON)
        {
            Debug.Log("アイテムの範囲外に出ました");
            itemFindFlag_ = false;
            NonTextCommon();
        }
    }

    private void RadioMessage(action text)
    {
        if (radioAudio_.GetRadioAround() == false)
        {
            NonTextCommon();    // ラジオボックスの範囲外の時
        }
        else
        {
            // ラジオの範囲内にいる時
            if (radioAudio_.GetNowVoice() == false)
            {
                TextCommon(true, text);   // 再生されてない時
            }
            else
            {
                // 再生されている時
                if (radioAudio_.GetNowRound() == true)
                {
                    textBack.enabled = true;
                    textMessage.text = "【E】ラジオ停止";
                }
                else
                {
                    textBack.enabled = true;
                    textMessage.text = "再生中です";
                }
            }
        }
    }

    private void UseKeyMessage()
    {
        if (collision.GetDoorColFlag() == false || useKeyNum_ == 8)
        {
            // プレイヤーのカメラがアクティブの時
            NonTextCommon(); 
            return;
        }

        if (useKeyNum_ == collision.GetkeyItemCnt())
        {
            textBack.enabled = true; // 背景を表示
            textMessage.text = "あと" + (8 - collision.GetkeyItemCnt()) + "個";
        }
        else
        {
            useKeyNum_ = collision.GetUseKeyCnt();
            Debug.Log("使用したドアのカギ" + useKeyNum_);
            TextCommon(true, action.DOOR);
        }
    }

    private void ActionMessage(action text)
    {
        Debug.Log("○○できます系のなか");
        if (mainCamera.activeSelf == true       // プレイヤーのカメラがアクティブの時
        && testBox_[boxNum_].InFlagCheck() == false)       // ボックスの範囲外
        {
            // プレイヤーのカメラがアクティブの時
            NonTextCommon();
        }
        else
        {
            // 箱から出る時
            if (mainCamera.activeSelf == false && testBox_[boxNum_].InFlagCheck() == false)
            {
                text = action.BOX_OUT;
            }
            TextCommon(true, text);
        }
    }

    private void ItemGetMessage(action text)
    {
        Debug.Log("○○拾いました系のなか");
        textMessage.text = message_[(int)text];
        if (0.0f < alpha_)
        {
            // 表示中　
            leanMessage.SetActive(false);    // アイテム文字表示中に出ないように
            alpha_ -= 0.005f;                // 透明度を下げていく
            textMessage.color = new Color(0.0f, 0.0f, 0.0f, alpha_);
            textBack.color = new Color(255.0f, 255.0f, 255.0f, alpha_);
        }
        else
        {
            // アルファ値が0になったら非表示に
            textMessage.text = message_[(int)action.NON];       // 何も表示しない状態
            collision.SetItemNum(PlayerCollision.item.NON);     // ItemNumをリセット
            action_ = action.NON;       // どのアクションでもない
            textActiveFlag_ = false;          // テキストのアクティブ状態を変更
            TextCommon(false, text);
        }
    }

    private void TextCommon(bool flag, action text)
    {
        // true ずっと表示する系
        // false 表示後リセットのする系
        textMessage.enabled = flag;
        textBack.enabled = flag;
        textMessage.text = message_[(int)text];
        alpha_ = 1.0f;
        textMessage.color = new Color(0.0f, 0.0f, 0.0f, alpha_);
        textBack.color = new Color(255.0f, 255.0f, 255.0f, alpha_);
    }

    private void NonTextCommon()
    {
        textMessage.text = message_[(int)action.NON]; // 何も表示しない状態
        action_ = action.NON;     // どのアクションでもない
        textActiveFlag_ = false;  // テキストのアクティブ状態を変更 
        textBack.enabled = false; // 背景を非表示
    }
}

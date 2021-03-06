using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemMessage : MonoBehaviour
{
    public enum action
    {
        NON,
        BATTERY,    // 電池を取得した
        BARRIER_GET,// バリアを取得した
        KEY,        // 鍵を取得した
        RADIO_USE,  // ラジオを使えます
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

    private HideBox hideBox_;           // ボックスとプレイヤーの接触状態
    public RadioVoiceAudio radioAudio_; // ラジオを使える時
    private int saveRadioNum = -1;      // 使用したラジオの番号を保存

    private float alpha_ = 1.0f;    // テキストのアルファ値を変更
    private string[] message_;      // 行動表示の文字を保存
    private bool textActiveFlag_ = false;   // テキストを表示しているか
    private bool itemFindFlag_ = false;     // アイテムを見つけたか

    void Start()
    {
        // 隠れる箱を探す
        GameObject hideObj = GameObject.Find("MannequinBox").gameObject;
        hideBox_ = hideObj.GetComponent<HideBox>();

        action_ = action.NON;
        message_ = new string[(int)action.MAX]{"","ライトを充電しました",
            "防御アイテムを拾いました" , "鍵を拾いました",
            "【E】ラジオを使う", "【E】隠れる","【E】出る","【T】通路を覗き込む"};

        textMessage.text = message_[(int)action.NON];
        textBack.enabled = false;
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
            if (itemFindFlag_ ==true)
            {
                FindMessage();
            }
           else  if (action.BOX_IN <= action_)
            {
                // ラジオ以降（〇〇できる系）
                // 行動を起こすまで表示
                ActionMessage(action_);
            }
            else if(action.RADIO_USE== action_)
            {
                RadioMessage(action_);
            }

            if(action_<= action.KEY&& itemFindFlag_==false)
            {
                ItemGetMessage(action_);
            }
        }
    }

    private void ItemAction()
    {
        Debug.Log("どのアイテムかをチェックする");

        for (int i = 0; i < (int)PlayerCollision.item.MAX; i++)
        {
            if (collision.GetFindItem() != PlayerCollision.item.NON)
            {
                // アイテム接触系
                if (collision.GetFindItem() == (PlayerCollision.item)i)
                {
                    action_ = (action)collision.GetFindItem();
                    itemFindFlag_ = true;
                    Debug.Log(action_ + "と接触しています。メッセージを表示しますitemFindFlag_" + itemFindFlag_);
                }
            }
        }

        // ラジオを使える範囲にいる時
        if (radioAudio_.GetRadioAround() == true)
        {
            action_ = action.RADIO_USE;
        }

        // ボックスを使える範囲にいる時
        if (hideBox_.InFlagCheck() == true)
        {
            action_ = action.BOX_IN;
        }

        // NON以外＝何らかの行動をしているからテキストを表示
        if (action_!=action.NON)
        {
            textActiveFlag_ = true;
        }
    }

    private void FindMessage()
    {
        if (action_==action.BATTERY)
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

        if (collision.GetItemNum() == (PlayerCollision.item)action_)
        {
            Debug.Log("アイテムを拾えますからアイテムを拾った");
            itemFindFlag_ = false;
            action_ = (action)collision.GetItemNum();
            ItemGetMessage(action_);
            return;
        }

        // 拾わずに離れた場合
        if (collision.GetFindItem() == PlayerCollision.item.NON)
        {
            Debug.Log("アイテムの範囲外に出ました");
            textBack.enabled = false;
            textMessage.enabled = false;
            itemFindFlag_ = false;
            collision.SetItemNum(PlayerCollision.item.NON);     // ItemNumをリセット
            action_ = action.NON;     // どのアクションでもない
        }
    }

    private void RadioMessage(action text)
    {
        if (radioAudio_.GetRadioAround() == false)
        {
            // ラジオボックスの範囲外の時
            textMessage.text = message_[(int)action.NON]; // 何も表示しない状態
            textActiveFlag_ = false;        // テキストのアクティブ状態を変更 
            textBack.enabled = false; // 背景を非表示
            action_ = action.NON;     // どのアクションでもない
        }
        else
        {
            // ラジオの範囲内にいる時
            if (radioAudio_.GetNowVoice() == false)
            {
                // 再生されてない時
                saveRadioNum = radioAudio_.GetRadioNum();// 使用番号を保存
                ActiveCommon(true, text);
            }
            else
            {
                // 再生されている時
                // 保存された番号と使おうとしている番号があっているか
                if (saveRadioNum == radioAudio_.GetRadioNum())
                {
                    textBack.enabled = true;
                    textMessage.text = "ラジオを止める";
                }
                else
                {
                    textBack.enabled = true;
                    textMessage.text = "再生中です";
                }
            }
        }
    }

    private void ActionMessage(action text)
    {

        Debug.Log("○○できます系のなか");
        if (mainCamera.activeSelf == true       // プレイヤーのカメラがアクティブの時
        && hideBox_.InFlagCheck() == false )       // ボックスの範囲外
        {
            // プレイヤーのカメラがアクティブの時
            textMessage.text = message_[(int)action.NON]; // 何も表示しない状態
            textActiveFlag_ = false;        // テキストのアクティブ状態を変更 
            textBack.enabled = false; // 背景を非表示
            action_ = action.NON;     // どのアクションでもない
        }
        else
        {
            // 箱から出る時
            if (mainCamera.activeSelf == false && hideBox_.InFlagCheck() == false)
            {
                text = action.BOX_OUT; 
            }

            ActiveCommon(true, text);
        }
    }

    private void ItemGetMessage(action text)
    {
        Debug.Log("○○拾いました系のなか");
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

            ActiveCommon(false, text);
        }
    }

    private void ActiveCommon(bool flag ,action text )
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
}

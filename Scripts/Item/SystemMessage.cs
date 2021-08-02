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
        LEAN,       // 傾けれる
        MAX
    }
    private action action_;

    public HideControl hideCtl;     // 隠れているかのチェック
    public PlayerCollision collision;   // どのアイテムを取得したかの確認
    public GameObject mainCamera;  // player側のカメラ

    public Image textBack;              // 文字の背景
    public Text textMessage;            // 表示する文字
    public GameObject leanMessage;      // レーン用の文字

    private HideBox hideBox_;        // ボックスとプレイヤーの接触状態
    private RadioVoice radioVoice_;   // ラジオを使える時

    private float alpha_ = 1.0f;    // テキストのアルファ値を変更
    private bool textActiveFlag_ = false;
    private string[] message_;      // 行動表示の文字を保存

    void Start()
    {
        // 隠れる箱を探す
        GameObject hideObj = GameObject.Find("MannequinBox").gameObject;
        hideBox_ = hideObj.GetComponent<HideBox>();

        // ラジオを見つける
        GameObject radioObj = GameObject.Find("RadioVintage").gameObject;
        radioVoice_ = radioObj.GetComponent<RadioVoice>();

        action_ = action.NON;
        message_ = new string[(int)action.MAX]{"","ライトを充電しました",
            "防御アイテムを拾いました" , "鍵を拾いました",
            "Eキー：ラジオを使う", "Eキー：隠れる","Eキー：出る","Tキー：通路を覗き込む"};
        textMessage.text = message_[(int)action.NON];
        textBack.enabled = false;
    }

    void Update()
    {
        if (textActiveFlag_ == false)
        {
            // メッセージが表示されていないとき
            ItemAction();
        }
        else
        {
            if (action.RADIO_USE <= action_)
            {
                // ラジオ以降（〇〇できる系）
                // 行動を起こすまで表示
                ActionMessage(action_);
            }
            else
            {
                // 鍵までの（○○しました系）
                // 表示したら徐々に消していく
                ItemGetMessage(action_);
            }
        }
    }

    private void ItemAction()
    {
        // どのアイテムを拾ったかplayerCollisionから確認する
        for (int i = 0; i < (int)PlayerCollision.item.MAX; i++)
        {
            if (collision.GetItemNum() != PlayerCollision.item.NON)
            {
                // アイテム拾う系
                if (collision.GetItemNum() == (PlayerCollision.item)i)
                {
                    action_ = (action)collision.GetItemNum();
                }
            }
        }

        // ラジオを使える範囲にいる時
        if (radioVoice_.GetRadioAround() == true)
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

    private void ActionMessage(action text)
    {
        if (mainCamera.activeSelf == true       // プレイヤーのカメラがアクティブの時
        && hideBox_.InFlagCheck() == false        // ボックスの範囲外
        && radioVoice_.GetRadioAround() == false) // ラジオの範囲外
        {
            // プレイヤーのカメラがアクティブの時かつボックスの範囲外の時
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

            // 受け取った行動のものを表示する
            textBack.enabled = true;
            textMessage.text = message_[(int)text];
            alpha_ = 1.0f;
            textMessage.color = new Color(0.0f, 0.0f, 0.0f, alpha_);
            textBack.color = new Color(255.0f, 255.0f, 255.0f, alpha_);
        }
    }

    private void ItemGetMessage(action text)
    {
        // アイテムを取得した時のメッセージ表示
        textMessage.text = message_[(int)text];
        textBack.enabled = true;
        if (0.0f <= alpha_)
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
            textBack.enabled = false;   // 背景を非表示
            action_ = action.NON;       // どのアクションでもない
            alpha_ = 1.0f;              // アルファ値をリセット
            textActiveFlag_ = false;          // テキストのアクティブ状態を変更
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemMessage : MonoBehaviour
{
    public enum action
    {

        NON,
        BATTERY,
        BARRIER_GET,
        // BARRIER_USE,
        BOTTLE_GET,
        //BOTTLE_TRHOW,
        KEY,
        MAX
    }
    private action action_;
        

    public PlayerCollision collision;// どのアイテムを取得したかの確認

    public Image textBack;
    public Text textMessage;
    private float alpha_;// テキストのアルファ値を変更
    private string[] message_;
    private bool activeFlag_;

    void Start()
    {
        alpha_ = 255.0f;
        activeFlag_ = false;
        action_ = action.NON;
        message_ = new string[(int)action.MAX]{"","ライトの充電をします",  
            "防御アイテムを拾いました" ,"誘導アイテムを拾いました","鍵を入手しました"};
        textMessage.text = message_[(int)action.NON];
        textBack.enabled = false;
    }

    void Update()
    {
        if (activeFlag_ == true)
        {
            // actionした時だけはいる
            CheckMessage(action_);
        }
        else
        {
             ItemAction();
        }
    }

    private void ItemAction()
    {
        for (int i = 0; i < (int)PlayerCollision.item.MAX; i++)
        {
            if (collision.GetItemNum() != PlayerCollision.item.NON)
            {
                // アイテム拾う系
                if (collision.GetItemNum() == (PlayerCollision.item)i)
                {
                    action_ = (action)collision.GetItemNum();
                    activeFlag_ = true;
                }
            }
        }
    }

    private void CheckMessage(action text)
    {
        if (action_ != action.NON)
        {
            textMessage.text = message_[(int)text];
            if (alpha_ <= 0)
            {
                textMessage.text = message_[(int)action.NON];// 何も表示しない状態
                action_ = action.NON;// どのアクションでもない
                collision.SetItemNum(PlayerCollision.item.NON);// ItemNumをリセット
                activeFlag_ = false;// テキストのアクティブ状態を変更
                alpha_ = 255.0f;
                textBack.enabled = false;
            }
            else
            {
                // 表示中　透明度を下げていく
                alpha_ -= 2.0f;
                textMessage.color = new Color(0, 0, 0, alpha_);
                textBack.enabled = true;
            }
        }
        else
        {
            textMessage.text = message_[(int)action.NON];// 何も表示しない状態

        }
      //  Debug.Log(alpha_);
    }
}

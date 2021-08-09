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

        //BATTERY_FIND,// 電池を拾う
        //BARRIER_FIND,// バリアを拾う
        //KEY_FIDN,   // 鍵を拾う
        RADIO_USE,  // ラジオを使えます
        BOX_IN,     // 隠れる
        BOX_OUT,    // 隠れてる状態から出る
        LEAN,       // 傾きから戻す
        MAX
    }
    private action action_;

    //public enum find
    //{
    //    NON,
    //    BATTERY,    // 電池を取得した
    //    BARRIER_GET,// バリアを取得した
    //    KEY,        // 鍵を取得した
    //    MAX
    //}
    //private find find_;


    public HideControl hideCtl;     // 隠れているかのチェック
    public PlayerCollision collision;   // どのアイテムを取得したかの確認
    public GameObject mainCamera;  // player側のカメラ

    public Image textBack;              // 文字の背景
    public Text textMessage;            // 表示する文字
  //  public Text testMessage2;
    public GameObject leanMessage;      // レーン用の文字

    private HideBox hideBox_;        // ボックスとプレイヤーの接触状態
    public RadioVoiceAudio radioAudio_;   // ラジオを使える時

    private float alpha_ = 1.0f;    // テキストのアルファ値を変更
    private bool textActiveFlag_ = false;
    private string[] message_;      // 行動表示の文字を保存
    private string[] findMessage_;      // 行動表示の文字を保存
    private bool itemFindFlag_ = false;

    void Start()
    {
        // 隠れる箱を探す
        GameObject hideObj = GameObject.Find("MannequinBox").gameObject;
        hideBox_ = hideObj.GetComponent<HideBox>();

        action_ = action.NON;
       // find_ = find.NON;
        message_ = new string[(int)action.MAX]{"","ライトを充電しました",
            "防御アイテムを拾いました" , "鍵を拾いました",
        //    "【E】電池を拾う","【E】防御アイテムを拾う","【E】鍵を拾う",
            "【E】ラジオを使う", "【E】隠れる","【E】出る","【T】通路を覗き込む"};
        //findMessage_ = new string[(int)find.MAX]   {
        //   "", "【E】電池を拾う","【E】防御アイテムを拾う","【E】鍵を拾う"
        //};

        //testMessage2.text = message_[(int)find.NON];
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
                FindMessage(action_);
            }
           else  if (action.RADIO_USE <= action_)
            //      if (action.BATTERY_FIND <= action_)
            {
                // ラジオ以降（〇〇できる系）
                // 行動を起こすまで表示
                ActionMessage(action_);
            }
            //else
            //{
            //    // 鍵までの（○○しました系）
            //    // 表示したら徐々に消していく
            //    ItemGetMessage(action_);
            //}

            if(action_<= action.KEY&& itemFindFlag_==false)
            {
                ItemGetMessage(action_);

            }
        }
    }

    private void ItemAction()
    {
        Debug.Log("どのアイテムかをチェックする");
        //// どのアイテムを拾ったかplayerCollisionから確認する
        //for (int i = 0; i < (int)PlayerCollision.item.MAX; i++)
        //{
        //    if (collision.GetItemNum() != PlayerCollision.item.NON)
        //    {
        //        // アイテム拾う系
        //        if (collision.GetItemNum() == (PlayerCollision.item)i)
        //        {
        //            action_ = (action)collision.GetItemNum();
        //        }
        //    }
        //}

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
        if (action_!=action.NON)//||find_!=find.NON)
        {
            textActiveFlag_ = true;
        }
    }

    private void FindMessage(action text)
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
        //Debug.Log(find_+"を見つけたメッセージを表示しています");
        // アイテムを取得した時のメッセージ表示
        //  testMessage2.text = findMessage_[(int)text];
        textBack.enabled = true;
        textMessage.enabled = true;
        //if (find_ != find.NON)
        //{
        //    textActiveFlag_ = false;
        //}


        if (collision.GetItemNum() == (PlayerCollision.item)action_)
        {
            Debug.Log("アイテムを拾えますからアイテムを拾った");
          //  textBack.enabled = false;
            //textMessage.enabled = false;
            itemFindFlag_ = false;
            action_ = (action)collision.GetItemNum();
            ItemGetMessage(action_);
            return;
        }

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

    private void ActionMessage(action text)
    {
        Debug.Log("○○できます系のなか");
        if (mainCamera.activeSelf == true       // プレイヤーのカメラがアクティブの時
        && hideBox_.InFlagCheck() == false        // ボックスの範囲外
        && radioAudio_.GetRadioAround() == false) // ラジオの範囲外
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
            textMessage.enabled = true;
            textBack.enabled = true;
            textMessage.text = message_[(int)text];
            alpha_ = 1.0f;
            textMessage.color = new Color(0.0f, 0.0f, 0.0f, alpha_);
            textBack.color = new Color(255.0f, 255.0f, 255.0f, alpha_);
        }
    }

    private void ItemGetMessage(action text)
    {
        Debug.Log("○○拾いました系のなか");
        // アイテムを取得した時のメッセージ表示
        textMessage.text = message_[(int)text];
        //textBack.enabled = true;
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
            textBack.enabled = false;   // 背景を非表示
            textMessage.enabled = false;
            action_ = action.NON;       // どのアクションでもない
            alpha_ = 1.0f;              // アルファ値をリセット
            textActiveFlag_ = false;          // テキストのアクティブ状態を変更
            textMessage.color = new Color(0.0f, 0.0f, 0.0f, alpha_);
            textBack.color = new Color(255.0f, 255.0f, 255.0f, alpha_);
        }
    }
}

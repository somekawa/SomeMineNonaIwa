using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerCollision : MonoBehaviour
{
    // アイテムの種類
    public enum item
    {
        NON,
        BATTERY,
        BARRIER,
        ESCAPE,
        MAX
    }
    private item item_;
    private item findItem_;// アイテムと接触している時用

    private HideControl hideControl_;     // 箱に隠れる処理

    private bool batteryGetFlag_ = false; // バッテリーを拾ったかのチェック

    // 脱出アイテム関連
    private int keyItemCnt_    = 0;       // 現在所持してる脱出アイテムの数
    private int maxKeyItemNum_ = 8;       // 脱出アイテムの個数(8個)
    private bool keyItemColFlag_ = false; // 鍵と接触したか
    private bool doorColFlag_ = false;    // ドアと接触しているか

    private int chainCnt_ = 0;            // 扉の鎖の外した本数

    private bool  itemGetFlg_  = true;    // true：取得できる,false：取得できない
    private float itemGetTime_ = 0.0f;    // アイテムが取得できるようになるまでの時間
    private float itemGetTimeMax_ = 1.0f; // アイテムが取得できるようになるまでの時間の最大値

    void Start()
    {
        item_ = item.NON;
        findItem_ = item.NON;
        // PlayerからHideControl取得
        GameObject playerObj = transform.root.gameObject;
        hideControl_ = playerObj.GetComponent<HideControl>();
    }

    void Update()
    {
        if (maxKeyItemNum_ <= keyItemCnt_)
        {
            // 所持数が最大個数になったら　もし脱出アイテムがあっても拾えなくする
            keyItemColFlag_ = false;
        }

        if (keyItemColFlag_ == true)
        {
            keyItemColFlag_ = false;
            keyItemCnt_++;              // 脱出アイテム所持数を増やす
            Debug.Log("脱出アイテムゲット" + keyItemCnt_);

            // 鍵を2つ取得する毎にスレンダーマンの数を増やす
            if (keyItemCnt_ % 2 == 0)
            {
                SlenderSpawner.GetInstance().instantiateFlag = true;
            }
        }

        // アイテムが取得可能時間ではないとき
        if(!itemGetFlg_)
        {
            if (itemGetTime_ < itemGetTimeMax_)
            {
                itemGetTime_ += Time.deltaTime;
            }
            else
            {
                // 取得できる状態に切り替える
                itemGetFlg_ = true;
                Debug.Log("アイテムの取得可能時間になりました");
            }
        }

        Debug.Log("PlayerCollision" + findItem_);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 持ってる脱出アイテムが最大の時
        if (keyItemCnt_ == maxKeyItemNum_)
        {
            // ドアと接触したらクリアシーンに移る
            if (other.gameObject.tag == "Door")
            {
                Debug.Log("ドアに接触");
                //SceneManager.LoadScene("ClearScene");
            }
       }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Untagged")
        {
            return;     // タグが付いていないものが範囲内にあったら抜ける
        }

        // ドアと接触
        if (other.gameObject.tag == "Door")
        {
            Debug.Log("ドアに接触");
            doorColFlag_ = true;
            if (Input.GetKeyUp(KeyCode.E))
            {
                Debug.Log("ドアの鍵を開けました");
                // 鍵を開ける音を鳴らす
                SoundScript.GetInstance().PlaySound(9);
                foreach (Transform c in other.gameObject.transform)
                {
                    ChainCheck(c, "chain");
                    ChainCheck(c, "padLock");
                }
            }
        }

        // 脱出アイテムとの当たり判定
        if (other.gameObject.tag == "EscapeItem")
        {
            findItem_ = item.ESCAPE;// 接触中
            if (Common(other))
            {
                Debug.Log("脱出アイテムゲット");
                keyItemColFlag_ = true;
                item_ = item.ESCAPE;
                findItem_ = item.NON;
                // SEの音を鳴らす
                SoundScript.GetInstance().PlaySound(7);
            }
            return;
        }
        // 電池との当たり判定
        else if (other.gameObject.tag == "Battery")
        {
            findItem_ = item.BATTERY; // 接触中
            if (Common(other))
            {
                Debug.Log("電池ゲット");
                batteryGetFlag_ = true;
                item_ = item.BATTERY;
                findItem_ = item.NON;
                // SEの音を鳴らす
                SoundScript.GetInstance().PlaySound(8);
            }
            return;
        }
        // 防御アイテム取得処理
        else if (other.gameObject.tag == "BarrierItem")
        {
            findItem_ = item.BARRIER;// 接触中
            if (Common(other))
            {
                Barrier barrier = Barrier.FindObjectOfType<Barrier>();
                // 同じオブジェクト内の他のスクリプトを参照する場合
                barrier.SetBarrierItemFlg(true);
                // Barrierクラスのflagをtrueにしたい
                Debug.Log("防御アイテムゲット");
                item_ = item.BARRIER;
                findItem_ = item.NON;
                // SEの音を鳴らす
                SoundScript.GetInstance().PlaySound(8);
            }
            return;
        }
        // 箱に隠れる処理
        // HideControlにて実装
        else if (other.gameObject.tag == "HideObj")
        {
            hideControl_.HideBoxAction(other.gameObject);
            return;
        }
        else
        {
            return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // ドアと接触
        if (other.gameObject.tag == "Door")
        {
            Debug.Log("ドアから離れる");
            doorColFlag_ = false;
        }

        if (other.gameObject.tag == "EscapeItem"
        || other.gameObject.tag == "Battery"
        || other.gameObject.tag == "BarrierItem")
        {
            findItem_ = item.NON;
        }
    }

    public bool GetBatteryFlag()
    {
        return batteryGetFlag_;
    }

    public void SetBatteryFlag(bool flag)
    {
        batteryGetFlag_ = flag;
    }

    public bool GetKeyFlag()
    {
        return keyItemColFlag_;
    }

    public item GetItemNum()
    {
        // どのアイテムを取得したか
        return item_;
    }
    public item GetFindItem()
    {
        return findItem_;// どのアイテムが範囲にあるか
    }

    public void SetItemNum(item setItem)
    {
        // 渡されるものがリセットされないからmessageがおかしくなるため
        item_ = setItem;
    }

    // 共通処理をまとめた関数
    bool Common(Collider other)
    {
        // Eキーの押下時 && 取得可能かのフラグを確認(全ての取得条件を満たしているときにtrueを返す)
        if (Input.GetKeyUp(KeyCode.E) && itemGetFlg_)
        {
            // 取得できるとき
            itemGetFlg_ = false;

            // 既にバリアアイテムを所持している場合は、取得できないようにする
            if (other.gameObject.tag == "BarrierItem")
            {
                if (Barrier.FindObjectOfType<Barrier>().GetBarrierItemFlg())
                {
                    return false;
                }
            }
            Destroy(other.gameObject);   // オブジェクトを削除
            return true;
        }

        return false;
    }

    public int GetkeyItemCnt()
    {
        return keyItemCnt_;
    }

    public void SetKeyItemCnt(int num)
    {
        keyItemCnt_ = num;
    }

    public bool GetDoorColFlag()
    {
        return doorColFlag_;
    }

    public int GetUseKeyCnt()
    {
        return chainCnt_;
    }

    // 扉の共通処理
    void ChainCheck(Transform c ,string str)
    {
        if (c.gameObject.tag != str)
        {
            return;
        }

        if (chainCnt_ < keyItemCnt_ || (keyItemCnt_ == maxKeyItemNum_))
        {
            Destroy(c.gameObject);   // オブジェクトを削除
            if(c.gameObject.tag == "padLock")
            {
                chainCnt_++;
                Debug.Log("ドアのチェーン破壊" + chainCnt_);
            }
        }
        else
        {
            return;
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    private bool batteryGetFlag_ = false; // バッテリーを拾ったかのチェック
    // 脱出アイテム関連
    private int keyItemCnt_ = 0;     // 現在所持してる脱出アイテムの数
    private int maxKeyItemNum_ = 8;  // 脱出アイテムの個数　8個まで

    // 脱出アイテムと接触したか　true=接触 false=接触してない
    private bool keyItemColFlag_ = false;

    private int chainCnt = 0;

    void Start()
    {

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
            keyItemCnt_++;      // 脱出アイテム所持数を増やす
            Debug.Log("脱出アイテムゲット" + keyItemCnt_);
            if (keyItemCnt_ % 2 == 0)
            {
                SlenderSpawner.GetInstance().instantiateFlag = true;
            }
        }

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
            return;
        }

        // ドアと接触
        if (other.gameObject.tag == "Door")
        {
            Debug.Log("ドアに接触");
            if (Input.GetKeyUp(KeyCode.E))
            {
                foreach (Transform c in other.gameObject.transform)
                {
                    if (c.gameObject.tag == "chain")
                    {
                        if (chainCnt < keyItemCnt_ || (keyItemCnt_ == maxKeyItemNum_))
                        {
                            Destroy(c.gameObject);   // オブジェクトを削除
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (c.gameObject.tag == "padLock")
                    {
                        if (chainCnt < keyItemCnt_ || (keyItemCnt_ == maxKeyItemNum_))
                        {
                            Destroy(c.gameObject);   // オブジェクトを削除
                            chainCnt++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }

        // 脱出アイテムとの当たり判定
        if (other.gameObject.tag == "EscapeItem")
        {
            if (Common(other))
            {
                Debug.Log("脱出アイテムゲット");
                keyItemColFlag_ = true;
            }
            return;
        }
        // 電池との当たり判定
        else if (other.gameObject.tag == "Battery")
        {
            if(Common(other))
            {
                Debug.Log("電池ゲット");
                batteryGetFlag_ = true;
            }
            return;
        }
        // 防御アイテム取得処理
        else if (other.gameObject.tag == "BarrierItem")
        {
            if (Common(other))
            {
                Barrier barrier = transform.parent.gameObject.GetComponent<Barrier>();
                // 同じオブジェクト内の他のスクリプトを参照する場合
                barrier.SetBarrierItemFlg(true);
                // Barrierクラスのflagをtrueにしたい
                Debug.Log("防御アイテムゲット");
                item_ = item.BARRIER;
                // SEの音を鳴らす
                audioSource_.PlayOneShot(itemGetSE_);
            }
            return;
        }
        // 誘導アイテム取得処理
        else if (other.gameObject.tag == "InductionItem")
        {
            if(Common(other))
            {
                ItemTrhow itemTrhow = transform.parent.gameObject.GetComponent<ItemTrhow>();
                if (!itemTrhow.GetTrhowItemFlg())
                {
                    itemTrhow.SetTrhowItemFlg(true);
                    Debug.Log("誘導アイテムゲット");
                }
            }
            return;
        }
        else
        {
            return;
        }
    }

    public bool GetBatteryFlag()
    {
        // Debug.Log("SetBatteryFlag+++++++電池ゲット");
        return batteryGetFlag_;
    }

    public void SetBatteryFlag(bool flag)
    {
        // Debug.Log("GetBatteryFlag+++++++電池ゲット");
        batteryGetFlag_ = flag;
    }

    bool Common(Collider other)
    {
        // 共通処理をまとめた関数

        // Eキーの押下時にtrueを返す
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (other.gameObject.tag == "BarrierItem")
            {
                if (transform.parent.gameObject.GetComponent<Barrier>().GetBarrierItemFlg())
                {
                    return false;
                }
            }

            Destroy(other.gameObject);   // オブジェクトを削除
            return true;
        }

        return false;
    }
}

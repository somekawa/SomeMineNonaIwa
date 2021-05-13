using System.Collections;
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
      // 脱出アイテムとの当たり判定
        if (other.gameObject.tag == "EscarpItem")
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                keyItemColFlag_ = true;
                Destroy(other.gameObject);            // オブジェクトを削除
            }
        }

        // 電池との当たり判定
        if (other.gameObject.tag == "Battery")
        {
            Debug.Log("電池と接触");
            if (Input.GetKeyUp(KeyCode.E))
            {

                batteryGetFlag_ = true;
                Destroy(other.gameObject);            // オブジェクトを削除
            }
        }

        // ここに防御アイテムを拾ったときの処理とワインボトルの処理を書く
        if (other.gameObject.tag == "BarrierItem")
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                // 同じオブジェクト(Cube)内の他のスクリプトを参照する場合
                GetComponent<Barrier>().SetBarrierItemFlg(true);

                // Barrierクラスのflagをtrueにしたい
                Debug.Log("BarrierItemゲット");
                Destroy(other.gameObject);            // オブジェクトを削除
            }
        }
    }

    public bool SetBatteryFlag()
    {
        // Debug.Log("SetBatteryFlag+++++++電池ゲット");
        return batteryGetFlag_;
    }

    public void GetBatteryFlag(bool flag)
    {
        // Debug.Log("GetBatteryFlag+++++++電池ゲット");
        batteryGetFlag_ = flag;

    }


}

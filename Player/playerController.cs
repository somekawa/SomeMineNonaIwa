using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private CharacterController controller_;

    private float speed_ = 3.0f;          // デフォルトの移動速度
    private float gravity_ = 9.8f;        // 重力
    private bool walkFlg_ = false;        // 移動中はtrue
    private bool slowWalk_ = false;       // 移動速度が遅くなる場合はtrue
    private bool batteryGetFlag_ = false; // バッテリーを拾ったかのチェック

    private const float speedMax_ = 3.0f; // 移動速度の最大値
    private const int   countMax_ = 120;  // エフェクト再生時間の最大値

    void Start()
    {
        controller_ = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            //エンターキー入力
            Debug.Log("エンターキー入力");
            slowWalk_ = true;
        }
        else
        {
            slowWalk_ = false;
        }

        CalculateMove();
    }

    void CalculateMove()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);

        // 移動中かを調べてフラグを切り替える
        if (direction.x != 0 || direction.z != 0)
        {
            walkFlg_ = true;    // 移動中
        }
        else
        {
            walkFlg_ = false;   // 立ち止まっている
        }

        if (slowWalk_)
        {
            if (speed_ >= speedMax_)
            {
                speed_ /= 2.0f; // slowがtrueなら速度/2にする
            }
        }
        else
        {
            speed_ = speedMax_;
        }

        Vector3 velocity = direction * speed_;
        velocity.y -= gravity_;
        velocity = transform.transform.TransformDirection(velocity);
        controller_.Move(velocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 両方のColliderのIsTrrigerにチェックを入れる
        if (other.gameObject.tag == "Battery")
        {
            // Debug.Log("GetBatteryFlag+++++++電池ゲット");
            batteryGetFlag_ = true;
            Destroy(other.gameObject);            // オブジェクトを削除
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

    public bool GetWalkFlg()
    {
        return walkFlg_;
    }

    public bool GetSlowWalkFlg()
    {
        return slowWalk_;
    }

    public int GetCountMax()
    {
        // 移動速度変更フラグを見て、エフェクトの再生時間を調整する
        if (!slowWalk_)
        {
            return countMax_;
        }
        else
        {
            return countMax_ * 2;
        }

        Debug.Log("GetCountMaxでエラー");
    }
}
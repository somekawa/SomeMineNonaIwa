using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private CharacterController controller;

    [SerializeField]
    private float speed_   = 3.0f;
    private float gravity_ = 9.8f;
    private bool walkFlg_  = false;  // 移動中はtrue
    private bool slowWalk_ = false;  // 移動速度が遅くなる場合はtrue

    // ゆっくり歩く処理が必要
    // 特定のキーの押下状態を調べて、押下ならフラグか何かをtrueにする

    void Start()
    {
        controller = GetComponent<CharacterController>();
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
        if(direction.x != 0 || direction.z != 0)
        {
            walkFlg_ = true;    // 移動中
        }
        else
        {
            walkFlg_ = false;   // 立ち止まっている
        }

        if(slowWalk_)
        {
            if(speed_ >= 3.0f)
            {
                speed_ /= 2.0f; // slowがtrueなら速度/2にする
            }
        }
        else
        {
            speed_ = 3.0f;
        }

        Vector3 velocity = direction * speed_;
        velocity.y -= gravity_;
        velocity = transform.transform.TransformDirection(velocity);
        controller.Move(velocity * Time.deltaTime);
    }

    public bool GetWalkFlg()
    {
        return walkFlg_;
    }

    public bool GetSlowWalkFlg()
    {
        return slowWalk_;
    }
}
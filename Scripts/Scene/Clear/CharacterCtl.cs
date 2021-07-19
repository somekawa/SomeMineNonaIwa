using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCtl : MonoBehaviour
{

    private CharacterController controller_;

    private float speed_ = 4.0f;          // デフォルトの移動速度
    private float gravity_ = 9.8f;        // 重力
    /*カメラ*/
    public float x_sensi;                          // マウスのX座標の感度格納用
    public float y_sensi;                          // マウスのY座標の感度格納用
    public GameObject playerCamera;                // 動かすカメラ格納用
    public CameraController cameraController;

    private float x_Rotation_;
    private float y_Rotation_;

    private bool moveFlag_=false;
    
    void Start()
    {
        controller_ = GetComponent<CharacterController>();
    }

    void Update()
    {
        cameracon();
        CalculateMove();

        if ((this.enabled==true) && (moveFlag_ ==false))
        {
            moveFlag_ = true;
            this.transform.transform.position = new Vector3(15.0f, 1.0f, 4.5f);
        }

    }


    void CalculateMove()
    {
        // 基本移動処理
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);


        Vector3 velocity = direction * speed_;
        velocity.y -= gravity_;
        velocity = transform.transform.TransformDirection(velocity);
        controller_.Move(velocity * Time.deltaTime);

    }

    private void cameracon()
    {
        y_Rotation_ = Input.GetAxis("Mouse Y");                   // マウスのY座標の取得
        y_Rotation_ = y_Rotation_ * y_sensi;                      // カメラのY座標の回転速度の計算
        playerCamera.transform.Rotate(-y_Rotation_, 0, 0);       // Y座標の回転は子オブジェクトのカメラに

        x_Rotation_ = Input.GetAxis("Mouse X");               // マウスのX座標の取得
        x_Rotation_ = x_Rotation_ * x_sensi;                  // カメラのX座標の回転速度の計算
        this.transform.Rotate(0, x_Rotation_, 0);             // X座標の回転はスクリプトをアタッチしているオブジェクトに
    }


}

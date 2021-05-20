using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraControll : MonoBehaviour
{
    public float x_sensi;                          // マウスのX座標の感度格納用
    public float y_sensi;                          // マウスのY座標の感度格納用
    public GameObject playerCamera;                // 動かすカメラ格納用
    public CameraController cameraController_;     

    private float x_Rotation_;
    private float y_Rotation_;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        cameracon();
    }


    private void cameracon()
    {
        if(cameraController_.FullMapFlag()==false)
        {
            x_Rotation_ = Input.GetAxis("Mouse X");                   // マウスのX座標の取得
            y_Rotation_ = Input.GetAxis("Mouse Y");                   // マウスのY座標の取得

            x_Rotation_ = x_Rotation_ * x_sensi;                      // カメラのX座標の回転速度の計算
            y_Rotation_ = y_Rotation_ * y_sensi;                      // カメラのY座標の回転速度の計算
        }

        this.transform.Rotate(0, x_Rotation_, 0);                 // X座標の回転はスクリプトをアタッチしているオブジェクトに
        playerCamera.transform.Rotate(-y_Rotation_, 0, 0);      　// Y座標の回転は子オブジェクトのカメラに
    }
}

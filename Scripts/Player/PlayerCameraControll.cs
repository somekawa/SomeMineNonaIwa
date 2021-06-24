using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraControll : MonoBehaviour
{
    public float x_sensi;                          // マウスのX座標の感度格納用
    public float y_sensi;                          // マウスのY座標の感度格納用
    public GameObject playerCamera;                // 動かすカメラ格納用
    public CameraController cameraController_;
    public playerController playerController_;
    public PauseScript pause;                      // pause中の処理

    private float x_Rotation_;
    private float y_Rotation_;
    private HideControl hideControl_;

    private bool operationFlag_ = true;             // カメラ操作できるか

    // Start is called before the first frame update
    void Start()
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        hideControl_ = GetComponent<HideControl>();
    }

    // Update is called once per frame
    void Update()
    {
        cameracon();
    }


    private void cameracon()
    {
        if (pause.GetPauseFlag() == false || cameraController_.FullMapFlag() == false || ((hideControl_ != null) && (hideControl_.GetHideFlg() == true)))
        {
            //Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;

            if(!operationFlag_)
            {
                // 別のほうでカメラ操作されている
                return;
            }

            y_Rotation_ = Input.GetAxis("Mouse Y");                   // マウスのY座標の取得
            y_Rotation_ = y_Rotation_ * y_sensi;                      // カメラのY座標の回転速度の計算
            playerCamera.transform.Rotate(-y_Rotation_, 0, 0);      　// Y座標の回転は子オブジェクトのカメラに

            if (!playerController_.GetNowLean())
            {
                x_Rotation_ = Input.GetAxis("Mouse X");               // マウスのX座標の取得
                x_Rotation_ = x_Rotation_ * x_sensi;                  // カメラのX座標の回転速度の計算
                this.transform.Rotate(0, x_Rotation_, 0);             // X座標の回転はスクリプトをアタッチしているオブジェクトに
            }
        }
        else
        {
            Cursor.visible = true;                                    // マウスカーソルの表示
            Cursor.lockState = CursorLockMode.None;                   // マウスカーソルの場所の固定解除
        }
    }

    public void SetOperationFlag(bool flag)
    {
        operationFlag_ = flag;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCameraControll : MonoBehaviour
{
    // Component取得用変数
    public playerController playerController;      // コントローラー情報の取得
    private GameScene gameScene_;

    private CameraController cameraController_;    // カメラ情報の取得
    private HideControl hideControl_;              // プレイヤーの隠れている状態の取得

    public float x_sensi;                          // マウスのX座標の感度格納用
    public float y_sensi;                          // マウスのY座標の感度格納用
    public GameObject playerCamera;                // 動かすカメラ格納用

    private float x_Rotation_;                     // カメラの回転(X軸)
    private float y_Rotation_;                     // カメラの回転(Y軸)
    private bool operationFlag_ = true;            // カメラ操作できるか

    private string sceneName_;

    void Start()
    {
        gameScene_ = FindObjectOfType<GameScene>();
        cameraController_ = CameraController.FindObjectOfType<CameraController>();
        hideControl_ = GetComponent<HideControl>();

        Cursor.visible = false;                                    // マウスカーソルの非表示
        Cursor.lockState = CursorLockMode.Locked;                  // マウスカーソルの場所の固定

        sceneName_ = SceneManager.GetActiveScene().name;
    }

    void Update()
    {
        // スタート時のアニメーション中はカメラ操作ができないようにする
        if (gameScene_ != null && gameScene_.GetStartAnimTime() < gameScene_.GetMaxAnimTime() 
            && sceneName_ != "TutorialScene")
        {
            return;
        }

        CameraOn();
    }


    private void CameraOn()
    {
        if (gameScene_.GetPauseFlag() == false || cameraController_.FullMapFlag() == false ||
           ((hideControl_ != null) && (hideControl_.GetHideFlg() == true)))
        {
            Cursor.visible = false;                                    // マウスカーソルの非表示
            Cursor.lockState = CursorLockMode.Locked;                  // マウスカーソルの場所の固定

            if (!operationFlag_)
            {
                // 別のほうでカメラ操作されている
                return;
            }

            y_Rotation_ = Input.GetAxis("Mouse Y");                   // マウスのY座標の取得
            y_Rotation_ = y_Rotation_ * y_sensi;                      // カメラのY座標の回転速度の計算
            playerCamera.transform.Rotate(-y_Rotation_, 0, 0);      　// Y座標の回転は子オブジェクトのカメラに

            if (!playerController.GetNowLean())
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

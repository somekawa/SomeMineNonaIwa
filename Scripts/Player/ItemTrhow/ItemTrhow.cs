using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrhow : MonoBehaviour
{
    private HideControl hideControl_;  // 箱に隠れる処理

    public GameObject wineBottle;      // 生成

    private Vector3 cursorPos_;        // マウスカーソル座標
    private Vector3 cursorPos3d_;      // マウスカーソルの3D座標
    private Vector3 cameraPos_;        // カメラ座標
    private Vector3 throwDirection_;   // ボールを投げる方向
    private const float MaxCursorPosY_ = 1.5f;  // y軸の最大値
    private const float CursorPosZ_    = 1.5f;  // z軸の適当な数値

    private Rigidbody rbball_;         // ボールへ力を加える 
    public float thrust = 10.0f;       // 加える力の設定

    void Start()
    {
        hideControl_ = GetComponent<HideControl>();
    }

    void Update()
    {
        if ((hideControl_ != null) && (hideControl_.GetHideFlg()))
        {
            // 箱に隠れている
            return;
        }

        cameraPos_ = Camera.main.transform.position; // カメラの位置

        cursorPos_ = Input.mousePosition;            // 画面上のカーソルの位置
        cursorPos_.z = CursorPosZ_;                  // z座標に適当な値を入れる
        cursorPos3d_ = Camera.main.ScreenToWorldPoint(cursorPos_); // 3Dの座標になおす

        // 数字が大きすぎるときは上限決める
        if (cursorPos3d_.y > MaxCursorPosY_)
        {
            cursorPos3d_.y = MaxCursorPosY_;
        }

        //Debug.Log(cursorPosition3d.y);

        throwDirection_ = cursorPos3d_ - cameraPos_; // 玉を飛ばす方向

        if (Input.GetMouseButtonDown(1))             // マウスの左クリックをしたとき
        {
            rbball_ = Instantiate(wineBottle, cameraPos_, transform.rotation).GetComponent<Rigidbody>(); // 玉を生成
            rbball_.AddForce(throwDirection_ * thrust, ForceMode.Impulse);                         // カーソルの方向に力を一度加える
        }
    }
}

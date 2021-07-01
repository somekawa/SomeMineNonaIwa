using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrhow : MonoBehaviour
{
    private HideControl hideControl_;           // 箱に隠れる処理
    public PauseScript pause;                   // pause中の処理

    public GameObject wineBottle;               // 生成

    private Vector3 cursorPos_;                 // マウスカーソル座標
    private Vector3 cursorPos3d_;               // マウスカーソルの3D座標
    private Vector3 cameraPos_;                 // カメラ座標
    private Vector3 throwDirection_;            // ボールを投げる方向
    private const float MaxCursorPosY_ = 1.5f;  // y軸の最大値
    private const float CursorPosZ_    = 1.5f;  // z軸の適当な数値

    private Rigidbody rbball_;                  // ボールへ力を加える 
    public float thrust = 10.0f;                // 加える力の設定

    private bool TrhowItemHaveFlg_ = false;     // アイテムを所持しているか確認するフラグ

    void Start()
    {
        hideControl_ = GetComponent<HideControl>();
    }

    void Update()
    {
        if (pause.GetPauseFlag() == true)
        {
            // pause中は何の処理もできないようにする
            return;
        }

        if ((hideControl_ != null) && (hideControl_.GetHideFlg()))
        {
            // 箱に隠れている
            return;
        }

        cameraPos_ = Camera.main.transform.position; // カメラの位置

        //マウス左クリックで発動
        if (Input.GetMouseButtonDown(1))
        {
            if (!TrhowItemHaveFlg_)
            {
                return; // アイテム未所持の場合はreturn
            }
            TrhowItemHaveFlg_ = false;

            rbball_ = Instantiate(wineBottle, cameraPos_, transform.rotation).GetComponent<Rigidbody>(); // 玉を生成
            //前方に向けてthrustだけの力をかける
            rbball_.AddForce(transform.forward * thrust, ForceMode.Impulse);
        }
    }

    public void SetTrhowItemFlg(bool flag)
    {
        // プレイヤーコントロールクラスからセットされるようにする
        TrhowItemHaveFlg_ = flag;
    }

    public bool GetTrhowItemFlg()
    {
        return TrhowItemHaveFlg_;
    }

}

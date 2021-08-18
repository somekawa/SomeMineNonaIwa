using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class tLightRange : MonoBehaviour
{
    public Barrier barrier;                     // バリア状態かを取得する際に使用する
    public CameraAction cameraAction;           // カメラアクションの制御

    private playerController playerController_; // リーン状態の取得
    private SlenderManCtl[] slenderManCtl_;     // 敵の挙動制御

    private bool hitFlag_   = false;            // 敵と遭遇したか

    private bool rangeFlag_ = false;            // 範囲内か
    private float rangeTime_    = 0.0f;         // 範囲内に入ってからの時間
    private float rangeMaxTime_ = 0.5f;         // 範囲内に入ってからの猶予時間

    /*リザルトシーンで使う*/
    public static int hitNum;// 敵を見た回数

    void Start()
    {
        playerController_ = transform.root.gameObject.GetComponent<playerController>();
        slenderManCtl_ = new SlenderManCtl[4];
        hitNum = 0;
    }

    void Update()
    {
        for (int i = 0; i < SlenderSpawner.GetInstance().spawnSlender.Length; i++)
        {
            if (slenderManCtl_[i] == null && SlenderSpawner.GetInstance().slenderManCtl[i] != null)
            {
                slenderManCtl_[i] = SlenderSpawner.GetInstance().slenderManCtl[i];
            }
        }
    }

    void OnDisable()
    {
        // 非アクティブ時、敵が懐中電灯の範囲内だったら正気度低下時の処理を止める
        if (rangeFlag_)
        {
            hitFlag_ = false;
            rangeFlag_ = false;
            cameraAction.OffCameraAction();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Enemy")
        {
            return;         // タグがEnemy以外なら以降の処理を行わない
        }
        else
        {
            if (!rangeFlag_)
            {
                rangeFlag_ = true;
                rangeTime_ = 0.0f;
            }

            if (!playerController_.GetNowLean())
            {
                // カメラが傾いていない場合のみ
                for (int i = 0; i < SlenderSpawner.GetInstance().spawnSlender.Length; i++)
                {
                    if (slenderManCtl_[i] != null)
                    {
                        slenderManCtl_[i].navMeshAgent_.ResetPath();
                        slenderManCtl_[i].status = SlenderManCtl.Status.NULL;

                        slenderManCtl_[i].stopFlag = true;
                        slenderManCtl_[i].status = SlenderManCtl.Status.IDLE;
                    }
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Enemy")
        {
            return;         // タグがEnemy以外なら以降の処理を行わない
        }
        else
        {
            rangeTime_ += Time.deltaTime;
            Debug.Log("範囲内時間：" + rangeTime_);

            cameraAction.SanitCameraAction(other.gameObject);

            if (!cameraAction.CameraLong())
            {
                // slenderMan このタイミングでワープ
                for (int i = 0; i < SlenderSpawner.GetInstance().spawnSlender.Length; i++)
                {
                    if (slenderManCtl_[i] != null)
                    {
                        slenderManCtl_[i].inSightFlag = true;
                    }
                }

                // バリアアイテムを取得しているか確認を行う
                if (barrier.GetBarrierItemFlg())
                {
                    hitFlag_ = false;
                    barrier.SetBarrierItemFlg(false);
                    Debug.Log("防御アイテムを使用しました。");
                }
                else
                {
                    hitFlag_ = true;
                }
                Debug.Log("ライトの範囲内に敵を確認しました。");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Enemy")
        {
            return;         // タグがEnemy以外なら以降の処理を行わない
        }

        //slenderMan このタイミングで動き
        for (int i = 0; i < SlenderSpawner.GetInstance().spawnSlender.Length; i++)
        {
            if (slenderManCtl_[i] != null)
            {
                slenderManCtl_[i].stopFlag = false;
                slenderManCtl_[i].status = SlenderManCtl.Status.WALK;
            }
        }

        hitFlag_ = false;
        rangeFlag_ = false;
        cameraAction.OffCameraAction();
        Debug.Log("敵がライトの範囲外にいきました。");

        hitNum++;
        Debug.Log("hitNum" + hitNum);
    }

    public bool GetHitCheck()
    {
        return hitFlag_;
    }
}

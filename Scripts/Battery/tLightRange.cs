using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class tLightRange : MonoBehaviour
{
    public Barrier barrier;
    public CameraAction cameraAction_;

    private playerController playerController_;

    private SlenderManCtl[] slenderManCtl_;
    private bool hitFlag_ = false;
    private bool rangeFlag_ = false;        // 範囲内か
    private float rangeTime_ = 0.0f;        // 範囲内に入ってからの時間
    private float rangeMaxTime_ = 0.5f;     // 範囲内に入ってからの猶予時間

    void Start()
    {
        playerController_ = transform.root.gameObject.GetComponent<playerController>();

        slenderManCtl_ = new SlenderManCtl[4];

        //slenderMan_=GameObject.Find("Slender");
        //slenderManCtl_ = slenderMan_.GetComponent<SlenderManCtl>();
    }

    void Update()
    {
        for (int i = 0; i < SlenderSpawner.GetInstance().spawnSlender.Length; i++)
        {
            if (slenderManCtl_[i] == null && SlenderSpawner.GetInstance().spawnSlender[i] != null)
            {
                slenderManCtl_[i] = SlenderSpawner.GetInstance().spawnSlender[i].gameObject.GetComponent<SlenderManCtl>();
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
            cameraAction_.OffShake();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Enemy")
        {
            return;
        }

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
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Enemy")
        {
            return;
        }

        rangeTime_ += Time.deltaTime;
        Debug.Log("範囲内時間：" + rangeTime_);

        cameraAction_.SanitCameraAction(other.gameObject);

        if (!cameraAction_.CameraLong())
        {
            //@slenderMan このタイミングでワープお願いします。
            for (int i = 0; i < SlenderSpawner.GetInstance().spawnSlender.Length; i++)
            {
                if (slenderManCtl_[i] != null)
                {
                    slenderManCtl_[i].inSightFlag = true;
                }
            }

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

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Enemy")
        {
            return;
        }

        //@slenderMan このタイミングで動きお願いします
        for (int i = 0; i < SlenderSpawner.GetInstance().spawnSlender.Length; i++)
        {
            if (slenderManCtl_[i] != null)
            {
                slenderManCtl_[i].status = SlenderManCtl.Status.WALK;
            }
        }

        hitFlag_ = false;
        rangeFlag_ = false;
        cameraAction_.OffShake();
        Debug.Log("敵がライトの範囲外にいきました。");
    }

    public bool GetHitCheck()
    {
        return hitFlag_;
    }
}

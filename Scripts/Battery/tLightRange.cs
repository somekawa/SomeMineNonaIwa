using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class tLightRange : MonoBehaviour
{
    public Barrier barrier;
    public CameraShake cameraShake_;

    private GameObject slenderMan_;
    private SlenderManCtl slenderManCtl_;
    private bool hitFlag_       = false;

    private bool rangeFlag_     = false;    // 範囲内か
    private float rangeTime_    = 0.0f;     // 範囲内に入ってからの時間
    private float rangeMaxTime_ = 0.5f;     // 範囲内に入ってからの猶予時間

    void Start()
    {
        slenderMan_=GameObject.Find("Slender");
        slenderManCtl_ = slenderMan_.GetComponent<SlenderManCtl>();
    }

    void Update()
    {
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
        slenderManCtl_.navMeshAgent_.ResetPath();
        slenderManCtl_.status = SlenderManCtl.Status.NULL;
        //slenderManCtl_.inSightFlag = true;       
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Enemy")
        {
            return;
        }

        rangeTime_ += Time.deltaTime;
        Debug.Log("範囲内時間：" + rangeTime_);

        // 画面揺れ処理追加予定
        cameraShake_.Shake(other.gameObject);

        if (rangeTime_ >= rangeMaxTime_)
        {
            //@slenderMan このタイミングでワープお願いします。

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

        //@slenderMan このタイミングで動きお願いします。

        hitFlag_ = false;
        rangeFlag_ = false;
        cameraShake_.OffShake();
        Debug.Log("敵がライトの範囲外にいきました。");
    }

    public bool GetHitCheck()
    {
        return hitFlag_;
    }
}

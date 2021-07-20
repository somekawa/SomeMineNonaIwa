using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallObjFall : MonoBehaviour
{
    private bool rigidBodyFlg = false;  // リジットボディが追加されているかを判断する
    private float time_ = 0.0f;         // リジットボディ追加後の経過時間
    private float maxTime_ = 2.0f;      // オブジェクト消滅までの時間

    // Sliderのワープ関連変数
    //private GameObject[] slenderMan_;
    //private SlenderManCtl[] slenderManCtl_;
    //private float minDistance_;
    //private float nowDistance_;
    //private int minCnt_;

    void Start()
    {
        //slenderMan_ = new GameObject[4];
        //slenderManCtl_ = new SlenderManCtl[4];
    }

    void Update()
    {
        //for (int i = 0; i < SlenderSpawner.GetInstance().spawnSlender.Length; i++)
        //{
        //    if (slenderManCtl_[i] == null && SlenderSpawner.GetInstance().spawnSlender[i] != null)
        //    {
        //        slenderManCtl_[i] = SlenderSpawner.GetInstance().spawnSlender[i].gameObject.GetComponent<SlenderManCtl>();
        //    }
        //}

        // リジットボディが追加されていない間はreturnする
        if (!rigidBodyFlg)
        {
            return;
        }

        time_ += Time.deltaTime;

        // 落下したオブジェクトは、時間経過で消えるようにする
        if (time_ >= maxTime_)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // オブジェクトが範囲内に入っていたらリジットボディを追加して、落下させる
        if (other.gameObject.tag == "RecognitionCylinder")
        {
            gameObject.AddComponent<Rigidbody>();
            rigidBodyFlg = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // 床との接触時に、物が落ちた音を再生
        if(collision.gameObject.name == "floor")
        {
            SlenderSpawner.GetInstance().ClosestObject(this.gameObject, 2, false, false);
            //for (int x = 0; x < SlenderSpawner.GetInstance().spawnSlender.Length; x++)
            //{
            //    if (slenderMan_[x] != null)
            //    {
            //        nowDistance_ = Vector3.Distance(gameObject.transform.position, slenderMan_[x].transform.position);
            //        if (minDistance_ >= nowDistance_)
            //        {
            //            minDistance_ = nowDistance_;
            //            minCnt_ = x;
            //        }
            //    }
            //}
            //if (slenderManCtl_ != null)
            //{
            //    slenderManCtl_[minCnt_].soundPoint.x = this.gameObject.transform.position.x;
            //    slenderManCtl_[minCnt_].soundPoint.z = this.gameObject.transform.position.z;
            //    slenderManCtl_[minCnt_].navMeshAgent_.stoppingDistance = 2;
            //    slenderManCtl_[minCnt_].listenFlag = true;
            //}
            SoundScript.GetInstance().PlaySound(5);
        }
    }
}

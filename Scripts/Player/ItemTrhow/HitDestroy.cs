using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDestroy : MonoBehaviour
{
    public string targeTag;

    //private GameObject[] slenderMan_;
    //private SlenderManCtl[] slenderManCtl_;
    //private float minDistance_;
    //private float nowDistance_;
    //private int minCnt_;

    void Start()
    {
        //slenderMan_ = new GameObject[4];
        //slenderManCtl_ = new SlenderManCtl[4];
        //for (int i = 0; i < SlenderSpawner.GetInstance().spawnSlender.Length; i++)
        //{
        //    if (SlenderSpawner.GetInstance().spawnSlender[i] != null)
        //    {
        //        slenderMan_[i] = SlenderSpawner.GetInstance().spawnSlender[i];
        //        slenderManCtl_[i] = slenderMan_[i].gameObject.GetComponent<SlenderManCtl>();
        //    }
        //}
        //minDistance_ = 0;
        //minCnt_ = 0;
    }

    void OnCollisionEnter(Collision collision)
    {
        // このScriptがアタッチされているオブジェクトが、指定したターゲットに接触した時
        // このオブジェクトが消滅する
        if (collision.gameObject.tag == targeTag)
        {
            SlenderSpawner.GetInstance().ClosestObject(this.gameObject, 0, false, false);
            //for (int i = 0; i < SlenderSpawner.GetInstance().spawnSlender.Length; i++)
            //{
            //    if (slenderMan_[i] != null)
            //    {
            //        nowDistance_ = Vector3.Distance(gameObject.transform.position, slenderMan_[i].transform.position);
            //        if (minDistance_ >= nowDistance_)
            //        {
            //            minDistance_ = nowDistance_;
            //            minCnt_ = i;
            //        }
            //    }
            //}

            //if(slenderManCtl_!=null)
            //{
            //    slenderManCtl_[minCnt_].soundPoint.x = this.gameObject.transform.position.x;
            //    slenderManCtl_[minCnt_].soundPoint.z = this.gameObject.transform.position.z;
            //    slenderManCtl_[minCnt_].listenFlag = true;
            //}

            //GameObject obj = (GameObject)Resources.Load("GlassSE");
            //if (obj == null)
            //{
            //    Debug.Log("objがnullです");
            //}

            //Instantiate(obj);

            if (SoundScript.GetInstance() != null)
            {
                SoundScript.GetInstance().PlaySound(4);
            }

            Destroy(this.gameObject);
        }
    }
}

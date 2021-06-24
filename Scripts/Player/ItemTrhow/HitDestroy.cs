using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDestroy : MonoBehaviour
{
    public string targeTag;

    private GameObject[] slenderMan_;
    private SlenderManCtl[] slenderManCtl_;
    private float minDistance_;
    private float nowDistance_;
    private int minCnt_;
    private int lengthCnt_;

    void Start()
    {
        lengthCnt_ = SlenderSpawner.GetInstance().spawnSlender.Length;
        slenderMan_ = new GameObject[lengthCnt_];
        slenderManCtl_ = new SlenderManCtl[lengthCnt_];
        for (int i = 0; i < lengthCnt_; i++)
        {
            if (SlenderSpawner.GetInstance().spawnSlender[i] != null)
            {
                slenderMan_[i] = SlenderSpawner.GetInstance().spawnSlender[i];
                slenderManCtl_[i] = slenderMan_[i].gameObject.GetComponent<SlenderManCtl>();
            }
        }
        minDistance_ = 0;
        minCnt_ = 0;
    }

    void OnCollisionEnter(Collision collision)
    {
        // このScriptがアタッチされているオブジェクトが、指定したターゲットに接触した時
        // このオブジェクトが消滅する
        if (collision.gameObject.tag == targeTag)
        {
            for (int i = 0; i < lengthCnt_; i++)
            {
                if (slenderMan_[i] != null)
                {
                    nowDistance_ = Vector3.Distance(gameObject.transform.position, slenderMan_[i].transform.position);
                    if (minDistance_ >= nowDistance_)
                    {
                        minDistance_ = nowDistance_;
                        minCnt_ = i;
                    }
                }
            }

            if(slenderManCtl_!=null)
            {
                slenderManCtl_[minCnt_].soundPoint.x = this.gameObject.transform.position.x;
                slenderManCtl_[minCnt_].soundPoint.z = this.gameObject.transform.position.z;
                slenderManCtl_[minCnt_].listenFlag = true;
            }

            GameObject obj = (GameObject)Resources.Load("GlassSE");
            if (obj == null)
            {
                Debug.Log("objがnullです");
            }

            Instantiate(obj);
            Destroy(this.gameObject);
        }
    }
}

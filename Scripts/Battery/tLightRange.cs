using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class tLightRange : MonoBehaviour
{
    private GameObject slenderMan_;
    private SlenderManCtl slenderManCtl_;
    private bool hitCheck_  = false;

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
        if (other.gameObject.tag == "Enemy")
        {
            slenderManCtl_.navMeshAgent_.ResetPath();
            slenderManCtl_.status = SlenderManCtl.Status.NULL;
            //slenderManCtl_.inSightFlag = true;
            hitCheck_ = true;
            Debug.Log("ライトの範囲内に敵を確認しました。");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            hitCheck_ = false;
            Debug.Log("敵がライトの範囲外にいきました。");
        }
    }

    public bool GetHitCheck()
    {
        return hitCheck_;
    }
}

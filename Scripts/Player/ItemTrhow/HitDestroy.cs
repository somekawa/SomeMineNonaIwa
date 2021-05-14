using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDestroy : MonoBehaviour
{
    public string targeTag;

    private GameObject slenderMan_;
    private SlenderManCtl slenderManCtl_;

    void Start()
    {
        slenderMan_ = GameObject.Find("Slender");
        slenderManCtl_ = slenderMan_.gameObject.GetComponent<SlenderManCtl>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // このScriptがアタッチされているオブジェクトが、指定したターゲットに接触した時
        // このオブジェクトが消滅する
        if (collision.gameObject.tag == targeTag)
        {
            slenderManCtl_.soundPoint.x = this.gameObject.transform.position.x;
            slenderManCtl_.soundPoint.z = this.gameObject.transform.position.z;
            slenderManCtl_.listenFlag = true;

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

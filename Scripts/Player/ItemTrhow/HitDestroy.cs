using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDestroy : MonoBehaviour
{
    public string targeTag;

    void Start()
    {
    }

    void OnCollisionEnter(Collision collision)
    {
        // このScriptがアタッチされているオブジェクトが、指定したターゲットに接触した時
        // このオブジェクトが消滅する
        if (collision.gameObject.tag == targeTag)
        {
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

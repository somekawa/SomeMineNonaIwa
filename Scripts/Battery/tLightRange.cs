using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tLightRange : MonoBehaviour
{
    private bool hitCheck_ = false;

    void Start()
    {
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            hitCheck_ = true;
            Debug.Log("ライトの範囲内に敵を確認しました。");
       }
    }
}

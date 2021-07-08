using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallObjFall : MonoBehaviour
{
    private bool rigidBodyFlg = false;  // リジットボディが追加されているかを判断する
    private float time_ = 0.0f;         // リジットボディ追加後の経過時間
    private float maxTime_ = 2.0f;      // オブジェクト消滅までの時間

    void Start()
    {
    }

    void Update()
    {
        // リジットボディが追加されていない間はreturnする
        if(!rigidBodyFlg)
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
            SoundScript.GetInstance().PlaySound(5);
        }
    }
}

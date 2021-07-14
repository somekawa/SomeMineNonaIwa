using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchBox : MonoBehaviour
{
    // マネキンの腕が伸びてくる処理
    //public GameObject arm;
    //private bool moveFlg_ = false;

    public GameObject instanseSpot; // ワインボトルの落下開始位置(cubeとかで設置)
    public GameObject bottle;       // ワインボトルをここにいれる(ワインボトルにはrigidbodyをつけておく)
    private bool switchFlg_ = false;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // マネキンの腕が伸びてくる処理
        //if(!moveFlg_)
        //{
        //    return;
        //}
        //if(arm.transform.position.y < 0.6f)
        //{
        //    arm.transform.position += new Vector3(0, 0.05f, 0);
        //}

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ItemHitArea")
        {
            // 動的に1度だけ生成したいワインボトルの処理
            if(!switchFlg_)
            {
                // 指定したInstanceSpotからワインボトルが生成されて落ちてくるようにする
                Instantiate(bottle, instanseSpot.transform.position, Quaternion.identity);

                switchFlg_ = true;

                // オブジェクトを削除する
                Destroy(this.gameObject);
            }

            // マネキンの処理
            //Debug.Log("boxと衝突");
            //if(!moveFlg_)
            //{
            //    moveFlg_ = true;
            //}
        }
    }
}

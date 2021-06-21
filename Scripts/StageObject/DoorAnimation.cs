using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    public GameObject door;             // 回転するオブジェクト格納用
    public bool openFlag;               // 開くためのフラグ
    public bool closeFlag;              // 閉じるためのフラグ

    private float minAngle_;            // 回転する前の角度
    private float maxAngle_;            // 回転した後の角度
    private float angle_;               // 回転角度格納用変数

    // Start is called before the first frame update
    void Start()
    {
        minAngle_ = door.transform.rotation.y;
        maxAngle_ = 90;
        openFlag = false;
        closeFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 鎖と南京錠の数を調べて、0以下の時に扉がアニメーションするようにフラグを立てる
        if(transform.Find("Door").childCount <= 0)
        {
            openFlag = true;
        }

        if (openFlag == true)
        {
            angle_ = Mathf.LerpAngle(minAngle_, maxAngle_, Time.time);
            door.transform.eulerAngles = new Vector3(0, angle_, 0);
            if(door.transform.eulerAngles == new Vector3(0, maxAngle_, 0))
            {
                openFlag = false;
            }
        }
        else if(closeFlag == true)
        {
            angle_ = Mathf.LerpAngle(maxAngle_, minAngle_, Time.time);
            door.transform.eulerAngles = new Vector3(0, angle_, 0);
            if (door.transform.eulerAngles == new Vector3(0, minAngle_, 0))
            {
                closeFlag = false;
            }
        }
        else
        {
            return;
        }
    }
}

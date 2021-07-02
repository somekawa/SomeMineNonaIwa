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
    public float angle_;               // 回転角度格納用変数
    private bool flag;

    // Start is called before the first frame update
    void Start()
    {
        minAngle_ = 0f;
        maxAngle_ = -5f;
        openFlag = false;
        closeFlag = false;
        flag = false;
        angle_ = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    openFlag = true;
        //}
        // 鎖と南京錠の数を調べて、0以下の時に扉がアニメーションするようにフラグを立てる
        if (door.transform.childCount <= 0&&flag==false)
        {
            flag = true;
            openFlag = true;
        }

        if (openFlag == true)
        {
            float step = -120f * Time.deltaTime;
            angle_ += step;
            //指定した方向にゆっくり回転する場合
            door.transform.rotation = Quaternion.RotateTowards(door.transform.rotation, Quaternion.Euler(0, maxAngle_, 0), step);
            //door.transform.Rotate(0f, step, 0f);

            if (angle_ <= -90f)
            {
                openFlag = false;
            }
        }
        else if(closeFlag == true)
        {
            float step = 120f * Time.deltaTime;
            //指定した方向にゆっくり回転する場合
            door.transform.rotation = Quaternion.RotateTowards(door.transform.rotation, Quaternion.Euler(0, minAngle_, 0), step);
            //door.transform.Rotate(0f, step, 0f);

            if (door.transform.rotation.y >= minAngle_)
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
